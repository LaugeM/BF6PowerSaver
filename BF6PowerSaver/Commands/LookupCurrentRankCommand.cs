using BF6PowerSaver.Services;
using BF6PowerSaver.Stores;
using BF6PowerSaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BF6PowerSaver.Commands
{
    public class LookupCurrentRankCommand : CommandBase
    {
        private readonly RankCheckViewModel rankCheckViewModel;
        private readonly SearchStore searchStore;

        public LookupCurrentRankCommand(RankCheckViewModel rankCheckViewModel, SearchStore searchStore)
        {
            this.rankCheckViewModel = rankCheckViewModel;
            this.searchStore = searchStore;
        }

        public override void Execute(object parameter)
        {
            rankCheckViewModel.LookupCurrentRank();
        }
    }
}
