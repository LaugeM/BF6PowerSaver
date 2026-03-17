using BF6PowerSaver.Commands;
using BF6PowerSaver.Services;
using BF6PowerSaver.Services.Network;
using BF6PowerSaver.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BF6PowerSaver.ViewModels
{
    public class RankCheckViewModel : BaseViewModel
    {
        HttpRequestService httpRequestService;
        private readonly SearchStore searchStore;
        readonly RankMonitorService rankMonitorService;
        readonly IShutdownService shutdownService;

        public string Username => searchStore.CurrentResult?.Username;
        public int PersonalId => searchStore.CurrentResult.PersonalId;

        private int startRank;
        public int StartRank
        {
            get { return startRank; }
            set
            {
                startRank = value;
                OnPropertyChanged(nameof(startRank));
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        private int currentRank;
        public int CurrentRank
        {
            get { return currentRank; }
            set
            {
                currentRank = value;
                OnPropertyChanged(nameof(currentRank));
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        private int rankGoal;
        public int RankGoal
        {
            get { return rankGoal; }
            set
            {
                rankGoal = value;
                OnPropertyChanged(nameof(rankGoal));
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool running;
        public bool Running
        {
            get { return running; }
            set
            {
                running = value;
                OnPropertyChanged(nameof(running));
            }
        }

        private bool autoShutdownEnabled = true;
        public bool AutoShutdownEnabled
        {
            get { return autoShutdownEnabled; }
            set
            {
                autoShutdownEnabled = value;
                OnPropertyChanged(nameof(autoShutdownEnabled));
            }
        }

        public ICommand NavigateToHomeViewCommand { get; }
        public ICommand LookupCurrentRankCommand { get; }
        public ICommand StartRankCheckerCommand { get; }

        // Accept an optional shutdownService so callers can inject a test double.
        public RankCheckViewModel(SearchStore searchStore, NavigationStore navigationStore, IShutdownService? shutdownService = null)
        {
            this.httpRequestService = new();
            this.searchStore = searchStore;
            this.shutdownService = shutdownService ?? new ShutdownService();

            NavigationService<HomeViewModel> navigationServiceHomeView = new NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(searchStore, navigationStore));
            NavigateToHomeViewCommand = new NavigateCommand<HomeViewModel>(navigationServiceHomeView);
            LookupCurrentRankCommand = new LookupCurrentRankCommand(this, searchStore);
            StartRankCheckerCommand = new StartRankCheckerCommand(this, searchStore);

            rankMonitorService = new RankMonitorService(httpRequestService);
            rankMonitorService.RankUpdated += OnRankUpdated;
            rankMonitorService.RankStalled += HandleRankStalled;
        }

        // Start checker loop
        public void StartAutoRefresh()
        {
            // Set StartRank before checking
            SetStartRank();
            // Begin checking every minute
            if (!rankMonitorService.IsRunning)
                rankMonitorService.Start(PersonalId, TimeSpan.FromSeconds(60));
        }

        // Stop checker loop
        public async void StopAutoRefresh()
        {
            await rankMonitorService.StopAsync();
        }

        void OnRankUpdated(int newRank)
        {
            Application.Current.Dispatcher.Invoke(() => CurrentRank = newRank);
        }

        // Set StartRank
        public async void SetStartRank()
        {
            StartRank = await httpRequestService.GetRankFromId(PersonalId);
        }

        // Lookup current rank once
        public async void LookupCurrentRank()
        {
            CurrentRank = await httpRequestService.GetRankFromId(PersonalId);
        }

        private async void HandleRankStalled(int stalledRank)
        {
            bool shouldShutdown = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (System.Diagnostics.Debugger.IsAttached) return;
                if (!AutoShutdownEnabled) return;
                shouldShutdown = true;
            });

            if (!shouldShutdown) return;

            // perform shutdown request and await it
            await shutdownService.RequestShutdownAsync(30).ConfigureAwait(false);
        }
    }
}
