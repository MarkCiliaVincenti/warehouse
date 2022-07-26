﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Vayosoft.Core.SharedKernel;
using Vayosoft.Core.SharedKernel.Entities;
using Vayosoft.Core.SharedKernel.Exceptions;

namespace Vayosoft.Data.MongoDB
{
    public class MongoContextBase : MongoContext
    {
        public MongoContextBase(IConfiguration config) : base(config) { }
        public MongoContextBase(ConnectionSetting config) : base(config) { }
        public MongoContextBase(string connectionString, string[] bootstrapServers)
            : base(connectionString, bootstrapServers) { }

        public IQueryable<T> AsQueryable<T>() where T : class, IEntity =>
            Collection<T>().AsQueryable();

        public async Task<T> GetAsync<T>(string id, CancellationToken cancellationToken = default) where T : IEntity =>
            await GetAsync<T, string>(id, cancellationToken);

        public async Task<T> GetAsync<T, TId>(TId id, CancellationToken cancellationToken = default) where T : IEntity =>
            await FindAsync<T>(id, cancellationToken) ?? throw EntityNotFoundException.For<T>(id);

        public async Task GetAndUpdateAsync<T>(string id, Action<T> action, CancellationToken cancellationToken = default)
            where T : class, IEntity
        {
            var entity = await GetAsync<T>(id, cancellationToken);
            action(entity);
            await UpdateAsync(entity, cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(criteria).FirstOrDefaultAsync(cancellationToken);

        public Task<T> SingleOrDefaultAsync<T>(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(criteria).SingleOrDefaultAsync(cancellationToken);

        public Task<List<T>> ListAsync<T>(CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(Builders<T>.Filter.Empty).ToListAsync(cancellationToken);

        public Task<List<T>> ListAsync<T>(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(criteria).ToListAsync(cancellationToken);

        public Task<TResult> FirstOrDefaultAsync<T, TResult>(Expression<Func<T, bool>> criteria, IMapper mapper, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(criteria).Project(x => mapper.Map<TResult>(x)).FirstOrDefaultAsync(cancellationToken);

        public Task<TResult> SingleOrDefaultAsync<T, TResult>(Expression<Func<T, bool>> criteria, IMapper mapper, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(criteria).Project(x => mapper.Map<TResult>(x)).SingleOrDefaultAsync(cancellationToken);

        public Task<List<TResult>> ListAsync<T, TResult>(Expression<Func<T, bool>> criteria, IMapper mapper, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(criteria).Project(x => mapper.Map<TResult>(x)).ToListAsync(cancellationToken);

        public Task<T> FindAsync<T>(object id, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().Find(q => q.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);

        public Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().InsertOneAsync(entity, cancellationToken: cancellationToken);

        public Task UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, cancellationToken: cancellationToken);
        
        public Task DeleteAsync<T>(T entity, CancellationToken cancellationToken = default) where T : IEntity =>
            Collection<T>().DeleteOneAsync(e => e.Id.Equals(entity.Id), cancellationToken: cancellationToken);

        public Task DeleteAsync<T>(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken) where T : IEntity =>
            Collection<T>().DeleteOneAsync(criteria, cancellationToken: cancellationToken);

    }
}
