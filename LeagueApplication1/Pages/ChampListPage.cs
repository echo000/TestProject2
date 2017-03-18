using Xamarin.Forms;
using RiotSharp;
using Acr.UserDialogs;
using System.Collections.ObjectModel;
using RiotSharp.StaticDataEndpoint;
using System.Threading.Tasks;
using System.Linq;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;

namespace LeagueApplication1
{
	class ChampListPage : ContentPage
	{
		FilterControl filter;
		DataTemplate imageTemplate;
		DataTemplate otherTemplate;
		DataTemplate summonerSpellTemplate;
		ContentView CView = new ContentView();
		StaticRiotApi staticApi = App.staticApi;

		public ChampListPage()
		{
			Title = "Database";
			filter = new FilterControl { Items = new List<string> { "Champions", "Items", "Summoner Spells" } };

			imageTemplate = new DataTemplate(typeof(ChampionListCell));
			otherTemplate = new DataTemplate(typeof(ItemListCell));
			summonerSpellTemplate = new DataTemplate(typeof(SummonerSpellListCell));

			var task = Task.Run(async () =>
			{
				var championList = await loadChampions();
				CView.Content = championList;
			});
			task.Wait();

			filter.SelectedIndexChanged += async (sender, e) =>
			{
				switch (filter.SelectedIndex)
				{
					case 0:
						var championList = await loadChampions();
						CView.Content = championList;
						break;
					case 1:
						var itemList = await loadItems();
						CView.Content = itemList;
						break;
					case 2:
						var summonerSpellList = await loadSummonerSpells();
						CView.Content = summonerSpellList;
						break;

				}
			};

			Content = new StackLayout
			{
				Children = {
					filter,
					CView
				}
			};

			NavigationPage.SetHasNavigationBar(this, true);
		}


		async Task<ListView> loadSummonerSpells()
		{
			var listView = new ListView();
			listView.ItemTemplate = summonerSpellTemplate;

			var tempSpells = App.summonerSpellList;
			var items = new ObservableCollection<SummonerSpellStatic>(tempSpells);

			listView.ItemsSource = items;

			/*listView.ItemTapped += async (sender, e) =>
			{
				var myListView = (ListView)sender;
				var champion = (ChampionStatic)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading " + champion.Name, MaskType.Black);
				var champPlayed = await staticApi.GetChampionAsync(Region.euw, champion.Id, RiotSharp.StaticDataEndpoint.ChampionData.all);
				UserDialogs.Instance.HideLoading();
				await Navigation.PushAsync(new ChampionPage(champPlayed));
			};*/
			return listView;
		}

		async Task<ListView> loadItems()
		{
			var listView = new ListView();
			listView.ItemTemplate = otherTemplate;
			var list = App.itemList;
			var tempList2 = list.OrderBy(x => x.Gold.TotalPrice).ToList();
			var iList2 = tempList2.Where(x => x.Gold.TotalPrice > 10).ToList();
			var items = new ObservableCollection<ItemStatic>(iList2);

			/*foreach (var item in items)
			{
				if (item.Name == "Head of Kha'Zix"
				    || item.Name.Contains("Poro-Snax")
				    || item.Name.Contains("Golden Transcendence"))
				{
					items.Remove(item);
				}
			}*/

			listView.ItemsSource = items;

			/*listView.ItemTapped += async (sender, e) =>
			{
				var myListView = (ListView)sender;
				var champion = (ChampionStatic)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading " + champion.Name, MaskType.Black);
				var champPlayed = await staticApi.GetChampionAsync(Region.euw, champion.Id, RiotSharp.StaticDataEndpoint.ChampionData.all);
				UserDialogs.Instance.HideLoading();
				await Navigation.PushAsync(new ChampionPage(champPlayed));
			};*/
			return listView;
		}


