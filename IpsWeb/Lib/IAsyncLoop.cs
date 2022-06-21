﻿namespace IpsWeb.Lib
{
    public interface IAsyncLoop : IDisposable
    {
        string Name { get; }

        TimeSpan RepeatEvery { get; set; }

        IAsyncLoop Run(TimeSpan? repeatEvery = null, TimeSpan? startAfter = null);

        IAsyncLoop Run(CancellationToken cancellation, TimeSpan? repeatEvery = null, TimeSpan? startAfter = null);

        Task RunningTask { get; }
    }
}
