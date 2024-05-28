using DND.Middleware.Base;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DND.Storage.IRepositories
{
    public interface IRepository<in TKey, in TUserKey, TAuditedEntity, in TAuditedEntityDto, in TFilterDto>
        where TKey : struct, IEquatable<TKey>
        where TAuditedEntity : AuditedEntity<TKey, TUserKey>
        where TAuditedEntityDto : AuditedEntityDto<TKey?, TUserKey>
        where TFilterDto : FilterDto
    {
        Task<TAuditedEntity> GetAsync(TKey id, CancellationToken cancellationToken = default);
        Task<List<TAuditedEntity>> GetListAsync(TFilterDto filter, CancellationToken cancellationToken = default);
        Task<TAuditedEntity> GetAsSelectedAsync(TKey id, Expression<Func<TAuditedEntity, TAuditedEntity>> selectExpression, CancellationToken cancellationToken = default);
        Task<List<TAuditedEntity>> GetListAsSelectedAsync(TFilterDto filter, Expression<Func<TAuditedEntity, TAuditedEntity>> selectExpression, CancellationToken cancellationToken = default);
        IQueryable<TAuditedEntity> Filter(IQueryable<TAuditedEntity> queryableSet, TFilterDto filter);
        Task<TAuditedEntity> CreateAsync(TAuditedEntityDto dto, CancellationToken cancellationToken = default);
        Task<TAuditedEntity> UpdateAsync(TAuditedEntityDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(TAuditedEntityDto dto, CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CreateSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);
        Task RollbackToSavepointAsync(IDbContextTransaction transaction, string pointName, CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }
}
