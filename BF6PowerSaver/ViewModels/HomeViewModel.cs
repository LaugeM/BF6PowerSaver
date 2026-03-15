using BF6PowerSaver.Commands;
using BF6PowerSaver.Stores;
using BF6PowerSaver.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BF6PowerSaver.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private int username;
        public int Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(nameof(Username)); }
        }

        private int personalId;

        public int PersonalId
        {
            get { return personalId; }
            set { personalId = value; OnPropertyChanged(nameof(personalId)); }
        }

        public ICommand NavigateToFirstViewCommand { get; }
        public ICommand NavigateToSecondViewCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            //NavigationService navigationServiceFirstView = new NavigationService(navigationStore, () => new FirstViewModel(navigationStore));
            
            //NavigateToFirstViewCommand = new NavigateCommand(navigationServiceFirstView);
        }
    }
}
