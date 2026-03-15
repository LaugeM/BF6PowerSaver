using BF6PowerSaver.Commands;
using BF6PowerSaver.Services;
using BF6PowerSaver.Services.Network;
using BF6PowerSaver.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BF6PowerSaver.ViewModels
{
    public class RankCheckViewModel : BaseViewModel
    {
        HttpRequestService httpRequestService;
        private readonly SearchStore searchStore;

        public string Username => searchStore.CurrentResult?.Username;
        public int PersonalId => searchStore.CurrentResult.PersonalId;

        private int rank;
        public int Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged(nameof(rank));
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

        public ICommand NavigateToHomeViewCommand { get; }
        public ICommand LookupCurrentRankCommand { get; }

        public RankCheckViewModel(SearchStore searchStore, NavigationStore navigationStore)
        {
            httpRequestService = new();
            this.searchStore = searchStore;

            NavigationService<HomeViewModel> navigationServiceHomeView = new NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(searchStore, navigationStore));
            NavigateToHomeViewCommand = new NavigateCommand<HomeViewModel>(navigationServiceHomeView);
            LookupCurrentRankCommand = new LookupCurrentRankCommand(this, searchStore);
        }

        public async void LookupCurrentRank()
        {
            Rank = await httpRequestService.GetRankFromId(PersonalId);
        }
    }
}
