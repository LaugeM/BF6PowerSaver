using System;
using System.Collections.Generic;
using System.Text;

namespace BF6PowerSaver.Models
{
    public class SearchResult
    {
		private string username;
		public string Username
		{
			get { return username; }
			set { username = value; }
		}

        private int personalId;
        public int PersonalId
        {
            get { return personalId; }
            set { personalId = value; }
        }

        private int rank;
        public int Rank
        {
            get { return rank; }
            set { rank = value;}
        }

        public SearchResult(string username, int personalId)
        {
            Username = username;
            PersonalId = personalId;
        }

    }
}
