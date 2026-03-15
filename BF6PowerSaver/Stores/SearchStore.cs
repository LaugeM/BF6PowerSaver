using BF6PowerSaver.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BF6PowerSaver.Stores
{
    public class SearchStore
    {
		private SearchResult currentResult;

		public SearchResult CurrentResult
		{
			get { return currentResult; }
			set { currentResult = value; }
		}

	}
}
