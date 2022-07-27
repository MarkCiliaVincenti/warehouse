﻿using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Vayosoft.Data.MongoDB;
using Warehouse.Core.Entities.Models;

namespace Warehouse.Core.Persistence.Mapping
{
    public class BeaconRegisteredEntityClassMap : MongoClassMap<BeaconRegisteredEntity>
    {
        public override void Map(BsonClassMap<BeaconRegisteredEntity> cm)
        {
            cm.AutoMap();
            cm.MapIdProperty(c => c.MacAddress)
                .SetIdGenerator(StringObjectIdGenerator.Instance);
        }
    }
    public class BeaconReceivedEntityClassMap : MongoClassMap<BeaconRegisteredEntity>
    {
        public override void Map(BsonClassMap<BeaconRegisteredEntity> cm)
        {
            cm.AutoMap();
            cm.MapIdProperty(c => c.MacAddress)
                .SetIdGenerator(StringObjectIdGenerator.Instance);
        }
    }

    public class BeaconEntityClassMap : MongoClassMap<BeaconEntity>
    {
        public override void Map(BsonClassMap<BeaconEntity> cm)
        {
            cm.AutoMap();
            cm.MapIdProperty(c => c.MacAddress)
                .SetIdGenerator(StringObjectIdGenerator.Instance);
        }
    }
}
