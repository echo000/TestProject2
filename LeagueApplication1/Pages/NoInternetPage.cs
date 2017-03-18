using System;
using Acr.UserDialogs;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class NoInternetPage : ContentPage
	{
		public NoInternetPage()
		{
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "No internet connection." }
				}
			};
		}
	}
}

