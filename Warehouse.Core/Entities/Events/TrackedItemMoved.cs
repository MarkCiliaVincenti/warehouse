﻿using Vayosoft.Core.SharedKernel.Events.External;
using Vayosoft.Core.SharedKernel.ValueObjects;

namespace Warehouse.Core.Entities.Events
{
    public record TrackedItemMoved(
        MacAddress Id,
        DateTime Timestamp,
        string SourceId,
        string DestinationId
        ) : IExternalEvent
    {
        public static TrackedItemMoved Create(MacAddress id, DateTime timestamp, string sourceId, string destinationId)
        {
            if(timestamp == default)
                timestamp = DateTime.UtcNow;
            if(string.IsNullOrEmpty(sourceId))
                throw new ArgumentNullException(nameof(sourceId));
            if (string.IsNullOrEmpty(destinationId))
                throw new ArgumentNullException(nameof(destinationId));

            return new TrackedItemMoved(id, timestamp, sourceId, destinationId);
        }
    }
}
