﻿using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Vayosoft.Caching;
using Vayosoft.Core;
using Vayosoft.Core.Queries;
using Vayosoft.Data.Redis;
using Vayosoft.Streaming.Redis;
using Warehouse.API.Services;
using Warehouse.API.Services.Localization;
using Warehouse.API.TagHelpers;
using Warehouse.API.UseCases.Resources;
using Warehouse.Core.Entities.Models;
using Warehouse.Core.Persistence;
using Warehouse.Core.Services;
using Warehouse.Core.Services.Authentication;
using Warehouse.Core.UseCases.Administration;
using Warehouse.Core.UseCases.BeaconTracking;
using Warehouse.Core.UseCases.Management;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Authentication;
using Warehouse.Infrastructure.Persistence;

namespace Warehouse.API
{
    public static class Configuration
    {
        public static IServiceCollection AddApiVersioningService(this IServiceCollection services)
        {

            //https://christian-schou.dk/how-to-use-api-versioning-in-net-core-web-api/
            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;//api-support-versions
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    //new QueryStringApiVersionReader("x-api-version"),
                    //new MediaTypeApiVersionReader("x-api-version"), //accept
                    new HeaderApiVersionReader("x-api-version"));
            });
            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });
            return services;
        }

        public static IServiceCollection AddWarehouseApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddCoreServices()
                .AddRedisConnection()
                .AddRedisProducer()
                .AddCaching(configuration);
            //builder.Services.AddRedisCache(configuration);

            services.AddHttpContextAccessor()
                .AddScoped<IUserContext, UserContext>();

            services.AddInfrastructure(configuration);

            services
                .AddAppAdministrationServices()
                .AddAppTrackingServices()
                .AddAppManagementServices();

            return services;
        }

        public static IServiceCollection AddUserService(this IServiceCollection services, IConfiguration configuration)
        {
            // configure strongly typed settings object
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            // configure DI for application services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordHasher, MD5PasswordHasher>();
            services.AddScoped<IUserStore<UserEntity>, UserStore>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Over18",
            //        policy => policy.Requirements.Add(new Over18Requirement()));
            //});

            return services;
        }

        public static IServiceCollection AddLocalizationService(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<SharedLocalizationService>();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("he"),
                };

                options.DefaultRequestCulture = new RequestCulture("he");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            services.AddQueryHandler<GetResources, IEnumerable<ResourceGroup>, GetResources.ResourcesQueryHandler>();

            return services;
        }
    }
}
