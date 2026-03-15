using BF6PowerSaver.Views;
using BF6PowerSaver.ViewModels;
using BF6PowerSaver.Services;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using BF6PowerSaver.Models;
using BF6PowerSaver.Stores;

namespace BF6PowerSaver.Commands
{
    public class NavigateToRankCheckCommand : CommandBase
    {
        private readonly HomeViewModel homeViewModel;
        private readonly SearchStore searchStore;
        private readonly NavigationService<RankCheckViewModel> navigationService;

        public NavigateToRankCheckCommand(HomeViewModel homeViewModel, SearchStore searchStore, NavigationService<RankCheckViewModel> navigationService)
        {
            this.homeViewModel = homeViewModel;
            this.searchStore = searchStore;
            this.navigationService = navigationService;
        }

        public override bool CanExecute(object? parameter)
        {
            bool result = false;
            if (homeViewModel.CheckPersonalId())
            {
                result = true;
            }
            return result;
        }

        public override void Execute(object parameter)
        {
            SearchResult searchResult = new SearchResult(homeViewModel.Username, homeViewModel.PersonalId);

            searchStore.CurrentResult = searchResult;
            
            navigationService.Navigate();
        }
    }
}
