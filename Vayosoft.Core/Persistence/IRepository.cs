﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Vayosoft.Core.SharedKernel.Aggregates;
using Vayosoft.Core.SharedKernel.Entities;
using Vayosoft.Core.SharedKernel.Models.Pagination;

namespace Vayosoft.Core.Persistence
{
    public interface IRepositoryBase<TEntity, in TKey> where TEntity : class, IEntity
    {
        Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken);

        Task AddAsync(TEntity entity, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken);
    }

    public interface IRepository<T> : IRepository<T, Guid> where T : class, IAggregate
    { }

    public interface IRepository<T, in TKey> : IRepositoryBase<T, TKey> where T : class, IAggregate
    { }

    public interface IQueryableRepository<TEntity, in TKey> : IRepositoryBase<TEntity, TKey> where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetQueryable();
    }

    public interface IPageableRepository<TEntity, in TKey> : IRepositoryBase<TEntity, TKey> where TEntity : class, IEntity
    {
        Task<IPagedReadOnlyCollection<TEntity>> GetByPageAsync(IPaging<TEntity, object> query, CancellationToken cancellationToken);
    }

    public interface IEntityRepository<TEntity, TKey> : IPageableRepository<TEntity, TKey>,
        IQueryableRepository<TEntity, TKey> where TEntity : class, IEntity
    { }
}
