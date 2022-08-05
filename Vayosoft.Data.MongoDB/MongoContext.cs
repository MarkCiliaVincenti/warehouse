﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Vayosoft.Core.SharedKernel.Entities;

namespace Vayosoft.Data.MongoDB
{
    public delegate void OnTraceLine(string commandName, string command);
    public class MongoContext : IMongoContext
    {
        private readonly MongoClient _client;
        public IMongoDatabase Database { get; }
        public IClientSessionHandle Session { get; private set; }

        public event OnTraceLine TraceLine;

        [ActivatorUtilitiesConstructor]
        public MongoContext(IConfiguration config) : this(config.GetConnectionSetting()) { }
        public MongoContext(ConnectionSetting config) : this(config?.ConnectionString, config?.ReplicaSet?.BootstrapServers) { }
        public MongoContext(string connectionString, string[] bootstrapServers)
        {
            MongoClientSettings settings;
            string databaseName = null;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                var connectionUrl = new MongoUrl(connectionString);
                settings = MongoClientSettings.FromUrl(connectionUrl);
                databaseName = GetDatabaseName(connectionString!);
            }
            else
            {
                settings = new MongoClientSettings
                {
                    DirectConnection = false,
                    ReadPreference = ReadPreference.Primary
                };

                if (bootstrapServers != null)
                    settings.Servers = bootstrapServers.Select(MongoServerAddress.Parse);
                else
                {
                    settings.Servers = new[]
                    {
                        new MongoServerAddress("localhost", 37017),
                        new MongoServerAddress("localhost", 37018),
                        new MongoServerAddress("localhost", 37019)
                    };
                }
            }

            settings.ClusterConfigurator = cb =>
            {
                cb.Subscribe<CommandStartedEvent>(e =>
                {
                    TraceLine?.Invoke(e.CommandName, e.Command.ToJson());
                });
            };

            _client = new MongoClient(settings);
            Database = _client.GetDatabase(databaseName ?? "default");
        }

        public async Task<IClientSessionHandle> StartSession(CancellationToken cancellationToken = default)
        {
            var session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
            Session = session;
            return session;
        }

        public IMongoDatabase GetDatabase(string db)
            => _client.GetDatabase(db);

        public IMongoCollection<T> Collection<T>(CollectionName collectionName = null) where T : IEntity
            => Database.GetDocumentCollection<T>(collectionName);

        private static string GetDatabaseName(string connectionString)
        {
            var hostIndex = connectionString.IndexOf("//", StringComparison.Ordinal);
            if (hostIndex > 0)
            {
                var startIndex = connectionString.IndexOf("/", hostIndex + 2, StringComparison.Ordinal) + 1;
                var endIndex = connectionString.IndexOf("?", startIndex, StringComparison.Ordinal);
                if (startIndex > 0)
                {
                    if (endIndex > 0)
                        return connectionString.Substring(startIndex, endIndex - startIndex);
                    else
                        return connectionString[startIndex..];
                }
            }

            throw new ArgumentException("Unsupported DB connection string", nameof(connectionString));
        }
    }
}
