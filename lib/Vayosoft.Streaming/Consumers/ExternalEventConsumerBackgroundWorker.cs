using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vayosoft.Core.SharedKernel.Events.External;

namespace Vayosoft.Streaming.Consumers
{
    //See more: https://www.stevejgordon.co.uk/asp-net-core-2-ihostedservice
    public sealed class ExternalEventConsumerBackgroundWorker: IHostedService
    {
        private Task executingTask;
        private CancellationTokenSource cts;
        private readonly IExternalEventConsumer externalEventConsumer;
        private readonly ILogger<ExternalEventConsumerBackgroundWorker> logger;

        public ExternalEventConsumerBackgroundWorker(
            IExternalEventConsumer externalEventConsumer,
            ILogger<ExternalEventConsumerBackgroundWorker> logger
        )
        {
            this.externalEventConsumer = externalEventConsumer;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("External Event Consumer started");

            // Create a linked token so we can trigger cancellation outside of this token's cancellation
            cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // Store the issue we're executing
            executingTask = externalEventConsumer.StartAsync(cancellationToken);

            // If the issue is completed then return it, otherwise it's running
            return executingTask is {IsCompleted: true} ? executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (executingTask == null)
            {
                return;
            }

            // Signal cancellation to the executing method
            cts?.Cancel();

            // Wait until the issue completes or the stop token triggers
            await Task.WhenAny(executingTask, Task.Delay(-1, cancellationToken));

            // Throw if cancellation triggered
            cancellationToken.ThrowIfCancellationRequested();

            logger.LogInformation("External Event Consumer stopped");
        }
    }
}
