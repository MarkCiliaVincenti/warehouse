﻿using Vayosoft.Core.SharedKernel.Events.External;

namespace Vayosoft.Testing;

public class DummyExternalEventConsumer : IExternalEventConsumer
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
