using BF6PowerSaver.Services;
using BF6PowerSaver.Stores;
using BF6PowerSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BF6PowerSaver.Commands
{
    public class LookupUsernameCommand : CommandBase
    {
        private readonly HomeViewModel homeViewModel;
        private readonly SearchStore searchStore;

        public LookupUsernameCommand(HomeViewModel homeViewModel, SearchStore searchStore)
        {
            this.homeViewModel = homeViewModel;
            this.searchStore = searchStore;
        }

        public override void Execute(object parameter)
        {
            if (homeViewModel.CheckUsername())
            {
                homeViewModel.LookupUsername();
            }
        }
    }
}
