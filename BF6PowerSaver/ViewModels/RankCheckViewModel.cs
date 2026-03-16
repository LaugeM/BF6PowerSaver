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
        readonly RankRefreshService rankRefreshService;

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

        public ICommand NavigateToHomeViewCommand { get; }
        public ICommand LookupCurrentRankCommand { get; }
        public ICommand StartRankCheckerCommand { get; }

        public RankCheckViewModel(SearchStore searchStore, NavigationStore navigationStore)
        {
            httpRequestService = new();
            this.searchStore = searchStore;

            NavigationService<HomeViewModel> navigationServiceHomeView = new NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(searchStore, navigationStore));
            NavigateToHomeViewCommand = new NavigateCommand<HomeViewModel>(navigationServiceHomeView);
            LookupCurrentRankCommand = new LookupCurrentRankCommand(this, searchStore);
            StartRankCheckerCommand = new StartRankCheckerCommand(this, searchStore);

            rankRefreshService = new RankRefreshService(httpRequestService);
            rankRefreshService.RankUpdated += OnRankUpdated;
        }

        // Start checker loop
        public void StartAutoRefresh()
        {
            // Set StartRank before checking
            SetStartRank();
            // Begin checking every minute
            if (!rankRefreshService.IsRunning)
                rankRefreshService.Start(PersonalId, TimeSpan.FromSeconds(60));
        }

        // Stop checker loop
        public async void StopAutoRefresh()
        {
            await rankRefreshService.StopAsync();
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
    }
}
