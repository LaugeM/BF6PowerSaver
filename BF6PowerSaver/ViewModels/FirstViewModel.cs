using BF6PowerSaver.Commands;
using BF6PowerSaver.Stores;
using BF6PowerSaver.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BF6PowerSaver.ViewModels
{
    public class FirstViewModel : BaseViewModel
    {
        
        public ICommand NavigateToHomeViewCommand { get; }

        public FirstViewModel(NavigationStore navigationStore, string personalId)
        {
            NavigationService navigationServiceHomeView = new NavigationService(navigationStore, () => new HomeViewModel(navigationStore));
            NavigateToHomeViewCommand = new NavigateCommand(navigationServiceHomeView);
        }
    }
}
