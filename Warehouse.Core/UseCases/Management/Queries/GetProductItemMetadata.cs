﻿using Vayosoft.Core.Caching;
using Vayosoft.Core.Persistence;
using Vayosoft.Core.Queries;
using Vayosoft.Core.Utilities;
using Warehouse.Core.Entities.Models;

namespace Warehouse.Core.UseCases.Management.Queries
{
    public class GetProductItemMetadata : IQuery<ProductMetadata>
    { }

    public class HandlerGetProductItemMetadata : IQueryHandler<GetProductItemMetadata, ProductMetadata>
    {
        private readonly IDistributedMemoryCache _cache;
        private readonly IRepositoryBase<FileEntity> _fileRepository;

        public HandlerGetProductItemMetadata(IRepositoryBase<FileEntity> fileRepository, IDistributedMemoryCache cache)
        {
            _fileRepository = fileRepository;
            _cache = cache;
        }

        public async Task<ProductMetadata> Handle(GetProductItemMetadata request, CancellationToken cancellationToken)
        {
            var data = await _cache.GetOrCreateExclusiveAsync(CacheKey.With<ProductMetadata>("beacon"), async options =>
            {
                options.SlidingExpiration = TimeSpans.FiveMinutes;
                var entity = await _fileRepository.FindAsync("beacon_metadata", cancellationToken);
                ProductMetadata data = null;
                if (!string.IsNullOrEmpty(entity?.Content))
                    data = entity.Content.FromJson<ProductMetadata>();

                return data;
            });

            return data;
        }
    }
}
