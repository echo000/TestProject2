using System;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class CurrentGamePlayers
	{
		public string Name { get; set; }
		public UriImageSource Icon { get; set; }
		public string Rank { get; set; }
		public int ID { get; set; }
		public RiotSharp.Region Region { get; set; }

		public CurrentGamePlayers(string name, string rank, int id, UriImageSource icon, RiotSharp.Region region)
		{
			this.Name = name;
			this.Rank = rank;
			this.ID = id;
			this.Icon = icon;
			this.Region = region;
		}
	}
}

