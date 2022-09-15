﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Vayosoft.Core.SharedKernel.Entities;
using Vayosoft.Core.SharedKernel.Models.Pagination;
using Vayosoft.Core.Specifications;

namespace Vayosoft.Core.Persistence
{
    public interface IReadOnlyRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> FindAsync<TId>(TId id,
            CancellationToken cancellationToken = default) where TId : notnull;

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> criteria,
            CancellationToken cancellationToken = default);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> criteria,
            CancellationToken cancellationToken = default);

        Task<IPagedEnumerable<TEntity>> ListAsync(ISpecification<TEntity, object> spec,
            CancellationToken cancellationToken = default);
    }
}
