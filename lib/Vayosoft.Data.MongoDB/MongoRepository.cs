﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Vayosoft.Core.Persistence;
using Vayosoft.Core.SharedKernel.Entities;
using Vayosoft.Core.SharedKernel.Exceptions;
using Vayosoft.Core.SharedKernel.Models.Pagination;

namespace Vayosoft.Data.MongoDB
{
    public class MongoRepositoryBase<T> : IRepositoryBase<T> where T : class, IEntity
    {
        protected readonly IMongoCollection<T> Collection;

        public MongoRepositoryBase(IMongoConnection connection) =>
            Collection = connection.Collection<T>(CollectionName.For<T>());

        public IQueryable<T> AsQueryable() => Collection.AsQueryable();

        public Task<T> FindAsync<TId>(TId id, CancellationToken cancellationToken = default) =>
            Collection.Find(q => q.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);

        public Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
            Collection.InsertOneAsync(entity, cancellationToken: cancellationToken);

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default) =>
            Collection.ReplaceOneAsync(Builders<T>.Filter.Eq(e => e.Id, entity.Id), entity, cancellationToken: cancellationToken);

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default) =>
            Collection.DeleteOneAsync(Builders<T>.Filter.Eq(e => e.Id, entity.Id), cancellationToken: cancellationToken);

        public Task<T> GetAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
        {
            return FindAsync(id, cancellationToken) ?? throw EntityNotFoundException.For<T>(id);
        }

        public Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default) =>
            Collection.Find(criteria).FirstOrDefaultAsync(cancellationToken);

        public Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default) =>
            Collection.Find(criteria).SingleOrDefaultAsync(cancellationToken);

        public Task<List<T>> ListAsync(CancellationToken cancellationToken = default) =>
            Collection.Find(Builders<T>.Filter.Empty).ToListAsync(cancellationToken);

        public Task<List<T>> ListAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default) =>
            Collection.Find(criteria).ToListAsync(cancellationToken);

        public Task<IPagedEnumerable<T>> PagedListAsync(IPagingModel<T, object> model, Expression<Func<T, bool>> criteria, CancellationToken cancellationToken) =>
            Collection.AggregateByPage(model, Builders<T>.Filter.Where(criteria), cancellationToken);

        public Task<IPagedEnumerable<T>> PagedListAsync(IPagingModel<T, object> model, CancellationToken cancellationToken) =>
            Collection.AggregateByPage(model, Builders<T>.Filter.Empty, cancellationToken);
    }
}
