using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class ChampionRankedViewCell : ViewCell
	{
		public ChampionRankedViewCell()
		{
			var champProfileImage = new CachedImage
			{
				WidthRequest = 40,
				HeightRequest = 40,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			champProfileImage.SetBinding(CachedImage.SourceProperty, "Icon");

			var KDALabel = new Label
			{
				TextColor = Color.Black
			};
			KDALabel.SetBinding(Label.TextProperty, "KDA");

			var CSLabel = new Label
			{
				TextColor = Color.Black
			};
			CSLabel.SetBinding(Label.TextProperty, "CreepScore");

			var winRateLabel = new Label
			{
				TextColor = Color.Black
			};
			winRateLabel.SetBinding(Label.TextProperty, "WinRate");

			var winRateProgressBar = new ProgressBar
			{
				WidthRequest = 200
			};
			winRateProgressBar.SetBinding(ProgressBar.ProgressProperty, "WinRateDecimal");

			var champLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					champProfileImage, winRateLabel, winRateProgressBar, KDALabel, CSLabel
				}
			};

			var cellLayout = new StackLayout
			{
				Padding = new Thickness(10, 5, 10, 5),
				//Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { champLayout }
			};

			this.View = cellLayout;
		}
	}
}

