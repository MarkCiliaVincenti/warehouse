﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Vayosoft.Core.SharedKernel.Aggregates;
using Vayosoft.Core.SharedKernel.Entities;


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
}
