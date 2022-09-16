﻿using Vayosoft.Core.Persistence;
using Vayosoft.Core.Queries;
using Vayosoft.Core.SharedKernel.Models.Pagination;
using Vayosoft.Core.Specifications;
using Vayosoft.Core.Utilities;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Services;
using Warehouse.Core.Services.Security;
using Warehouse.Core.UseCases.BeaconTracking.Models;

namespace Warehouse.Core.UseCases.BeaconTracking.Queries
{
    public class GetBeaconEvents : PagingModelBase, IQuery<IPagedEnumerable<BeaconEventDto>>, ILinqSpecification<BeaconEventEntity>
    {
        public string SearchTerm { get; init; }
        public long ProviderId { get; set; }

        public IQueryable<BeaconEventEntity> Apply(IQueryable<BeaconEventEntity> query)
        {
            return query.Where(e => e.ProviderId == ProviderId)
                .WhereIf(!string.IsNullOrEmpty(SearchTerm), e => e.MacAddress.ToLower().Contains(SearchTerm.ToLower()))
                .OrderByDescending(p => p.Id)
                .Paginate(Page, Size);
        }
    }

    internal class HandleGetBeaconEvents : IQueryHandler<GetBeaconEvents, IPagedEnumerable<BeaconEventDto>>
    {
        private readonly IReadOnlyRepository<WarehouseSiteEntity> _sites;
        private readonly IReadOnlyRepository<BeaconEntity> _beacons;
        private readonly IReadOnlyRepository<BeaconEventEntity> _events;
        private readonly IUserContext _userContext;

        public HandleGetBeaconEvents(
            IReadOnlyRepository<WarehouseSiteEntity> sites,
            IReadOnlyRepository<BeaconEntity> beacons, 
            IReadOnlyRepository<BeaconEventEntity> events,
            IUserContext userContext)

        {
            _sites = sites;
            _beacons = beacons;
            _events = events;
            _userContext = userContext;
        }

        public async Task<IPagedEnumerable<BeaconEventDto>> Handle(GetBeaconEvents query, CancellationToken cancellationToken)
        {
            query.ProviderId = _userContext.User.Identity.GetProviderId();

            var data = await _events.PagedEnumerableAsync(query, cancellationToken);

            var list = new List<BeaconEventDto>();
            foreach (var e in data)
            {
                var productItem = await _beacons.FirstOrDefaultAsync(q => q.Id.Equals(e.MacAddress), cancellationToken);
                var dto = new BeaconEventDto
                {
                    Beacon = new BeaconItem
                    {
                        MacAddress = e.MacAddress,
                        Name = productItem?.Name
                    },
                    TimeStamp = e.TimeStamp,
                    Type = e.Type,
                };
                if (!string.IsNullOrEmpty(e.SourceId))
                {
                    var site = await _sites.FindAsync(e.SourceId, cancellationToken);
                    dto.Source = new SiteInfo
                    {
                        Id = e.SourceId,
                        Name = site.Name
                    };
                }
                if (!string.IsNullOrEmpty(e.DestinationId))
                {
                    var site = await _sites.FindAsync(e.DestinationId, cancellationToken);
                    dto.Destination = new SiteInfo
                    {
                        Id = e.DestinationId,
                        Name = site.Name
                    };
                }
                list.Add(dto);
            }
            return new PagedEnumerable<BeaconEventDto>(list, data.TotalCount);
        }
    }
}
