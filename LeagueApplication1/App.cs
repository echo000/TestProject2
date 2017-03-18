using System.Diagnostics;
using Xamarin.Forms;
using RiotSharp;
using XLabs.Ioc;
using XLabs.Platform.Device;
using RiotSharp.GameEndpoint.Enums;
using RiotSharp.StatsEndpoint.Enums;
using RiotSharp.StaticDataEndpoint;
using System.Linq;
using System;
using Amazon.Lambda;
using Plugin.Connectivity;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LeagueApplication1
{
	public class App : Application
	{
		public static RiotApi api = new RiotApi();
		public static StaticRiotApi staticApi = new StaticRiotApi();
		public static IDevice device;
		public static IDisplay display;
		public const string AppName = "SummonersLol";
		public static UriImageSource emptyIcon;
		public static TabbedPages MyTabbedPage;
		public static string appVersion;
		public static List<ItemStatic> itemList = new List<ItemStatic>();
		public static List<ChampionStatic> championList = new List<ChampionStatic>();
		public static List<SummonerSpellStatic> summonerSpellList = new List<SummonerSpellStatic>();

		public static AmazonLambdaClient lambdaClient = new AmazonLambdaClient("AKIAJRMKSQGGPBOROCZQ", "Ztt0zV+xP4p2yZar1JPhyfcQC6WuVB9OImn50+7d", Amazon.RegionEndpoint.USEast1);


		public App()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				MainPage = new NavigationPage(new NoInternetPage());
			}
			else
			{
				var versions = staticApi.GetVersions(Region.euw);
				appVersion = versions[0];
				LoadPageConnected();
			}
			CrossConnectivity.Current.ConnectivityChanged += async (sender, e) =>
			{
				if (!e.IsConnected)
				{
					await UserDialogs.Instance.AlertAsync("Error", "You have lost internet connection", "Okay");
					MainPage = new NavigationPage(new NoInternetPage());
				}
				else
				{
					LoadPageConnected();
				}
			};
		}

		public static void LoadPageConnected()
		{
			device = Resolver.Resolve<IDevice>();
			Debug.WriteLine(device.HardwareVersion);
			display = device.Display;
			emptyIcon = new UriImageSource { CachingEnabled = false, Uri = new Uri("https://raw.githubusercontent.com/echo000/testProject/master/Empty.png") };

			Current.Resources = new ResourceDictionary();

			if (DependencyService.Get<ICredentialsService>().DoCredentialsExist())
			{
				string id = DependencyService.Get<ICredentialsService>().ProfileId;
				var region = regFromString(DependencyService.Get<ICredentialsService>().Region);
				var x = int.Parse(id);
				var summoner = api.GetSummoner(region, x);
				MyTabbedPage = new TabbedPages(summoner, summoner.Region);
				Application.Current.MainPage = MyTabbedPage;
			}
			else
			{
				Application.Current.MainPage = new NavigationPage(new loginXaml());
			}
		}

		public static class Constants
		{
			public static string EUW = "euw";
			public static string EUNE = "eune";
			public static string NA = "na";
			public static string KR = "kr";
			public static string BR = "br";
			public static string LAN = "lan";
			public static string LAS = "las";
			public static string OCE = "oce";
			public static string RU = "ru";
			public static string TR = "tr";
			public static string JP = "jp";
		}

		public int ResumeAtTodoId { get; set; }

		protected override async void OnStart()
		{
			Debug.WriteLine("OnStart");
			if (Properties.ContainsKey("ResumeAtTodoId"))
			{
				var rati = Properties["ResumeAtTodoId"].ToString();
				Debug.WriteLine("rati=" + rati);
				if (!string.IsNullOrEmpty(rati))
				{
					Debug.WriteLine("rati = " + rati);
					ResumeAtTodoId = int.Parse(rati);

					if (ResumeAtTodoId >= 0)
					{
						//ar todoPage = new TodoItemPageX();
						//todoPage.BindingContext = Database.GetItem(ResumeAtTodoId);

						//MainPage.Navigation.PushAsync(
							//todoPage,
							//false); // no animation
					}
				}
			}
		}
		protected override void OnSleep()
		{
			Debug.WriteLine("OnSleep saving ResumeAtTodoId = " + ResumeAtTodoId);
			// the app should keep updating this value, to
			// keep the "state" in case of a sleep/resume
			Properties["ResumeAtTodoId"] = ResumeAtTodoId;
		}
		protected override void OnResume()
		{
			Debug.WriteLine("OnResume");
			if (!CrossConnectivity.Current.IsConnected)
			{
				MainPage = new NavigationPage(new NoInternetPage());
			}
		}

		public static Region regFromString(string regionName)
		{
			switch (regionName)
			{
				case "euw":
					return Region.euw;
				case "eune":
					return Region.eune;
				case "na":
					return Region.na;
				case "kr":
					return Region.kr;
				case "br":
					return Region.br;
				case "lan":
					return Region.lan;
				case "las":
					return Region.las;
				case "oce":
					return Region.oce;
				case "ru":
					return Region.ru;
				case "tr":
					return Region.tr;
				case "jp":
					return Region.jp;
			}
			return Region.global;
		}
		public static string FormatNumber(int num)
		{
			if (num >= 100000)
				return FormatNumber(num / 1000) + "K";
			if (num >= 10000)
			{
				return (num / 1000D).ToString("0.#") + "K";
			}
			return num.ToString("#,0");
		}
		public static string gameTypeName(GameSubType gamesubType)
		{
			switch (gamesubType)
			{
				case GameSubType.None:
					return "Custom Game";
				case GameSubType.Normal:
					return "Normal";
				case GameSubType.Normal3x3:
					return "Normal 3v3";
				case GameSubType.OdinUnranked:
					return "Dominion";
				case GameSubType.AramUnranked5x5:
					return "ARAM";
				case GameSubType.Bot:
					return "Co-op vs AI";
				case GameSubType.Bot3x3:
					return "Co-op vs AI 3v3";
				case GameSubType.RankedSolo5x5:
					return "Ranked Solo";
				case GameSubType.RankedTeam3x3:
					return "Ranked Teams 3v3";
				case GameSubType.RankedTeam5x5:
					return "Ranked Teams 5v5";
				case GameSubType.OneForAll5x5:
					return "One For All";
				case GameSubType.FirstBlood1x1:
					return "Snowdown Showdown 1v1";
				case GameSubType.FirstBlood2x2:
					return "Snowdown Showdown 2v2";
				case GameSubType.SR_6x6:
					return "Summoners Rift 6v6";
				case GameSubType.TeamBuilder5x5:
					return "Team Builder 5v5";
				case GameSubType.URF:
					return "Ultra Rapid Fire";
				case GameSubType.URFBots:
					return "URF vs AI";
				case GameSubType.NightmareBot:
					return "Nightmate Mode";
				case GameSubType.Ascension:
					return "Ascension";
				case GameSubType.Hexakill:
					return "Hexakill";
				case GameSubType.KingPoro:
					return "Poro King";
				case GameSubType.CounterPick:
					return "Nemesis Draft";
				case GameSubType.Bilgewater:
					return "Black Market Brawlers";
				case GameSubType.NexusSiege:
					return "Nexus Siege";
				case GameSubType.RankedFlexSR:
					return "Ranked Flex 5v5";
				case GameSubType.RankedFlexTT:
					return "Ranked Flex 3v3";
			}
			return "";
		}
		//Gets the game sub type for the stats
		public static string getGameStatType(PlayerStatsSummaryType stats)
		{
			switch (stats)
			{
				case PlayerStatsSummaryType.Unranked:
					return "Normal";
				case PlayerStatsSummaryType.Unranked3x3:
					return "Unranked 3v3";
				case PlayerStatsSummaryType.OdinUnranked:
					return "Dominion";
				case PlayerStatsSummaryType.AramUnranked5x5:
					return "ARAM";
				case PlayerStatsSummaryType.CoopVsAI:
					return "Co-op vs AI";
				case PlayerStatsSummaryType.CoopVsAI3x3:
					return "Co-op vs AI 3v3";
				case PlayerStatsSummaryType.RankedSolo5x5:
					return "Ranked";
				case PlayerStatsSummaryType.RankedPremade5x5:
					return "Ranked Premade 5v5";
				case PlayerStatsSummaryType.RankedPremade3x3:
					return "Ranked Premade 3v3";
				case PlayerStatsSummaryType.RankedTeam3x3:
					return "Ranked Teams 3v3";
				case PlayerStatsSummaryType.RankedTeam5x5:
					return "Ranked Teams 5v5";
				case PlayerStatsSummaryType.OneForAll5x5:
					return "One For All";
				case PlayerStatsSummaryType.FirstBlood1x1:
					return "Snowdown Showdown 1v1";
				case PlayerStatsSummaryType.FirstBlood2x2:
					return "Snowdown Showdown 2v2";
				case PlayerStatsSummaryType.SummonersRift6x6:
					return "Summoners Rift Hexakill";
				case PlayerStatsSummaryType.CAP5x5:
					return "Team Builder";
				case PlayerStatsSummaryType.URF:
					return "Ultra Rapid Fire";
				case PlayerStatsSummaryType.URFBots:
					return "Ultra Rapid Fire Vs AI";
				case PlayerStatsSummaryType.NightmareBot:
					return "Nightmate Mode";
				case PlayerStatsSummaryType.Ascension:
					return "Ascension";
				case PlayerStatsSummaryType.Hexakill:
					return "Hexakill";
				case PlayerStatsSummaryType.KingPoro:
					return "Legend of the Poro King";
				case PlayerStatsSummaryType.CounterPick:
					return "Nemesis Draft";
				case PlayerStatsSummaryType.Bilgewater:
					return "Black Market Brawlers";
				case PlayerStatsSummaryType.Siege:
					return "Nexus Siege";
				case PlayerStatsSummaryType.RankedFlexSR:
					return "Ranked Flex 5v5";
				case PlayerStatsSummaryType.RankedFlexTT:
					return "Ranked Flex 3v3";
			}
			return "";
		}
		public static Platform PlatformToRegion(Region region)
		{
			switch (region)
			{
				case Region.euw:
					return Platform.EUW1;
				case Region.eune:
					return Platform.EUN1;
				case Region.br:
					return Platform.BR1;
				case Region.kr:
					return Platform.KR;
				case Region.lan:
					return Platform.LA1;
				case Region.las:
					return Platform.LA1;
				case Region.na:
					return Platform.NA1;
				case Region.oce:
					return Platform.OC1;
				case Region.ru:
					return Platform.RU;
				case Region.tr:
					return Platform.TR1;
				case Region.jp:
					return Platform.JP1;
			}
			return Platform.EUW1;
		}

		public static SummonerSpell GetSummonerSpellFromId(int id)
		{
			switch (id)
			{
				case 1:
					return SummonerSpell.Cleanse;
				case 2:
					return SummonerSpell.Clairvoyance;
				case 3:
					return SummonerSpell.Exhaust;
				case 4:
					return SummonerSpell.Flash;
				case 6:
					return SummonerSpell.Ghost;
				case 7:
					return SummonerSpell.Heal;
				case 11:
					return SummonerSpell.Smite;
				case 12:
					return SummonerSpell.Teleport;
				case 13:
					return SummonerSpell.Clarity;
				case 14:
					return SummonerSpell.Ignite;
				case 21:
					return SummonerSpell.Barrier;
				case 30:
					return SummonerSpell.PoroRecall;
				case 31:
					return SummonerSpell.PoroToss;
				case 32:
					return SummonerSpell.Mark;
			}
			return SummonerSpell.Ghost;
		}
	}
}

