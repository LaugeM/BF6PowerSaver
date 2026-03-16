using System;
using System.Threading;
using System.Threading.Tasks;
using BF6PowerSaver.Services.Network;

namespace BF6PowerSaver.Services
{
    public class RankRefreshService : IAsyncDisposable
    {
        readonly HttpRequestService httpRequestService;
        PeriodicTimer? timer;
        CancellationTokenSource? cts;
        Task? loopTask;

        public event Action<int>? RankUpdated;
        public bool IsRunning { get; private set; }

        public RankRefreshService(HttpRequestService httpRequestService)
        {
            this.httpRequestService = httpRequestService;
        }

        // Start the background loop; returns immediately.
        public void Start(int personalId, TimeSpan period)
        {
            if (IsRunning) return;

            cts = new CancellationTokenSource();
            timer = new PeriodicTimer(period);
            IsRunning = true;

            loopTask = Task.Run(async () =>
            {
                try
                {
                    while (await timer!.WaitForNextTickAsync(cts.Token))
                    {
                        try
                        {
                            var rank = await httpRequestService.GetRankFromId(personalId);
                            RankUpdated?.Invoke(rank);
                        }
                        catch (OperationCanceledException) { throw; }
                        catch (Exception)
                        {
                            // swallow/log; don't kill the loop
                        }
                    }
                }
                catch (OperationCanceledException) { }
                finally
                {
                    IsRunning = false;
                }
            }, cts.Token);
        }

        public async Task StopAsync()
        {
            if (!IsRunning) return;
            cts?.Cancel();
            if (loopTask != null) await loopTask.ConfigureAwait(false);
            timer?.Dispose();
            cts?.Dispose();
            timer = null;
            cts = null;
            loopTask = null;
            IsRunning = false;
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync();
        }
    }
}
