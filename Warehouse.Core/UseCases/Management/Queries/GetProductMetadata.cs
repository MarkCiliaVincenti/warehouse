﻿using Vayosoft.Core.Caching;
using Vayosoft.Core.Persistence;
using Vayosoft.Core.Queries;
using Vayosoft.Core.Utilities;
using Warehouse.Core.Entities.Models;

namespace Warehouse.Core.UseCases.Management.Queries
{
    public class GetProductMetadata : IQuery<ProductMetadata>
    { }

    internal class HandleGetProductMetadata : IQueryHandler<GetProductMetadata, ProductMetadata>
    {
        private readonly IDistributedMemoryCache _cache;
        private readonly IRepository<FileEntity> _fileRepository;

        public HandleGetProductMetadata(IRepository<FileEntity> fileRepository, IDistributedMemoryCache cache)
        {
            _fileRepository = fileRepository;
            _cache = cache;
        }

        public async Task<ProductMetadata> Handle(GetProductMetadata request, CancellationToken cancellationToken)
        {
            var data = await _cache.GetOrCreateExclusiveAsync(CacheKey.With<ProductMetadata>(), async options =>
            {
                options.SlidingExpiration = TimeSpans.FiveMinutes;
                var entity = await _fileRepository.FindAsync("product_metadata", cancellationToken);
                ProductMetadata data = null;
                if (!string.IsNullOrEmpty(entity?.Content))
                    data = entity.Content.FromJson<ProductMetadata>();

                return data;
            });

            return data;
        }
    }
}
