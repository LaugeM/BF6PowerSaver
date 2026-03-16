using BF6PowerSaver.Stores;
using BF6PowerSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BF6PowerSaver.Commands
{
    public class StartRankCheckerCommand : CommandBase
    {
        private readonly RankCheckViewModel rankCheckViewModel;
        private readonly SearchStore searchStore;

        public StartRankCheckerCommand(RankCheckViewModel rankCheckViewModel, SearchStore searchStore)
        {
            this.rankCheckViewModel = rankCheckViewModel;
            this.searchStore = searchStore;
        }

        public override void Execute(object parameter)
        {
            if (rankCheckViewModel.Running)
            {
                rankCheckViewModel.StartAutoRefresh();
            }
            if (!rankCheckViewModel.Running)
            {
                rankCheckViewModel.StopAutoRefresh();
            }
        }
    }
}
