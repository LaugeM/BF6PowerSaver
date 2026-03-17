using System;
using System.Threading.Tasks;

namespace BF6PowerSaver.Services
{
    public interface IRankMonitor : IAsyncDisposable
    {
        event Action<int>? RankUpdated;
        event Action<int>? RankChanged;
        event Action<int>? RankUnchanged;
        event Action<int>? RankStalled;

        bool IsRunning { get; }
        void Start(int personalId, TimeSpan pollInterval, int unchangedThreshold = 10);
        Task StopAsync();
        void ResetBaseline();
    }
}