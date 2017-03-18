using System;
using Acr.UserDialogs;
using Xamarin.Forms;
using Plugin.InAppBilling;
using System.Diagnostics;
using System.IO;

namespace LeagueApplication1
{
	public class SettingsPage : ContentPage
	{
		TableView table;

		public SettingsPage()
		{
			if (!DependencyService.Get<ISaveAndLoad>().FileExists("haveyou"))
			{
				table = new TableView
				{
					Intent = TableIntent.Form,
					BackgroundColor = Color.White,
					Root = new TableRoot("Table Title") {
						new TableSection ("Developer Links") {
							new TextCell {
								Text = "Legal Stuff",
								Command = new Command(async () =>
								{
									await UserDialogs.Instance.AlertAsync("","League Summoners isn't endorsed by Riot Games and doesn't reflect the views or opinions of Riot Games or anyone officially involved in producing or managing League of Legends. League of Legends and Riot Games are trademarks or registered trademarks of Riot Games, Inc. League of Legends © Riot Games, Inc.","Okay");
									//await Navigation.PushAsync(contentPage);
								})
							},
							new TextCell {
								Text = "Remove ads",
								Command = new Command(async () =>
								{
									try
									{
										var productId = "com.echo000.SummonersLoL.removeAds";
										var connected = await CrossInAppBilling.Current.ConnectAsync();

										if(!connected)
										{
											return;
										}
										var purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, Plugin.InAppBilling.Abstractions.ItemType.InAppPurchase, "apppayload");
										if(purchase == null)
										{
											//Purchase failed
										}
										else
										{
											DependencyService.Get<ISaveAndLoad>().SaveText("haveyou", "yes");
										}
									}
									catch(Exception ex)
									{
										Debug.WriteLine(ex);
									}
									finally
									{
										await CrossInAppBilling.Current.DisconnectAsync();
									}

								})
							},

								new TextCell {
									Text = "Twitter",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://twitter.com/cameronFinn";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitch",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.twitch.tv/echo000";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								}
							},
							new TableSection ("League Of Legends Links") {
								new TextCell {
									Text = "Reddit",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.reddit.com/r/leagueoflegends/";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitch",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.twitch.tv/directory/game/League%20of%20Legends";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Facebook",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.facebook.com/leagueoflegends";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitter",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://twitter.com/LeagueOfLegends";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "YouTube",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.youtube.com/user/RiotGamesInc";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								}
							},
							new TableSection("Riot Games Links")
							{
								new TextCell {
									Text = "Twitch",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.twitch.tv/RiotGames";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Facebook",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.facebook.com/RiotGames";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitter",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://twitter.com/riotgames";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
							}
						}
					}
				};
			}
			else
			{
				table = new TableView
				{
					Intent = TableIntent.Form,
					BackgroundColor = Color.White,
					Root = new TableRoot("Table Title") {
							new TableSection ("Developer Links") {
								new TextCell {
									Text = "Twitter",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://twitter.com/cameronFinn";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitch",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.twitch.tv/echo000";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								}
							},
							new TableSection ("League Of Legends Links") {
								new TextCell {
									Text = "Reddit",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.reddit.com/r/leagueoflegends/";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitch",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.twitch.tv/directory/game/League%20of%20Legends";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Facebook",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.facebook.com/leagueoflegends";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitter",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://twitter.com/LeagueOfLegends";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "YouTube",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.youtube.com/user/RiotGamesInc";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								}
							},
							new TableSection("Riot Games Links")
							{
								new TextCell {
									Text = "Twitch",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.twitch.tv/RiotGames";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Facebook",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://www.facebook.com/RiotGames";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								},
								new TextCell {
									Text = "Twitter",
									Command = new Command(async () =>
									{
										var web = new ContentPage();
										var webView = new WebView();
										webView.Source = "https://twitter.com/riotgames";
										web.Content = webView;
										await Navigation.PushAsync(web);
									})
								}
							}
					}
				};
			}
			Content = table;
		}
	}
}

