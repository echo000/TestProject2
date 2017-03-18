using Xamarin.Forms;
using RiotSharp.SummonerEndpoint;
using RiotSharp;
using System.Linq;

namespace LeagueApplication1
{
	public class TabbedPages : TabbedPage
	{
		public TabbedPages(Summoner summoner, Region region)
		{
			Children.Add(new ProfPage(summoner, region) { Title = "Profile", Icon = "Profile.png" });
			Children.Add(new ChampsPage() { Title = "Database", Icon = "Books.png" });
			Children.Add(new SettingsNavPage() { Title = "About", Icon = "Cog.png" });
		}
	}

	public class ChampsPage : NavigationPage
	{
		public ChampsPage() : base(new ChampListPage())
		{
		}
	}

	public class SettingsNavPage : NavigationPage
	{
		public SettingsNavPage() : base(new SettingsPage())
		{
		}
	}

	public class SearchNavPage : NavigationPage
	{
		public SearchNavPage() : base(new SearchTabPage())
		{
		}
	}

	public class ProfPage : NavigationPage
	{
		public ProfPage(Summoner summoner, Region region) : base(new ProfilePage(summoner, region))
		{
		}
	}
}