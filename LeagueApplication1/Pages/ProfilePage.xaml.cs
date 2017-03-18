using System;
using Xamarin.Forms;
using RiotSharp;
using System.Collections.Generic;
using RiotSharp.SummonerEndpoint;
using System.Linq;
using RiotSharp.StaticDataEndpoint;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Acr.UserDialogs;
using System.Diagnostics;
using RiotSharp.MatchEndpoint;
using System.Net;
using FFImageLoading.Forms;

namespace LeagueApplication1
{
	public partial class ProfilePage : ContentPage
	{

		public static RiotApi api = App.api;
		public static StaticRiotApi staticApi = App.staticApi;
		public static Region Reg;
		public static Region searchRegion = Region.na;
		public static Summoner Summoner;
		static ObservableCollection<GroupedSummoners> grouped { get; set; }
		public static List<Summoner> summoners;
		public static Dictionary<long, List<RiotSharp.LeagueEndpoint.League>> allLeagues;
		public static RiotSharp.LeagueEndpoint.League ranked5v5League;
		public static string div;
		public static RiotSharp.LeagueEndpoint.League rankedFlexLeague;
		public static string flexDiv;
		public static RiotSharp.LeagueEndpoint.League rankedFlexTTLeague;
		public static string flexTTDiv;
		static public FilterControl filterControl;
		static public FilterControl searchFilterControl;
		public static ContentView currentGameView;
		public bool searchBarShown = false;
		ICredentialsService storeService;
		public static List<RiotSharp.GameEndpoint.Game> GameList;

		public string skinString = "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/{0}_{1}.jpg";
		static string icon = "http://avatar.leagueoflegends.com/summonerId/{0}/{1}.png";
		public static bool updated = false;

		public ProfilePage(Summoner summoner, Region region)
		{
			InitializeComponent();
			search.IsVisible = false;
			nameText.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);
			Title = "Profile";

			storeService = DependencyService.Get<ICredentialsService>();
			var logoutButton = new ToolbarItem { Text = "Logout", Priority = 1 };
			var searchButton = new ToolbarItem { Text = "Search", Icon = "Search.png", Priority = 0 };

			ToolbarItems.Add(logoutButton);
			ToolbarItems.Add(searchButton);

			logoutButton.Clicked += async (sender, e) =>
			{
				var logout = await UserDialogs.Instance.ConfirmAsync("", "Are you sure you want to Logout?", "Yes", "No");
				if (logout)
				{
					if (storeService.DoCredentialsExist())
					{
						storeService.DeleteCredentials();
						Application.Current.MainPage = new NavigationPage(new loginXaml());
						await Navigation.PopToRootAsync();
					}
				}
			};

			search.ScopeChanged += (sender, e) =>
			{
				searchRegion = App.regFromString(search.SelectedScopeText);
			};

			searchButton.Clicked += (sender, e) =>
			{
				if (search.IsFocused)
				{
					search.Unfocus();
					searchFilterControl.Unfocus();
				}
				else
				{
					search.IsVisible = true;
					search.Focus();
				}
			};

			search.SearchButtonPressed += async (sender, e) =>
			{
				string searchText = System.Net.WebUtility.HtmlEncode(search.Text); //search.Text.Replace(" ", "");
				search.Text = "";
				search.Unfocus();
				try
				{
					UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
					var searchedSummoner = await App.api.GetSummonerAsync(searchRegion, searchText);
					icon = string.Format(icon, searchedSummoner.Region, searchedSummoner.Id);
					var test = new SearchedSummonerPage(searchedSummoner, searchedSummoner.Region);
					UserDialogs.Instance.HideLoading();
					await Navigation.PushAsync(test);
				}
				catch
				{
					UserDialogs.Instance.HideLoading();
					await UserDialogs.Instance.AlertAsync("Try again", "Summoner not found!", "Okay");
				}

			};

			var task = Task.Run(async () =>
			{
				if (freqPlayedChamp.Source == null)
				{
					GameList = await api.GetRecentGamesAsync(region, summoner.Id);
					var freqPlayed = GameList.GroupBy(q => q.ChampionId)
											  .OrderByDescending(gp => gp.Count())
											  .Select(g => g.Key)
											  .First();
					var championPlayedMost = await staticApi.GetChampionAsync(Reg, (int)freqPlayed, new List<ChampionData> { ChampionData.skins });
					freqPlayedChamp.Source = new UriImageSource
					{
						Uri = new Uri(string.Format(skinString, championPlayedMost.Key, championPlayedMost.Skins[0].Num)),
						CachingEnabled = false
					};
					updated = true;
				}
			});
			task.Wait();


