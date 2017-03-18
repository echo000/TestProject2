using System;
using RiotSharp;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class CustomSearchBar : SearchBar
	{
		public event ScopeChangedEventHandler ScopeChanged;
		public string SelectedScopeText
		{
			get
			{
				return SelectedButtonIndex >= 0
					&& SelectedButtonIndex < ScopeButtons.Length
					? ScopeButtons[SelectedButtonIndex]
					: "";
			}
		}

		public int SelectedButtonIndex { get; set; }


		public string[] ScopeButtons
		{
			get
			{
				return new[] {
				App.Constants.NA,
				App.Constants.KR,
				App.Constants.BR,
				App.Constants.EUW,
				App.Constants.EUNE,
				App.Constants.LAN,
				App.Constants.LAS,
				App.Constants.OCE,
				App.Constants.RU,
				App.Constants.TR,
				App.Constants.JP
				};
			}
		}

		public void RaiseScopeChanged()
		{
			if (ScopeChanged != null)
				ScopeChanged.Invoke(this, EventArgs.Empty);
		}
	}

	public delegate void ScopeChangedEventHandler(
		object sender, EventArgs e);


	public enum ReturnKeyTypes : int
	{
		Default,
		Go,
		Google,
		Join,
		Next,
		Route,
		Search,
		Send,
		Yahoo,
		Done,
		EmergenyCall,
		Continue
	}
}

