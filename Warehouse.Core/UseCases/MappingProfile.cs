﻿using AutoMapper;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.UseCases.IPS.Models;
using Warehouse.Core.UseCases.Products.Models;
using Warehouse.Core.UseCases.Warehouse.Models;

namespace Warehouse.Core.UseCases
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, ProductEntity>();

            CreateMap<GatewayDto, Gateway>();
            CreateMap<BeaconDto, Beacon>();
            CreateMap<WarehouseSiteDto, WarehouseSiteEntity>()
                .ForMember(m => m.Gateways,
                    des =>
                        des.MapFrom(m => m.Gateways ?? new List<GatewayDto>()));

            CreateMap<ProductEntity, ProductDto>()
                .ForMember(p => p.Metadata, des => des.MapFrom(m => m.Metadata));

            CreateMap<Gateway, GatewayDto>();
            CreateMap<Beacon, BeaconDto>();
            CreateMap<WarehouseSiteEntity, WarehouseSiteDto>();

            CreateMap<IndoorPositionStatusEntity, IndoorPositionStatusDto>()
                .ForMember(m => m.SiteId, des => des.MapFrom(m => m.Id))
                ;

            CreateMap<BeaconEntity, ProductItemDto>();
            CreateMap<ProductItemDto, BeaconEntity>()
                .ForMember(p => p.Id, des => des.MapFrom(m => m.MacAddress))
                .ForMember(p => p.ProductId, des => des.MapFrom(m => m.Product));
        }
    }
}