			search.Unfocused += (sender, e) =>
			{
				search.IsVisible = false;
			};

			Reg = region;
			Summoner = summoner;
			filterControl = new FilterControl { Items = new List<string> { "Match History", "Ranked", "Current Game", "Champions" } };
			filterControl.SelectedIndex = 0;
			contentViewFilter.Content = filterControl;
			//contentVue.VerticalOptions = LayoutOptions.FillAndExpand;

			nameText.Text = "\n\t\t  " + summoner.Name;

			summonerIcon.Source = new UriImageSource
			{
				Uri = new Uri(string.Format(icon, region, summoner.Id)),
				CachingEnabled = true
			};

			switch (App.device.HardwareVersion)
			{
				case "iPhone SE":
				case "iPhone 5S":
				case "iPhone 5S GSM":
				case "iPhone 5S CDMA":
				case "iPhone 5":
				case "iPhone 5 GSM":
				case "iPhone 5 CDMA":
					contentVue.HeightRequest = 230;
					break;
				case "iPhone 6":
				case "iPhone 6S":
				case "iPhone 7":
					contentVue.HeightRequest = 330;
					break;
				case "iPhone 6+":
				case "iPhone 6s+":
				case "iPhone 6 Plus":
				case "iPhone 6S Plus":
				case "iPhone 7 Plus":
				case "iPhone 7+":
					contentVue.HeightRequest = 410;
					break;
			}
			try
			{
				allLeagues = api.GetLeagues(region, new List<int> { (int)summoner.Id });
				ranked5v5League = allLeagues[summoner.Id].Single(x => x.Queue == Queue.RankedSolo5x5);
				filterControl.Items = new List<string> { "Match History", "Ranked", "Current Game"};
			}
			catch
			{
				filterControl.Items = new List<string> { "Match History", "Current Game"};
			}

			filterControl.SelectedIndexChanged += async (object sender, EventArgs e) =>
			{
				UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);
				try
				{
					allLeagues = await api.GetLeaguesAsync(region, new List<int> { (int)summoner.Id });
					ranked5v5League = allLeagues[summoner.Id].Single(x => x.Queue == Queue.RankedSolo5x5);

					switch (filterControl.SelectedIndex)
					{
						case 0:
							var MatchList = await loadMatchHistory(summoner, region, this);
							contentVue.Content = MatchList;
							UserDialogs.Instance.HideLoading();
							break;
						case 1:
							var statsPage = await loadRanked(summoner, summoner.Region, this);
							contentVue.Content = statsPage;
							UserDialogs.Instance.HideLoading();
							break;
						case 2:
							currentGameView = await loadCurrentGameView(summoner, summoner.Region, this);
							contentVue.Content = currentGameView;
							UserDialogs.Instance.HideLoading();
							break;
					}
				}
				catch
				{
					switch (filterControl.SelectedIndex)
					{
						case 0:
							var MatchList = await loadMatchHistory(summoner, region, this);
							contentVue.Content = MatchList;
							UserDialogs.Instance.HideLoading();
							break;
						case 1:
							currentGameView = await loadCurrentGameView(summoner, summoner.Region, this);
							contentVue.Content = currentGameView;
							UserDialogs.Instance.HideLoading();
							break;
					}
				}
			};


