﻿using Vayosoft.Core.SharedKernel;
using Vayosoft.Core.SharedKernel.Aggregates;
using Vayosoft.Core.SharedKernel.Events;
using Vayosoft.Data.MongoDB;
using Warehouse.Core.Persistence;

namespace Warehouse.Infrastructure.Persistence
{
    public class AggregateRepository<T> : MongoRepositoryBase<T> ,IRepository<T> where T : class, IAggregate<string>
    {
        private readonly IEventBus _eventBus;

        public AggregateRepository(IMongoConnection connection, IMapper mapper, IEventBus eventBus) : base(connection, mapper)
        {
            _eventBus = eventBus;
        }
    }
}
