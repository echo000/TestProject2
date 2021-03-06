﻿using System;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class ExpandedGameCell : ViewCell
	{
		public ExpandedGameCell()
		{
			var champProfileImage = new CachedImage
			{
				WidthRequest = 50,
				HeightRequest = 50,
				Aspect = Aspect.AspectFill,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			champProfileImage.SetBinding(CachedImage.SourceProperty, "Icon");

			var goldIcon = new Image
			{
				WidthRequest = 25,
				HeightRequest = 25,
				Source = "goldIcon.png"
			};

			var minionIcon = new Image
			{
				WidthRequest = 25,
				HeightRequest = 25,
				Source = "minion.png"
			};

			var GoldLabel = new Label
			{
				FontSize = 12,
				TextColor = Color.Black
			};
			GoldLabel.SetBinding(Label.TextProperty, "GoldEarned");

			var CSLabel = new Label
			{
				FontSize = 12,
				TextColor = Color.Black
			};
			CSLabel.SetBinding(Label.TextProperty, "CreepScore");

			var nameLabel = new Label()
			{
				TextColor = Color.Black
			};
			nameLabel.SetBinding(Label.TextProperty, "Name");

			var distanceLabel = new Label()
			{
				FontSize = 12
			};
			distanceLabel.SetBinding(Label.TextProperty, "Title");

			var summonerSpell1Image = new Image()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5
			};
			summonerSpell1Image.SetBinding(Image.SourceProperty, "SummonerSpell1Icon");

			var summonerSpell2Image = new Image()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5
			};
			summonerSpell2Image.SetBinding(Image.SourceProperty, "SummonerSpell2Icon");

			var Item0Image = new CachedImage()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			Item0Image.SetBinding(CachedImage.SourceProperty, "Item0Icon");

			var Item1Image = new CachedImage()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			Item1Image.SetBinding(CachedImage.SourceProperty, "Item1Icon");

			var Item2Image = new CachedImage()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			Item2Image.SetBinding(CachedImage.SourceProperty, "Item2Icon");

			var Item3Image = new CachedImage()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			Item3Image.SetBinding(CachedImage.SourceProperty, "Item3Icon");

			var Item4Image = new CachedImage()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			Item4Image.SetBinding(CachedImage.SourceProperty, "Item4Icon");

			var Item5Image = new CachedImage()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			Item5Image.SetBinding(CachedImage.SourceProperty, "Item5Icon");

			var Item6Image = new CachedImage()
			{
				HeightRequest = 23.5,
				WidthRequest = 23.5,
				CacheDuration = TimeSpan.FromDays(30),
				DownsampleToViewSize = true,
				RetryCount = 5,
				RetryDelay = 250
			};
			Item6Image.SetBinding(CachedImage.SourceProperty, "Item6Icon");

			var ratingStack = new StackLayout()
			{
				Spacing = 3,
				Orientation = StackOrientation.Horizontal,
				Children = { summonerSpell1Image, summonerSpell2Image, Item0Image, Item1Image, Item2Image, Item3Image, Item4Image, Item5Image, Item6Image }
			};

			var goldLayout = new StackLayout
			{
				Spacing = 0,
				Orientation = StackOrientation.Horizontal,
				Children = {
					goldIcon, GoldLabel
				}
			};

			var minionLayout = new StackLayout
			{
				Spacing = 0,
				Orientation = StackOrientation.Horizontal,
				Children = {
					minionIcon, CSLabel
				}
			};

			var rightLayout = new StackLayout
			{
				Spacing = 0,
				Children = {
					goldLayout,
					minionLayout
				}
			};

			var vetDetailsLayout = new StackLayout
			{
				Padding = new Thickness(10, 0, 0, 0),
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { nameLabel, distanceLabel }
			};

			var champLayout = new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children = {
					champProfileImage, vetDetailsLayout, rightLayout
				}
			};

			var cellLayout = new StackLayout
			{
				Padding = new Thickness(10, 5, 10, 5),
				//Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = { champLayout, ratingStack }
			};

			this.View = cellLayout;
		}
	}
}

