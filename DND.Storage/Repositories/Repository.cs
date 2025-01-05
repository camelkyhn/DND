using DND.Storage.IRepositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DND.Middleware.Exceptions;
using AutoMapper;
using DND.Middleware.Base.Dto;
using DND.Middleware.Base.Filter;
using DND.Middleware.Base.Entity;
using DND.Middleware.Identity;
using DND.Middleware.Extensions;

namespace DND.Storage.Repositories
{
    public class Repository<TContext, TKey, TEntity> : IRepository<TKey, TEntity>
        where TContext : DbContext
        where TKey : struct, IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly TContext Context;
        protected readonly IAppSession Session;
        protected readonly IMapper Mapper;
        protected readonly DbSet<TEntity> Table;

        protected Repository(TContext context, IAppSession session, IMapper mapper)
        {
            Context = context;
            Session = session;
            Mapper = mapper;
            Table = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetTableAsQueryable()
        {
            return Table.AsQueryable();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await Table.FirstOrDefaultAsync(expression, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(id, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default)
        {
            var entity = await Table.Where(e => !e.IsDeleted).Select(selectExpression).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            return entity;
        }

        public virtual async Task<TResult> GetAsSelectedAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selectExpression, CancellationToken cancellationToken = default)
        {
            var entityResult = await Table.Where(e => !e.IsDeleted && e.Id.Equals(id)).Select(selectExpression).FirstOrDefaultAsync(cancellationToken);
            if (entityResult == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            return entityResult;
        }

        public virtual async Task<TEntity> CreateAsync<TEntityDto>(TEntityDto dto, CancellationToken cancellationToken = default) where TEntityDto : class, IEntityDto<TKey?>
        {
            var entity = Mapper.Map<TEntity>(dto);
            if (typeof(TEntity).IsAssignableFrom(typeof(ICreationAuditedEntity)))
            {
                entity.SetPropertyValue(nameof(ICreationAuditedEntity.CreationTime), DateTime.UtcNow);
                entity.SetPropertyValue(nameof(ICreationAuditedEntity.CreatorUserId), Session.UserId);
            }

            await Context.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync<TEntityDto>(TEntityDto dto, CancellationToken cancellationToken = default) where TEntityDto : class, IEntityDto<TKey?>
        {
            var oldEntity = await FindAsync(dto.Id.GetValueOrDefault(), cancellationToken);
            AttachIfNot(oldEntity);
            oldEntity = Mapper.Map(dto, oldEntity);
            if (typeof(TEntity).IsAssignableFrom(typeof(IModificationAuditedEntity)))
            {
                oldEntity.SetPropertyValue(nameof(IModificationAuditedEntity.ModificationTime), DateTime.UtcNow);
                oldEntity.SetPropertyValue(nameof(IModificationAuditedEntity.ModifierUserId), Session.UserId);
            }

            await Context.SaveChangesAsync(cancellationToken);
            return oldEntity;
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var oldEntity = await FindAsync(id, cancellationToken);
            AttachIfNot(oldEntity);
            oldEntity.IsDeleted = true;
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                oldEntity.SetPropertyValue(nameof(IDeletionAuditedEntity.DeletionTime), DateTime.UtcNow);
                oldEntity.SetPropertyValue(nameof(IDeletionAuditedEntity.DeleterUserId), Session.UserId);
            }

            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CreateSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default)
        {
            await transaction.CreateSavepointAsync(pointName, cancellationToken);
        }

        public async Task RollbackToSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default)
        {
            await transaction.RollbackToSavepointAsync(pointName, cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await Context.Database.CommitTransactionAsync(cancellationToken);
        }

        private async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await Table.Where(e => !e.IsDeleted).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TEntity));
            }

            return entity;
        }

        private void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }
    }

    public class Repository<TContext, TKey, TEntity, TFilterDto> : Repository<TContext, TKey, TEntity>, IRepository<TKey, TEntity, TFilterDto>
        where TContext : DbContext
        where TKey : struct, IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
        where TFilterDto : FilterDto
    {

        protected Repository(TContext context, IAppSession session, IMapper mapper) : base(context, session, mapper)
        {
        }

        public virtual async Task<List<TEntity>> GetListAsync(TFilterDto filter, CancellationToken cancellationToken = default)
        {
            return await Filter(Table, filter).ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetListAsSelectedAsync(TFilterDto filter, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default)
        {
            return await Filter(Table.Select(selectExpression), filter).ToListAsync(cancellationToken);
        }

        public virtual IQueryable<TEntity> Filter(IQueryable<TEntity> queryableSet, TFilterDto filter)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(ICreationAuditedEntity)))
            {
                if (filter.CreatedBeforeDate != null && filter.CreatedBeforeDate != DateTime.MinValue)
                {
                    queryableSet = queryableSet.Where(entity => ((ICreationAuditedEntity)entity).CreationTime.Date < filter.CreatedBeforeDate.Value.Date);
                }

                if (filter.CreatedAfterDate != null && filter.CreatedAfterDate != DateTime.MinValue)
                {
                    queryableSet = queryableSet.Where(entity => ((ICreationAuditedEntity)entity).CreationTime.Date > filter.CreatedAfterDate.Value.Date);
                }

                if (!string.IsNullOrEmpty(filter.CreatorUserEmail))
                {
                    queryableSet = queryableSet.Where(entity => ((ICreationAuditedEntity)entity).CreatorUser.Email.ToLower().Contains(filter.CreatorUserEmail.ToLower()));
                }
            }

            if (typeof(TEntity).IsAssignableFrom(typeof(IModificationAuditedEntity)))
            {
                if (filter.ModifiedBeforeDate != null && filter.ModifiedBeforeDate != DateTime.MinValue)
                {
                    queryableSet = queryableSet.Where(entity => ((IModificationAuditedEntity)entity).ModificationTime.Value.Date < filter.ModifiedBeforeDate.Value.Date);
                }

                if (filter.ModifiedAfterDate != null && filter.ModifiedAfterDate != DateTime.MinValue)
                {
                    queryableSet = queryableSet.Where(entity => ((IModificationAuditedEntity)entity).ModificationTime.Value.Date > filter.ModifiedAfterDate.Value.Date);
                }

                if (!string.IsNullOrEmpty(filter.ModifierUserEmail))
                {
                    queryableSet = queryableSet.Where(entity => ((IModificationAuditedEntity)entity).ModifierUser.Email.ToLower().Contains(filter.ModifierUserEmail.ToLower()));
                }
            }

            if (filter.IsDeleted != null)
            {
                queryableSet = queryableSet.Where(entity => entity.IsDeleted == filter.IsDeleted);
            }
            else
            {
                queryableSet = queryableSet.Where(entity => !entity.IsDeleted);
            }

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                var descending = filter.SortBy.ToLower().Contains(" desc");
                if (descending)
                {
                    var sortBy = filter.SortBy.Substring(0, filter.SortBy.Length - 5);
                    queryableSet = queryableSet.OrderByDescending(entity => EF.Property<object>(entity, sortBy));
                }
                else
                {
                    queryableSet = queryableSet.OrderBy(entity => EF.Property<object>(entity, filter.SortBy));
                }
            }

            filter.TotalCount = queryableSet.Count();
            if (filter.IsAllData)
            {
                return queryableSet;
            }

            if (filter.PageSize > 0 && filter.PageNumber > 0)
            {
                queryableSet = queryableSet.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);
            }

            return queryableSet;
        }
    }
}
