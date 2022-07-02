﻿using MongoDB.Driver;
using Vayosoft.Core.Caching;
using Vayosoft.Core.Queries.Handler;
using Vayosoft.Core.Queries.Query;
using Vayosoft.Core.Utilities;
using Vayosoft.Data.MongoDB;
using Warehouse.Core.Entities.Models;

namespace Warehouse.Core.UseCases.Warehouse.Queries
{
    public class GetRegisteredBeaconList : IQuery<IEnumerable<string>>
    {
        public class RegisteredBeaconQueryHandler : IQueryHandler<GetRegisteredBeaconList, IEnumerable<string>>
        {
            private readonly IDistributedMemoryCache _cache;
            private readonly IMongoCollection<BeaconRegisteredEntity> _collection;

            public RegisteredBeaconQueryHandler(IMongoContext context, IDistributedMemoryCache cache)
            {
                _collection = context.Database.GetCollection<BeaconRegisteredEntity>(CollectionName.For<BeaconRegisteredEntity>());
                _cache = cache;
            }

            public async Task<IEnumerable<string>> Handle(GetRegisteredBeaconList request, CancellationToken cancellationToken)
            {
                var data = await _cache.GetOrCreateExclusiveAsync(CacheKey.With<BeaconRegisteredEntity>(), async options =>
                {
                    options.AbsoluteExpirationRelativeToNow = TimeSpans.FiveMinutes;

                    var data = await _collection
                        .FindAsync(Builders<BeaconRegisteredEntity>.Filter.Empty, cancellationToken: cancellationToken);
                    return (await data.ToListAsync(cancellationToken: cancellationToken)).Select(b => b.MacAddress);
                });

                return data;
            }
        }
    }
}
