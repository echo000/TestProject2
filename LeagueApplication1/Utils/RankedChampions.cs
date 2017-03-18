using RiotSharp.GameEndpoint;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class RankedChampions
	{
		public string KDA { get; set; }
		public UriImageSource Icon { get; set; }
		public string CreepScore { get; set; }
		public string WinRate { get; set; }
		public double WinRateDecimal { get; set; }
		public int TotalSessionsPlayed { get; set; }


		public RankedChampions(string kda, string winrate, double winratedecimal, string creepScore, UriImageSource imagePath, int totalsessionsplayed)
		{
			KDA = kda;
			WinRate = winrate;
			WinRateDecimal = winratedecimal;
			CreepScore = creepScore;
			Icon = imagePath;
			TotalSessionsPlayed = totalsessionsplayed;
		}
	}
}

