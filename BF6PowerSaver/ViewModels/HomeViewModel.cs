using BF6PowerSaver.Commands;
using BF6PowerSaver.Stores;
using BF6PowerSaver.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using BF6PowerSaver.Models;
using BF6PowerSaver.Services.Network;

namespace BF6PowerSaver.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        HttpRequestService httpRequestService;

        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }

        private int personalId;
        public int PersonalId
        {
            get { return personalId; }
            set
            {
                personalId = value;
                OnPropertyChanged(nameof(PersonalId));
                System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand NavigateToRankCheckCommand { get; }
        public ICommand LookupUsernameCommand { get; }

        public HomeViewModel(SearchStore searchStore, NavigationStore navigationStore)
        {
            httpRequestService = new();

            NavigationService<RankCheckViewModel> navigationService = new NavigationService<RankCheckViewModel>(
                navigationStore,
                () => new RankCheckViewModel(searchStore, navigationStore));

            NavigateToRankCheckCommand = new NavigateToRankCheckCommand(this, searchStore, navigationService);
            LookupUsernameCommand = new LookupUsernameCommand(this, searchStore);
        }

        public bool CheckUsername()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }

        public bool CheckPersonalId()
        {
            return PersonalId > 0;
        }

        public async void LookupUsername()
        {
            PersonalId = await httpRequestService.GetEaIdFromUsername(Username);
        }

    }
}
