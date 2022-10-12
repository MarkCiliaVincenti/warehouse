﻿using Microsoft.Extensions.DependencyInjection;

namespace Vayosoft.MongoDB
{
    public static class Configuration
    {
        public static IServiceCollection AddMongoContext(this IServiceCollection services,
            Action configureOptions = null)
        {
            services.AddSingleton<IMongoConnection, MongoConnection>();
            configureOptions?.Invoke();
            return services;
        }
    }
}
