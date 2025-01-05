using DND.Middleware.Base.Dto;
using DND.Middleware.Base.Entity;
using DND.Middleware.Base.Filter;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DND.Storage.IRepositories
{
    public interface IRepository<TKey, TEntity> where TKey : struct, IEquatable<TKey> where TEntity : class, IEntity<TKey>
    {
        IQueryable<TEntity> GetTableAsQueryable();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
        Task<TEntity> GetAsSelectedAsync(TKey id, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default);
        Task<TEntity> CreateAsync<TEntityDto>(TEntityDto dto, CancellationToken cancellationToken = default) where TEntityDto : class, IEntityDto<TKey?>;
        Task<TEntity> UpdateAsync<TEntityDto>(TEntityDto dto, CancellationToken cancellationToken = default) where TEntityDto : class, IEntityDto<TKey?>;
        Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CreateSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);
        Task RollbackToSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }

    public interface IRepository<TKey, TEntity, in TFilterDto> : IRepository<TKey, TEntity>
        where TKey : struct, IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
        where TFilterDto : FilterDto
    {
        Task<List<TEntity>> GetListAsync(TFilterDto filter, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetListAsSelectedAsync(TFilterDto filter, Expression<Func<TEntity, TEntity>> selectExpression, CancellationToken cancellationToken = default);
        IQueryable<TEntity> Filter(IQueryable<TEntity> queryableSet, TFilterDto filter);
    }
}
