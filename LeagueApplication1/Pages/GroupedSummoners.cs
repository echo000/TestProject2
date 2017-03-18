using System.Collections.ObjectModel;

namespace LeagueApplication1
{
	class GroupedSummoners : ObservableCollection<CurrentGamePlayers>
	{
		public string LongName { get; set; }
		public string ShortName { get; set; }
	}
}