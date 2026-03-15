using BF6PowerSaver.Stores;
using BF6PowerSaver.ViewModels;
using BF6PowerSaver.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BF6PowerSaver.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly NavigationService<TViewModel> navigationService;

        public NavigateCommand(NavigationService<TViewModel> navigationService)
        {
            this.navigationService = navigationService;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            navigationService.Navigate();
        }
    }
}
