using System;
using System.Collections.Generic;
using RiotSharp;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public partial class loginXaml : ContentPage
	{
		ICredentialsService storeService;
		Region region = Region.na;
		public loginXaml()
		{
			InitializeComponent();

			storeService = DependencyService.Get<ICredentialsService>();
			NavigationPage.SetHasNavigationBar(this, true);

			LoginText.Text = "Welcome to League Summoners!\nSelect the region and search your summoner name!";
			searchImage.Source = "NaughtyTeemo.png";

			search.SearchButtonPressed += async (sender, e) =>
			{
				var summonerName = search.Text.Replace(" ", "");
				var summoner = await App.api.GetSummonerAsync(region, summonerName);

				var doCredentialsExist = storeService.DoCredentialsExist();
				if (!doCredentialsExist)
				{
					storeService.SaveCredentials(summoner.Name, summoner.Region.ToString(), summoner.Id.ToString());
				}

				//NavigationPage.SetHasNavigationBar(this, false);
				Application.Current.MainPage = new TabbedPages(summoner, region);
				await Navigation.PopToRootAsync();
			};

			search.ScopeChanged += (sender, e) =>
			{
				region = App.regFromString(search.SelectedScopeText);
			};
		}
	}
}
