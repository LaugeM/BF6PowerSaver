using BF6PowerSaver.Stores;
using BF6PowerSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BF6PowerSaver.Services
{
    public class ParameterNavigationService<TParameter, TViewModel>
        where TViewModel : BaseViewModel
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<TParameter, TViewModel> viewModelFactory;

        public ParameterNavigationService(NavigationStore navigationStore, Func<TParameter, TViewModel> viewModelFactory)
        {
            this.navigationStore = navigationStore;
            this.viewModelFactory = viewModelFactory;
        }

        public void Navigate(TParameter parameter)
        {
            navigationStore.CurrentViewModel = viewModelFactory(parameter);
        }
    }
}
