using DND.Middleware.Base.Dto;
using DND.Middleware.Base.Entity;
using DND.Middleware.Base.Filter;
using DND.Middleware.Exceptions;
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
}
