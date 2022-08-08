﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Vayosoft.Core.SharedKernel.Models.Pagination;
using Warehouse.Core.UseCases.Management.Models;
using Warehouse.Core.UseCases.Management.Queries;
using Vayosoft.Core.Queries;
using Vayosoft.Core.Commands;
using Vayosoft.Core.Persistence.Queries;
using Vayosoft.Data.MongoDB.QueryHandlers;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.UseCases.Management.Commands;

namespace Warehouse.Core.UseCases.Management
{ 
    internal static class Configuration
    {
        public static IServiceCollection AddWarehouseManagementServices(this IServiceCollection services) =>
            services
                .AddQueryHandlers()
                .AddCommandHandlers();

        private static IServiceCollection AddQueryHandlers(this IServiceCollection services) =>
            services
                .AddQueryHandler<GetProductMetadata, ProductMetadata, HandleGetProductMetadata>()
                .AddQueryHandler<GetProductItemMetadata, ProductMetadata, HandlerGetProductItemMetadata>()
                .AddQueryHandler<GetRegisteredBeaconList, IEnumerable<string>, HandleGetRegisteredBeaconList>()
                .AddQueryHandler<GetRegisteredGwList, IEnumerable<string>, GetRegisteredGwList.RegisteredGwQueryHandler>()
                .AddQueryHandler<GetBeacons, IPagedEnumerable<ProductItemDto>, HandleGetProductItems>()
                .AddQueryHandler<GetProducts, IPagedEnumerable<ProductEntity>, HandleGetProducts>()
                .AddQueryHandler<GetSites, IPagedEnumerable<WarehouseSiteEntity>, HandleGetSites>()
                .AddQueryHandler<GetAlerts, IPagedEnumerable<AlertEntity>, HandleGetAlerts>()
                .AddQueryHandler<GetBeacons, IPagedEnumerable<ProductItemDto>, HandleGetProductItems>()
                //.AddQueryHandler<SpecificationQuery<WarehouseProductSpec, IPagedEnumerable<BeaconRegisteredEntity>>, IPagedEnumerable<BeaconRegisteredEntity>,
                //    MongoPagingQueryHandler<WarehouseProductSpec, BeaconRegisteredEntity>>()
                //.AddQueryHandler<SpecificationQuery<ProductSpec, IPagedEnumerable<ProductEntity>>, IPagedEnumerable<ProductEntity>,
                //    MongoPagingQueryHandler<ProductSpec, ProductEntity>>()
                //.AddQueryHandler<SpecificationQuery<WarehouseAlertSpec, IPagedEnumerable<AlertEntity>>, IPagedEnumerable<AlertEntity>,
                //    MongoPagingQueryHandler<WarehouseAlertSpec, AlertEntity>>()
                //.AddQueryHandler<SpecificationQuery<WarehouseSiteSpec, IPagedEnumerable<WarehouseSiteEntity>>, IPagedEnumerable<WarehouseSiteEntity>,
                //    MongoPagingQueryHandler<WarehouseSiteSpec, WarehouseSiteEntity>>()
                .AddQueryHandler<SingleQuery<ProductEntity>, ProductEntity, MongoSingleQueryHandler<string, ProductEntity>>();

        
        private static IServiceCollection AddCommandHandlers(this IServiceCollection services) =>
            services
                .AddCommandHandler<SetWarehouseSite, HandleSetWarehouseSite>()
                .AddCommandHandler<DeleteWarehouseSite, HandleDeleteWarehouseSite>()
                .AddCommandHandler<SetGatewayToSite, HandleSetGatewayToSite>()
                .AddCommandHandler<RemoveGatewayFromSite, HandleRemoveGatewayFromSite>()
                .AddCommandHandler<SetBeacon, HandleSetBeacon>()
                .AddCommandHandler<SetAlert, HandleSetAlert>()
                .AddCommandHandler<DeleteAlert, HandleDeleteAlert>()
                .AddCommandHandler<DeleteBeacon, HandleDeleteBeacon>()
                .AddCommandHandler<SetProduct, HandleSetProduct>()
                .AddCommandHandler<DeleteProduct, HandleDeleteProduct>();
    }
}