		async Task<ListView> loadChampions()
		{
			if (App.championList.Count == 0)
			{
				var allChampions = await staticApi.GetChampionsAsync(Region.euw, new List<ChampionData> { ChampionData.image });
				var tempChampsList = allChampions.Champions.Values.ToList();
				foreach (var champ in tempChampsList)
				{
					champ.Icon = new UriImageSource
					{
						Uri = new Uri(string.Format("https://ddragon.leagueoflegends.com/cdn/{0}/img/champion/{1}", App.appVersion, champ.Image.Full)),
						CachingEnabled = false
					};
				}
				App.championList = tempChampsList;
			}

			var listView = new ListView();
			listView.ItemTemplate = imageTemplate;

			var tempChamps = App.championList;
			var tempList2 = tempChamps.OrderBy(x => x.Name).ToList();
			var champs = new ObservableCollection<ChampionStatic>(tempList2);
			listView.ItemsSource = champs;

			listView.ItemTapped += async (sender, e) =>
			{
				var myListView = (ListView)sender;
				var champion = (ChampionStatic)myListView.SelectedItem;
				UserDialogs.Instance.ShowLoading("Loading " + champion.Name, MaskType.Black);
				var champPlayed = await staticApi.GetChampionAsync(Region.euw, champion.Id, new List<ChampionData> { RiotSharp.StaticDataEndpoint.ChampionData.all });
				UserDialogs.Instance.HideLoading();
				await Navigation.PushAsync(new ChampionPage(champPlayed));
			};
			return listView;
		}
	}

	public class SummonerSpellListCell : ViewCell
	{
		public SummonerSpellListCell()
		{
			var SummonerSpellProfileImage = new CachedImage
			{
				WidthRequest = 43,
				HeightRequest = 43,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			SummonerSpellProfileImage.SetBinding(CachedImage.SourceProperty, "Icon");

			var nameLabel = new Label()
			{
				TextColor = Color.Black
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");

			var distanceLabel = new Label()
			{
				FontSize = 12,
				TextColor = Color.Accent
			};
			distanceLabel.SetBinding(Label.TextProperty, "Name");


			var vetDetailsLayout = new StackLayout
			{
				Padding = new Thickness(10, 5, 0, 0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { nameLabel, distanceLabel }
			};

			var champLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					SummonerSpellProfileImage, vetDetailsLayout
				}
			};

			var cellLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				//Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { champLayout }
			};

			View = cellLayout;
		}
	}

	public class ItemListCell : ViewCell
	{
		public ItemListCell()
		{
			var ItemProfileImage = new CachedImage
			{
				WidthRequest = 43,
				HeightRequest = 43,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			ItemProfileImage.SetBinding(CachedImage.SourceProperty, "Icon");

			var nameLabel = new Label()
			{
				TextColor = Color.Black
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");

			var distanceLabel = new Label()
			{
				FontSize = 12,
				TextColor = Color.Accent
			};
			distanceLabel.SetBinding(Label.TextProperty, "Gold.TotalPrice");


			var vetDetailsLayout = new StackLayout
			{
				Padding = new Thickness(10, 5, 0, 0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { nameLabel, distanceLabel }
			};

			var champLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					ItemProfileImage, vetDetailsLayout
				}
			};

			var cellLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				//Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { champLayout }
			};

			View = cellLayout;
		}
	}


	public class ChampionListCell : ViewCell
	{
		public ChampionListCell()
		{
			var champProfileImage = new CachedImage
			{
				WidthRequest = 43,
				HeightRequest = 43,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			champProfileImage.SetBinding(CachedImage.SourceProperty, "Icon");

			var nameLabel = new Label()
			{
				TextColor = Color.Black
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");

			var distanceLabel = new Label()
			{
				FontSize = 12,
				TextColor = Color.Accent
			};
			distanceLabel.SetBinding(Label.TextProperty, "Title");


			var vetDetailsLayout = new StackLayout
			{
				Padding = new Thickness(10, 5, 0, 0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { nameLabel, distanceLabel }
			};

			var champLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					champProfileImage, vetDetailsLayout
				}
			};

			var cellLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				//Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { champLayout }
			};

			View = cellLayout;
		}
	}
}

