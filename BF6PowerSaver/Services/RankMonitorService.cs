using System;
using System.Threading;
using System.Threading.Tasks;
using BF6PowerSaver.Services.Network;

namespace BF6PowerSaver.Services
{
    public class RankMonitorService : IRankMonitor
    {
        readonly HttpRequestService httpRequestService;
        CancellationTokenSource? cts;
        Task? loopTask;

        int? lastRank;
        int unchangedCount;
        readonly object sync = new();

        public event Action<int>? RankUpdated;
        public event Action<int>? RankChanged;
        public event Action<int>? RankUnchanged;
        public event Action<int>? RankStalled;

        public bool IsRunning { get; private set; }

        public RankMonitorService(HttpRequestService httpRequestService)
        {
            this.httpRequestService = httpRequestService;
        }

        public void Start(int personalId, TimeSpan pollInterval, int unchangedThreshold = 10)
        {
            if (IsRunning) return;

            cts = new CancellationTokenSource();
            var token = cts.Token;
            IsRunning = true;

            loopTask = Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        try
                        {
                            var rank = await httpRequestService.GetRankFromId(personalId).ConfigureAwait(false);
                            RankUpdated?.Invoke(rank);

                            bool isSame;
                            lock (sync)
                            {
                                isSame = lastRank.HasValue && lastRank.Value == rank;
                                if (!lastRank.HasValue || !isSame)
                                {
                                    lastRank = rank;
                                    unchangedCount = 0;
                                }
                                else
                                {
                                    unchangedCount++;
                                }
                            }

                            if (isSame)
                            {
                                RankUnchanged?.Invoke(rank);
                                if (unchangedCount >= unchangedThreshold)
                                    RankStalled?.Invoke(rank);
                            }
                            else
                            {
                                RankChanged?.Invoke(rank);
                            }
                        }
                        catch (OperationCanceledException) { break; }
                        catch
                        {
                            // swallow/log transient errors
                        }

                        try
                        {
                            await Task.Delay(pollInterval, token).ConfigureAwait(false);
                        }
                        catch (OperationCanceledException) { break; }
                    }
                }
                finally
                {
                    IsRunning = false;
                }
            }, token);
        }

        public void ResetBaseline()
        {
            lock (sync) { lastRank = null; unchangedCount = 0; }
        }

        public async Task StopAsync()
        {
            if (!IsRunning) return;
            cts?.Cancel();
            if (loopTask != null) await loopTask.ConfigureAwait(false);
            cts?.Dispose();
            cts = null;
            loopTask = null;
            IsRunning = false;
        }

        public async ValueTask DisposeAsync()
        {
            await StopAsync().ConfigureAwait(false);
        }
    }
}