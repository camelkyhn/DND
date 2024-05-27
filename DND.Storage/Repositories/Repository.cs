using DND.Middleware.Base;
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

namespace DND.Storage.Repositories
{
    public class Repository<TContext, TKey, TUserKey, TAuditedEntity, TAuditedEntityDto, TFilterDto> : IRepository<TKey, TUserKey, TAuditedEntity, TAuditedEntityDto, TFilterDto>
        where TContext : DbContext
        where TAuditedEntity : AuditedEntity<TKey, TUserKey>
        where TAuditedEntityDto : AuditedEntityDto<TKey, TUserKey>
        where TFilterDto : FilterDto
    {
        protected readonly TContext Context;
        private readonly IMapper _mapper;

        protected Repository(TContext context, IMapper mapper)
        {
            Context = context;
            _mapper = mapper;
        }

        public virtual async Task<TAuditedEntity> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(id, cancellationToken);
        }

        public virtual async Task<List<TAuditedEntity>> GetListAsync(TFilterDto filter, CancellationToken cancellationToken = default)
        {
            return await Filter(Context.Set<TAuditedEntity>(), filter).ToListAsync(cancellationToken);
        }

        public virtual async Task<TAuditedEntity> GetAsSelectedAsync(TKey id, Expression<Func<TAuditedEntity, TAuditedEntity>> selectExpression, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<TAuditedEntity>().Select(selectExpression).FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TAuditedEntity));
            }

            return entity;
        }

        public virtual async Task<List<TAuditedEntity>> GetListAsSelectedAsync(TFilterDto filter, Expression<Func<TAuditedEntity, TAuditedEntity>> selectExpression, CancellationToken cancellationToken = default)
        {
            return await Filter(Context.Set<TAuditedEntity>().Select(selectExpression), filter).ToListAsync(cancellationToken: cancellationToken);
        }

        public virtual IQueryable<TAuditedEntity> Filter(IQueryable<TAuditedEntity> queryableSet, TFilterDto filter)
        {
            if (filter.CreatedBeforeDate != null && filter.CreatedBeforeDate != DateTimeOffset.MinValue)
            {
                queryableSet = queryableSet.Where(entity => entity.CreationTime.Date < filter.CreatedBeforeDate.Value.Date);
            }

            if (filter.CreatedAfterDate != null && filter.CreatedAfterDate != DateTimeOffset.MinValue)
            {
                queryableSet = queryableSet.Where(entity => entity.CreationTime.Date > filter.CreatedAfterDate.Value.Date);
            }

            if (filter.ModifiedBeforeDate != null && filter.ModifiedBeforeDate != DateTimeOffset.MinValue)
            {
                queryableSet = queryableSet.Where(entity => entity.ModificationTime.Value.Date < filter.ModifiedBeforeDate.Value.Date);
            }

            if (filter.ModifiedAfterDate != null && filter.ModifiedAfterDate != DateTimeOffset.MinValue)
            {
                queryableSet = queryableSet.Where(entity => entity.ModificationTime.Value.Date > filter.ModifiedAfterDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(filter.CreatorUserEmail))
            {
                queryableSet = queryableSet.Where(entity => entity.CreatorUser.Email.ToLower().Contains(filter.CreatorUserEmail.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.ModifierUserEmail))
            {
                queryableSet = queryableSet.Where(entity => entity.ModifierUser.Email.ToLower().Contains(filter.ModifierUserEmail.ToLower()));
            }

            if (filter.IsDeleted != null)
            {
                queryableSet = queryableSet.Where(entity => entity.IsDeleted == filter.IsDeleted);
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

        public virtual async Task<TAuditedEntity> CreateAsync(TAuditedEntityDto dto, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<TAuditedEntity>(dto);
            entity.CreatorUserId = dto.CreatorUserId;
            entity.CreationTime = DateTimeOffset.UtcNow;
            entity.ModifierUserId = default;
            entity.ModificationTime = null;
            entity.DeletorUserId = default;
            entity.DeletionTime = null;
            await Context.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public virtual async Task<TAuditedEntity> UpdateAsync(TAuditedEntityDto dto, CancellationToken cancellationToken = default)
        {
            var oldEntity = await FindAsync(dto.Id, cancellationToken);
            var creatorId = oldEntity.CreatorUserId;
            var creationTime = oldEntity.CreationTime;
            Context.Attach(oldEntity);
            oldEntity = _mapper.Map(dto, oldEntity);
            oldEntity.CreatorUserId = creatorId;
            oldEntity.CreationTime = creationTime;
            oldEntity.ModificationTime = DateTimeOffset.UtcNow;
            oldEntity.DeletorUserId = default;
            oldEntity.DeletionTime = null;
            await Context.SaveChangesAsync(cancellationToken);
            return oldEntity;
        }

        public virtual async Task DeleteAsync(TAuditedEntityDto dto, CancellationToken cancellationToken = default)
        {
            var oldEntity = await FindAsync(dto.Id, cancellationToken);
            var creatorId = oldEntity.CreatorUserId;
            var creationTime = oldEntity.CreationTime;
            var modifierId = oldEntity.ModifierUserId;
            var modificationTime = oldEntity.ModificationTime;
            Context.Attach(oldEntity);
            oldEntity = _mapper.Map(dto, oldEntity);
            oldEntity.CreatorUserId = creatorId;
            oldEntity.CreationTime = creationTime;
            oldEntity.ModifierUserId = modifierId;
            oldEntity.ModificationTime = modificationTime;
            oldEntity.DeletorUserId = dto.DeletorUserId;
            oldEntity.DeletionTime = DateTimeOffset.UtcNow;
            await Context.SaveChangesAsync(cancellationToken);
        }

        private async Task<TAuditedEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<TAuditedEntity>().FindAsync(new object[] { id }, cancellationToken: cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException(nameof(TAuditedEntity));
            }

            return entity;
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
    }
}
