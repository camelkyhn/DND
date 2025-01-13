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
using DND.Middleware.Extensions;
using DND.Middleware.Web;

namespace DND.Storage.Repositories
{
    public interface IRepository<TKey, TEntity> : IDisposable where TKey : struct, IEquatable<TKey> where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> GetTableAsQueryable();

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the <see cref="TEntity"/> entity by id.
        /// </summary>
        /// <param name="id">The primary key of the table.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task.</param>
        /// <returns>Returns the <see cref="TEntity"/> by id.</returns>
        /// <exception cref="NotFoundException">If the search result is null.</exception>
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the <see cref="TEntity"/> entity by id.
        /// </summary>
        /// <param name="id">The primary key of the table.</param>
        /// <param name="selectExpression">A projection function to apply to each.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task.</param>
        /// <returns>Returns the <see cref="TEntity"/> by id.</returns>
        /// <exception cref="NotFoundException">If the search result is null.</exception>
        Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the <see cref="TEntity"/> entity by id.
        /// </summary>
        /// <param name="id">The primary key of the table.</param>
        /// <param name="selectExpression">A projection function to apply to each.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task.</param>
        /// <returns>Returns the <see cref="TEntity"/> by id.</returns>
        /// <exception cref="NotFoundException">If the search result is null.</exception>
        Task<TResult> GetAsSelectedAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selectExpression, CancellationToken cancellationToken = default);

        Task<TEntity> CreateAsync<TEntityDto>(TEntityDto dto, CancellationToken cancellationToken = default) where TEntityDto : class, IEntityDto<TKey?>;

        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync<TEntityDto>(TEntityDto dto, CancellationToken cancellationToken = default) where TEntityDto : class, IEntityDto<TKey?>;

        Task<TEntity> UpdateAsync(TEntity updatedEntity, CancellationToken cancellationToken = default);

        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CreateSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);

        Task RollbackToSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }

    public abstract class Repository<TContext, TKey, TEntity> : IRepository<TKey, TEntity>
        where TContext : DbContext
        where TKey : struct, IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        protected readonly TContext Context;
        protected readonly AppSession Session;
        protected readonly IMapper Mapper;
        protected readonly DbSet<TEntity> Table;

        protected Repository(TContext context, AppSession session, IMapper mapper)
        {
            Context = context;
            Session = session;
            Mapper = mapper;
            Table = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetTableAsQueryable()
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                return Table.Where(x => !((IDeletionAuditedEntity)x).IsDeleted);
            }

            return Table.AsQueryable();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                return await Table.Where(x => !((IDeletionAuditedEntity)x).IsDeleted).FirstOrDefaultAsync(expression, cancellationToken);
            }

            return await Table.FirstOrDefaultAsync(expression, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(id, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default)
        {
            TEntity entity;
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                entity = await Table.Where(e => !((IDeletionAuditedEntity)e).IsDeleted).Select(selectExpression).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
            }
            else
            {
                entity = await Table.Select(selectExpression).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
            }

            if (entity == null)
            {
                throw new NotFoundException(typeof(TEntity).Name);
            }

            return entity;
        }

        public virtual async Task<TResult> GetAsSelectedAsync<TResult>(TKey id, Expression<Func<TEntity, TResult>> selectExpression, CancellationToken cancellationToken = default)
        {
            TResult entityResult;
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                entityResult = await Table.Where(e => !((IDeletionAuditedEntity)e).IsDeleted && e.Id.Equals(id)).Select(selectExpression).FirstOrDefaultAsync(cancellationToken);
            }
            else
            {
                entityResult = await Table.Where(e => e.Id.Equals(id)).Select(selectExpression).FirstOrDefaultAsync(cancellationToken);
            }

            if (entityResult == null)
            {
                throw new NotFoundException(typeof(TEntity).Name);
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

        public virtual async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
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
                oldEntity.SetPropertyValue(nameof(IModificationAuditedEntity.LastModificationTime), DateTime.UtcNow);
                oldEntity.SetPropertyValue(nameof(IModificationAuditedEntity.LastModifierUserId), Session.UserId);
            }

            await Context.SaveChangesAsync(cancellationToken);
            return oldEntity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity updatedEntity, CancellationToken cancellationToken = default)
        {
            var oldEntity = await FindAsync(updatedEntity.Id, cancellationToken);
            AttachIfNot(oldEntity);
            oldEntity = Mapper.Map(updatedEntity, oldEntity);
            if (typeof(TEntity).IsAssignableFrom(typeof(IModificationAuditedEntity)))
            {
                oldEntity.SetPropertyValue(nameof(IModificationAuditedEntity.LastModificationTime), DateTime.UtcNow);
                oldEntity.SetPropertyValue(nameof(IModificationAuditedEntity.LastModifierUserId), Session.UserId);
            }

            await Context.SaveChangesAsync(cancellationToken);
            return oldEntity;
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var oldEntity = await FindAsync(id, cancellationToken);
            AttachIfNot(oldEntity);
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                oldEntity.SetPropertyValue(nameof(IDeletionAuditedEntity.IsDeleted), true);
                oldEntity.SetPropertyValue(nameof(IDeletionAuditedEntity.DeletionTime), DateTime.UtcNow);
                oldEntity.SetPropertyValue(nameof(IDeletionAuditedEntity.DeleterUserId), Session.UserId);
            }
            else
            {
                Context.Remove(oldEntity);
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
            TEntity entity;
            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                entity = await Table.Where(e => !((IDeletionAuditedEntity)e).IsDeleted).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken: cancellationToken);
            }
            else
            {
                entity = await Table.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
            }

            if (entity == null)
            {
                throw new NotFoundException(typeof(TEntity).Name);
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

        #region Dispose

        private bool _isDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }

            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

    public interface IRepository<TKey, TEntity, in TFilterDto> : IRepository<TKey, TEntity>
        where TKey : struct, IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
        where TFilterDto : FilterDto
    {
        Task<List<TEntity>> GetListAsync(TFilterDto filter, CancellationToken cancellationToken = default);

        Task<List<TEntity>> GetListAsSelectedAsync(TFilterDto filter, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default);

        Task<List<TResult>> GetListAsSelectedAsync<TResult>(TFilterDto filter, Expression<Func<TEntity, TResult>> selectExpression, CancellationToken cancellationToken = default);

        IQueryable<TEntity> Filter(IQueryable<TEntity> queryableSet, TFilterDto filter);
    }

    public abstract class Repository<TContext, TKey, TEntity, TFilterDto> : Repository<TContext, TKey, TEntity>, IRepository<TKey, TEntity, TFilterDto>
        where TContext : DbContext
        where TKey : struct, IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
        where TFilterDto : FilterDto
    {
        protected Repository(TContext context, AppSession session, IMapper mapper) : base(context, session, mapper)
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

        public virtual async Task<List<TResult>> GetListAsSelectedAsync<TResult>(TFilterDto filter, Expression<Func<TEntity, TResult>> selectExpression, CancellationToken cancellationToken = default)
        {
            return await Filter(Table, filter).Select(selectExpression).ToListAsync(cancellationToken);
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
                    queryableSet = queryableSet.Where(entity => ((IModificationAuditedEntity)entity).LastModificationTime.Value.Date < filter.ModifiedBeforeDate.Value.Date);
                }

                if (filter.ModifiedAfterDate != null && filter.ModifiedAfterDate != DateTime.MinValue)
                {
                    queryableSet = queryableSet.Where(entity => ((IModificationAuditedEntity)entity).LastModificationTime.Value.Date > filter.ModifiedAfterDate.Value.Date);
                }

                if (!string.IsNullOrEmpty(filter.ModifierUserEmail))
                {
                    queryableSet = queryableSet.Where(entity => ((IModificationAuditedEntity)entity).LastModifierUser.Email.ToLower().Contains(filter.ModifierUserEmail.ToLower()));
                }
            }

            if (typeof(TEntity).IsAssignableFrom(typeof(IDeletionAuditedEntity)))
            {
                if (filter.IsDeleted != null)
                {
                    queryableSet = queryableSet.Where(entity => ((IDeletionAuditedEntity)entity).IsDeleted == filter.IsDeleted);
                }
                else
                {
                    queryableSet = queryableSet.Where(entity => !((IDeletionAuditedEntity)entity).IsDeleted);
                }
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
