using System;
using CoreGraphics;
using LeagueApplication1;
using LeagueApplication1.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Linq;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
public class CustomSearchBarRenderer : SearchBarRenderer
{
	protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
	{
		base.OnElementChanged(e);

		var element = Element as CustomSearchBar;

		if (element != null)
		{
			Control.ShowsScopeBar = true;
			Control.ShowsCancelButton = true;
			Control.BackgroundColor = UIColor.White;
			Control.ScopeButtonTitles = element.ScopeButtons;

			var sub = Control.Subviews[0].Subviews[0].OfType<UISegmentedControl>();
			var segment = sub;
				segment.ElementAt(0).SetWidth(25f, 0);
				segment.ElementAt(0).SetWidth(25f, 1);
				segment.ElementAt(0).SetWidth(25f, 2);
				segment.ElementAt(0).SetWidth(35f, 3);
				segment.ElementAt(0).SetWidth(35f, 4);
				segment.ElementAt(0).SetWidth(35f, 5);
				segment.ElementAt(0).SetWidth(35f, 6);
				segment.ElementAt(0).SetWidth(35f, 7);
				segment.ElementAt(0).SetWidth(25f, 8);
				segment.ElementAt(0).SetWidth(25f, 9);
				segment.ElementAt(0).SetWidth(25f, 10);
			sub.ElementAt(0).Frame = new CGRect(20, 2.5f, UIScreen.MainScreen.Bounds.Width, 30);

			Control.SelectedScopeButtonIndexChanged +=
			(sender, args) =>
			{
				element.SelectedButtonIndex = (int)args.SelectedScope;
				element.RaiseScopeChanged();
			};

			Control.CancelButtonClicked +=
					(sender, args) => Control.ResignFirstResponder();
		}

		var toolbar = new UIToolbar(new CGRect(0.0f,0.0f,Control.Frame.Size.Width,44.0f));

		toolbar.Items = new[]
		{
			new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
			new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { Control.ResignFirstResponder(); })
		};

		this.Control.InputAccessoryView = toolbar;
	}
}

