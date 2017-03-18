using Xamarin.Forms;
using RiotSharp;
using System.Threading.Tasks;
using RiotSharp.StaticDataEndpoint;
using RiotSharp.MatchEndpoint;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RiotSharp.SummonerEndpoint;
using RiotSharp.GameEndpoint;
using System;

namespace LeagueApplication1
{
	public class PreviousGamePage : ContentPage
	{
		ListView listView;
		static ObservableCollection<GroupedPrevious> grouped { get; set; }

		public PreviousGamePage(ObservableCollection<GroupedPrevious> grouped)
		{
			Title = "Previous Game";
			var imageTemplate = new DataTemplate(typeof(ExpandedGameCell));
			listView = new ListView
			{
				IsGroupingEnabled = true,
				ItemTemplate = imageTemplate,
				ItemsSource = grouped,
				HasUnevenRows = true,
				GroupDisplayBinding = new Binding("LongName")
			};
			if (!DependencyService.Get<ISaveAndLoad>().FileExists("haveyou"))
			{
				var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };
				listView.Header = adView;
			}
			Content = listView;
		}

		public static async Task<PreviousGamePage> loadPrevious(MatchDetail thisGame, Game game, Region region, Summoner summonerPls, List<RiotSharp.GameEndpoint.Player> fellowPlayers)
		{
			var staticApi = App.staticApi;
			var api = App.api;
			grouped = new ObservableCollection<GroupedPrevious>();
			var blueTeam = new GroupedPrevious { LongName = "Blue Team", ShortName = "B" };
			var redTeam = new GroupedPrevious { LongName = "Red Team", ShortName = "R" };
			var playersInPrev = new List<playersInPrevious>();

			var allItems = App.itemList;
			var allSummonerSpells = App.summonerSpellList;

			playersInPrev.Add(new playersInPrevious { ChampionId = game.ChampionId, SummonerId = (int)summonerPls.Id });
			for (int i = 0; i < fellowPlayers.Count; i++)
			{
				playersInPrev.Add(new playersInPrevious { ChampionId = fellowPlayers[i].ChampionId, SummonerId = (int)fellowPlayers[i].SummonerId });
			}

			for (int i = 0; i < thisGame.Participants.Count; i++)
			{
				for (int j = 0; j < playersInPrev.Count; j++)
				{
					if (thisGame.Participants[i].ChampionId == playersInPrev[j].ChampionId)
					{
						var summoner = await api.GetSummonerAsync(region, playersInPrev[j].SummonerId);
						var champPlayed = await staticApi.GetChampionAsync(region, thisGame.Participants[i].ChampionId, new List<ChampionData> { ChampionData.image });
						var summonerSpellList = new List<UriImageSource>
						{
							allSummonerSpells.FirstOrDefault(y => y.Id == thisGame.Participants[i].Spell1Id).Icon,
							allSummonerSpells.FirstOrDefault(y => y.Id == thisGame.Participants[i].Spell2Id).Icon
						};
						var itemList = new List<ItemStatic>
						{
							allItems.FirstOrDefault((x) => x.Id == thisGame.Participants[i].Stats.Item0),
							allItems.FirstOrDefault((x) => x.Id == thisGame.Participants[i].Stats.Item1),
							allItems.FirstOrDefault((x) => x.Id == thisGame.Participants[i].Stats.Item2),
							allItems.FirstOrDefault((x) => x.Id == thisGame.Participants[i].Stats.Item3),
							allItems.FirstOrDefault((x) => x.Id == thisGame.Participants[i].Stats.Item4),
							allItems.FirstOrDefault((x) => x.Id == thisGame.Participants[i].Stats.Item5),
							allItems.FirstOrDefault((x) => x.Id == thisGame.Participants[i].Stats.Item6)
						};
						var itemNameList = new List<UriImageSource>();
						for (int z = 0; z < itemList.Count; z++)
						{
							if (itemList[z] != null)
								itemNameList.Add(itemList[z].Icon);
							else
								itemNameList.Add(new UriImageSource { CachingEnabled = false, Uri = new Uri("https://raw.githubusercontent.com/echo000/testProject/master/Empty.png") });
						}
						var prev = new PreviousExpanded(
							summoner.Name,
							(int)thisGame.Participants[i].Stats.ChampLevel,
							thisGame.Participants[i].Stats.Winner,
							App.FormatNumber((int)thisGame.Participants[i].Stats.GoldEarned),
							thisGame.Participants[i].Stats.MinionsKilled.ToString(),
							itemNameList,
							summonerSpellList,
							champPlayed.Icon,
							thisGame.Participants[i].Stats.Kills + "/" + thisGame.Participants[i].Stats.Deaths + "/" + thisGame.Participants[i].Stats.Assists
						);
						if (thisGame.Participants[i].TeamId == 100)
						{
							blueTeam.Add(prev);
						}
						else if (thisGame.Participants[i].TeamId == 200)
						{
							redTeam.Add(prev);
						}
					}
				}
			}
			grouped.Add(blueTeam);
			grouped.Add(redTeam);
			return new PreviousGamePage(grouped);
		}
	}
	public class GroupedPrevious : ObservableCollection<PreviousExpanded>
	{
		public string LongName { get; set; }
		public string ShortName { get; set; }
	}
	public class playersInPrevious
	{
		public int SummonerId { get; set; }
		public int ChampionId { get; set; }
	}
}