			var soloRankString = "";
			var soloIcon = "";
			var flexRankString = "";
			var flexIcon = "";
			try
			{
				div = ranked5v5League.Entries.Where(x => x.PlayerOrTeamId == summoner.Id.ToString()).Select(x => x.Division).Single();
				var soloLP = ranked5v5League.Entries.FirstOrDefault(x => x.PlayerOrTeamName == summoner.Name).LeaguePoints;
				soloRankString = ranked5v5League.Tier + " " + div + " " + soloLP + "LP\nSolo Queue";

				switch (ranked5v5League.Tier)
				{
					case RiotSharp.LeagueEndpoint.Enums.Tier.Bronze:
						soloIcon = "Bronze.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Silver:
						soloIcon = "Silver.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Gold:
						soloIcon = "Gold.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Platinum:
						soloIcon = "Platinum.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Diamond:
						soloIcon = "Diamond.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Master:
						soloIcon = "Master.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Challenger:
						soloIcon = "Challenger.png";
						break;
				}
			}
			catch
			{
				soloRankString = "Unranked";
				soloIcon = "Unranked.png";
			}
			try
			{
				rankedFlexLeague = allLeagues[summoner.Id].Single(x => x.Queue == Queue.RankedFlexSR);
				flexDiv = rankedFlexLeague.Entries.Where(x => x.PlayerOrTeamId == summoner.Id.ToString()).Select(x => x.Division).Single();
				var flexLP = rankedFlexLeague.Entries.FirstOrDefault(x => x.PlayerOrTeamName == summoner.Name).LeaguePoints;
				flexRankString = rankedFlexLeague.Tier + " " + flexDiv + " " + flexLP + "LP\nFlex 5v5";

				switch (rankedFlexLeague.Tier)
				{
					case RiotSharp.LeagueEndpoint.Enums.Tier.Bronze:
						flexIcon = "Bronze.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Silver:
						flexIcon = "Silver.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Gold:
						flexIcon = "Gold.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Platinum:
						flexIcon = "Platinum.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Diamond:
						flexIcon = "Diamond.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Master:
						flexIcon = "Master.png";
						break;
					case RiotSharp.LeagueEndpoint.Enums.Tier.Challenger:
						flexIcon = "Challenger.png";
						break;
				}
			}
			catch
			{
				flexRankString = "Unranked";
				flexIcon = "Unranked.png";
			}

			var grid = new Grid
			{
				RowDefinitions = {
					new RowDefinition { Height = new GridLength(50, GridUnitType.Absolute)}

				},
				ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute)},
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute)},
					new ColumnDefinition { Width = GridLength.Auto },
				}
			};

			grid.Children.Add(new Image { Source = soloIcon }, 0, 0);
			grid.Children.Add(new Label { TextColor = Color.White, Text = soloRankString }, 1, 0);

			grid.Children.Add(new Image { Source = flexIcon }, 2, 0);
			grid.Children.Add(new Label { TextColor = Color.White, Text = flexRankString }, 3, 0);

			grid.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);
			rankedImagesStuff.Content = grid;
		}

		protected override async void OnAppearing()
		{
			UserDialogs.Instance.ShowLoading("Loading", MaskType.Black);

			if (App.itemList.Count == 0)
			{
				var allItems = await staticApi.GetItemsAsync(Region.euw, new List<ItemData> { ItemData.image, ItemData.gold });
				var tempItems = allItems.Items.Values.ToList();
				foreach (var item in tempItems)
				{
					item.Icon = new UriImageSource
					{
						Uri = new Uri(string.Format("https://ddragon.leagueoflegends.com/cdn/{0}/img/item/{1}", App.appVersion, item.Image.Full)),
						CachingEnabled = false
					};
				}
				App.itemList = tempItems;
			}

			if (App.summonerSpellList.Count == 0)
			{
				var allSpells = await staticApi.GetSummonerSpellsAsync(Region.euw, new List<SummonerSpellData> { SummonerSpellData.image, SummonerSpellData.cooldown });
				var tempSpells = allSpells.SummonerSpells.Values.ToList();
				foreach (var spell in tempSpells)
				{
					spell.Icon = new UriImageSource
					{
						Uri = new Uri(string.Format("https://ddragon.leagueoflegends.com/cdn/{0}/img/spell/{1}", App.appVersion, spell.Image.Full)),
						CachingEnabled = false
					};
				}
				App.summonerSpellList = tempSpells;
			}
			base.OnAppearing();
			var MatchList = await loadMatchHistory(Summoner, Reg, this);
			contentVue.Content = MatchList;
			UserDialogs.Instance.HideLoading();
		}

		public async Task<ContentView> loadRanked(Summoner summoner, Region region, Page SearchPage)
		{
			var allItems = App.itemList;
			var allSummonerSpells = App.summonerSpellList;
			var previousMatches = new List<MatchDetail>();
			var prevMatchList = await api.GetMatchListAsync(summoner.Region, summoner.Id, null, null, null, null, null, 0, 9);
			foreach (var match in prevMatchList.Matches)
			{
				var tempMatch = await api.GetMatchAsync(region, match.MatchID);
				previousMatches.Add(tempMatch);
			}
			var previousRanked = new List<PreviousMatchesFile>();
			for (int i = 0; i < previousMatches.Count; i++)
			{
				var gameType = "Ranked";
				var playerId = previousMatches[i].ParticipantIdentities.FirstOrDefault(s => s.Player.SummonerId == summoner.Id).ParticipantId;
				var player = previousMatches[i].Participants.FirstOrDefault(j => j.ParticipantId == playerId);
				var champPlayed = App.championList.First(x => x.Id == player.ChampionId);
				var summonerSpellList = new List<UriImageSource>
				{
					allSummonerSpells.FirstOrDefault(y => y.Id == player.Spell1Id).Icon,
					allSummonerSpells.FirstOrDefault(y => y.Id == player.Spell2Id).Icon
				};
				var itemList = new List<ItemStatic>
				{
					allItems.FirstOrDefault((x) => x.Id == player.Stats.Item0),
					allItems.FirstOrDefault((x) => x.Id == player.Stats.Item1),
					allItems.FirstOrDefault((x) => x.Id == player.Stats.Item2),
					allItems.FirstOrDefault((x) => x.Id == player.Stats.Item3),
					allItems.FirstOrDefault((x) => x.Id == player.Stats.Item4),
					allItems.FirstOrDefault((x) => x.Id == player.Stats.Item5),
					allItems.FirstOrDefault((x) => x.Id == player.Stats.Item6)
				};
				var itemNameList = new List<UriImageSource>();
				for (int j = 0; j < itemList.Count; j++)
				{
					if (itemList[j] != null)
						itemNameList.Add(itemList[j].Icon);
					else
						itemNameList.Add(App.emptyIcon);
				}

				var prev = new PreviousMatchesFile(
					gameType,
					player.ChampionId,
					(int)player.Stats.ChampLevel,
					player.Stats.Winner,
					App.FormatNumber((int)player.Stats.GoldEarned),
					player.Stats.MinionsKilled.ToString(),
					itemNameList,
					summonerSpellList,
					champPlayed.Icon,
					previousMatches[i].MatchCreation.ToString("g"),
					player.Stats.Kills + "/" + player.Stats.Deaths + "/" + player.Stats.Assists,
					previousMatches[i]
				);
				previousRanked.Add(prev);
			}


			var imageTemplate = new DataTemplate(typeof(PreviousGameCell));
			var listView = new ListView
			{
				ItemTemplate = imageTemplate,
				HasUnevenRows = true,
				ItemsSource = previousRanked
			};

			listView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
			{
				var myListView = (ListView)sender;
				var myItem = (PreviousMatchesFile)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading Game", MaskType.Black);
				try
				{
					Debug.WriteLine(myItem.ThisGame.MatchId);
					var previousPage = await PreviousRankedPage.loadPrevious(myItem.ThisGame, Summoner.Region, Summoner, myItem.champId, myItem.ThisGame.Participants, myItem.ThisGame.ParticipantIdentities);
					UserDialogs.Instance.HideLoading();
					await SearchPage.Navigation.PushAsync(previousPage);
				}
				catch (Exception ex)
				{
					UserDialogs.Instance.HideLoading();
					Debug.WriteLine(ex);
					await UserDialogs.Instance.AlertAsync("Are you connected to the internet?\nTry again", "An Error Occured", "Okay");
				}
			};

			if (!DependencyService.Get<ISaveAndLoad>().FileExists("haveyou"))
			{
				var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };
				listView.Header = adView;
			}

			var contentView = new ContentView
			{
				Content = new StackLayout
				{
					Children = {
						listView
					}
				}
			};
			return contentView;
		}

		public async Task<ListView> loadMatchHistory(Summoner summoner, Region region, Page SearchPage)
		{
			var allItems = App.itemList;
			var allSummonerSpells = App.summonerSpellList;
			List<RiotSharp.GameEndpoint.Game> previousMatches;
			if (updated)
			{
				previousMatches = GameList;
			}
			else
			{
				previousMatches = await api.GetRecentGamesAsync(region, summoner.Id);
			}
			var previousAll = new List<Previous>();
			for (int i = 0; i < previousMatches.Count; i++)
			{
				var gameType = App.gameTypeName(previousMatches[i].GameSubType);
				var champPlayed = App.championList.First(c => c.Id == previousMatches[i].ChampionId);
				var summonerSpellList = new List<UriImageSource>
				{
					allSummonerSpells.FirstOrDefault(y => y.Id == previousMatches[i].SummonerSpell1).Icon,
					allSummonerSpells.FirstOrDefault(y => y.Id == previousMatches[i].SummonerSpell2).Icon
				};
				var itemList = new List<ItemStatic>
				{
					allItems.FirstOrDefault((x) => x.Id == previousMatches[i].Statistics.Item0),
					allItems.FirstOrDefault((x) => x.Id == previousMatches[i].Statistics.Item1),
					allItems.FirstOrDefault((x) => x.Id == previousMatches[i].Statistics.Item2),
					allItems.FirstOrDefault((x) => x.Id == previousMatches[i].Statistics.Item3),
					allItems.FirstOrDefault((x) => x.Id == previousMatches[i].Statistics.Item4),
					allItems.FirstOrDefault((x) => x.Id == previousMatches[i].Statistics.Item5),
					allItems.FirstOrDefault((x) => x.Id == previousMatches[i].Statistics.Item6)
				};
				var itemNameList = new List<UriImageSource>();
				for (int j = 0; j < itemList.Count; j++)
				{
					if (itemList[j] != null)
						itemNameList.Add(itemList[j].Icon);
					else
						itemNameList.Add(App.emptyIcon);
				}

				var prev = new Previous(
					gameType,
					previousMatches[i].Level,
					previousMatches[i].Statistics.Win,
					App.FormatNumber(previousMatches[i].Statistics.GoldEarned),
					previousMatches[i].Statistics.MinionsKilled.ToString(),
					itemNameList,
					summonerSpellList,
					champPlayed.Icon,
					previousMatches[i].CreateDate.ToString("g"),
					previousMatches[i].Statistics.ChampionsKilled + "/" + previousMatches[i].Statistics.NumDeaths + "/" + previousMatches[i].Statistics.Assists,
					previousMatches[i]
				);
				previousAll.Add(prev);
			}

			var imageTemplate = new DataTemplate(typeof(PreviousGameCell));
			var listView = new ListView
			{
				ItemTemplate = imageTemplate,
				HasUnevenRows = true,
				ItemsSource = previousAll
			};

			listView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
			{
				var myListView = (ListView)sender;
				var myItem = (Previous)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading Game", MaskType.Black);
				try
				{
					var match = await api.GetMatchAsync(Summoner.Region, myItem.ThisGame.GameId);
					var previousPage = await PreviousGamePage.loadPrevious(match, myItem.ThisGame, Summoner.Region, Summoner, myItem.ThisGame.FellowPlayers);
					UserDialogs.Instance.HideLoading();
					await SearchPage.Navigation.PushAsync(previousPage);
				}
				catch (Exception ex)
				{
					UserDialogs.Instance.HideLoading();
					Debug.WriteLine(ex);
					await UserDialogs.Instance.AlertAsync("Are you connected to the internet?\nTry again", "An Error Occured", "Okay");
				}
			};
			if (!DependencyService.Get<ISaveAndLoad>().FileExists("haveyou"))
			{
				var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };
				listView.Header = adView;
			}
			updated = false;
			return listView;
		}

		//Loads the current game if there is one
		public async Task<ContentView> loadCurrentGameView(Summoner summoner, Region region, Page SearchPage)
		{
			var gameView = new ContentView();

			var currentTemplate = new DataTemplate(typeof(ImageCell));
			currentTemplate.SetBinding(TextCell.TextProperty, "Name");
			currentTemplate.SetBinding(ImageCell.ImageSourceProperty, "Icon");
			currentTemplate.SetBinding(TextCell.DetailProperty, "Rank");
			try
			{
				var currentGame = await api.GetCurrentGameAsync(App.PlatformToRegion(region), summoner.Id);
				grouped = new ObservableCollection<GroupedSummoners>();
				var blueTeam = new GroupedSummoners { LongName = "Blue Team", ShortName = "B" };
				var redTeam = new GroupedSummoners { LongName = "Red Team", ShortName = "R" };

				var list = new ListView();
				var idList = new List<string>();
				summoners = new List<Summoner>();
				for (int i = 0; i < currentGame.Participants.Count; i++)
				{
					idList.Add(currentGame.Participants[i].SummonerName);
				}
				summoners = await api.GetSummonersAsync(region, idList);

				for (int i = 0; i < currentGame.Participants.Count; i++)
				{
					for (int j = 0; j < summoners.Count; j++)
					{
						if (currentGame.Participants[i].SummonerName == summoners[j].Name)
						{
							var champPlayed = App.championList.First(c => c.Id == currentGame.Participants[i].ChampionId);
							try
							{
								allLeagues = await api.GetLeaguesAsync(region, new List<int> { (int)summoners[j].Id });
								ranked5v5League = allLeagues[summoners[j].Id].Single(x => x.Queue == Queue.RankedSolo5x5);
								div = ranked5v5League.Entries.Where(x => x.PlayerOrTeamId == summoners[j].Id.ToString()).Select(x => x.Division).Single();

								if (currentGame.Participants[i].TeamId == 100)
								{
									blueTeam.Add(new CurrentGamePlayers(summoners[j].Name,
									                                    ranked5v5League.Tier + " " + div,
									                                    (int)currentGame.Participants[i].ChampionId,
									                                    champPlayed.Icon,
									                                    summoners[i].Region));
								}
								else if (currentGame.Participants[i].TeamId == 200)
								{
									redTeam.Add(new CurrentGamePlayers(summoners[j].Name,
									                                   ranked5v5League.Tier + " " + div,
									                                   (int)currentGame.Participants[i].ChampionId,
									                                   champPlayed.Icon,
									                                   summoners[i].Region));
								}
							}
							catch
							{
								if (currentGame.Participants[i].TeamId == 100)
								{
									blueTeam.Add(new CurrentGamePlayers(summoners[j].Name,
									                                    "Level " + summoners[i].Level,
									                                    (int)currentGame.Participants[i].ChampionId,
									                                    champPlayed.Icon,
									                                    summoners[i].Region));
								}
								else if (currentGame.Participants[i].TeamId == 200)
								{
									redTeam.Add(new CurrentGamePlayers(summoners[j].Name,
																		"Level " + summoners[i].Level,
																		(int)currentGame.Participants[i].ChampionId,
																		champPlayed.Icon,
																		summoners[i].Region));
								}
							}
						}
					}
				}
				grouped.Add(blueTeam);
				grouped.Add(redTeam);

				list.ItemsSource = grouped;
				list.IsGroupingEnabled = true;
				list.GroupDisplayBinding = new Binding("LongName");
				list.ItemTemplate = currentTemplate;

				list.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
				{
					var myListView = (ListView)sender;
					var myItem = (CurrentGamePlayers)myListView.SelectedItem;
					try
					{
						UserDialogs.Instance.ShowLoading("Loading " + myItem.Name, MaskType.Black);
						var summonerAsync = await api.GetSummonerAsync(myItem.Region, myItem.Name);
						var page = SearchPage.Navigation.NavigationStack.Last();
						var test = new SearchedSummonerPage(summonerAsync, myItem.Region);
						UserDialogs.Instance.HideLoading();
						await SearchPage.Navigation.PushAsync(test);
						SearchPage.Navigation.RemovePage(page);
					}
					catch
					{
						UserDialogs.Instance.Alert("Whoops", "Something went wrong", "Okay");
					}
				};

				if (!DependencyService.Get<ISaveAndLoad>().FileExists("haveyou"))
				{
					var adView = new AdMobView { WidthRequest = 320, HeightRequest = 50 };
					list.Header = adView;
				}
				gameView.Content = list;
			}
			catch
			{
				gameView.Content = new Label { Text = summoner.Name + " is not in an active game. Please try again later if the summoner is currently in game." };
			}
			return gameView;
		}
	}
	public class FilterHeaderCell : ViewCell
	{
		public FilterHeaderCell()
		{
			var filter = new FilterControl();
			filter.SetBinding(FilterControl.ItemsProperty, "Items");

			var cellLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				//Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { filter }
			};

			View = cellLayout;
		}
	}
}
