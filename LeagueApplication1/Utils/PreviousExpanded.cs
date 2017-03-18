using RiotSharp.GameEndpoint;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LeagueApplication1
{
	public class PreviousExpanded
	{
		public string Name { get; set; }
		public int Level { get; set; }
		public string Title { get; set; }
		public UriImageSource Icon { get; set; }
		public bool Result { get; set; }
		public string GoldEarned { get; set; }
		public string CreepScore { get; set; }
		public UriImageSource SummonerSpell1Icon { get; set; }
		public UriImageSource SummonerSpell2Icon { get; set; }
		public UriImageSource Item0Icon { get; set; }
		public UriImageSource Item1Icon { get; set; }
		public UriImageSource Item2Icon { get; set; }
		public UriImageSource Item3Icon { get; set; }
		public UriImageSource Item4Icon { get; set; }
		public UriImageSource Item5Icon { get; set; }
		public UriImageSource Item6Icon { get; set; }
		public Game ThisGame { get; set; }

		public bool Victory
		{
			get
			{
				return Result == true;
			}
		}
		public bool Loss
		{
			get
			{
				return Result == false;
			}
		}

		public PreviousExpanded(string champPlayed, int level, bool result, string gold, string creepScore, List<UriImageSource> itemNameList,
			List<UriImageSource> summonerSpellList, UriImageSource imagePath, string gameMode = "", Game thisGame = null)
		{
			ThisGame = thisGame;
			Name = champPlayed;
			Level = level;
			Title = gameMode;
			GoldEarned = gold;
			CreepScore = creepScore;
			Icon = imagePath;
			Result = result;
			SummonerSpell1Icon = summonerSpellList[0];
			SummonerSpell2Icon = summonerSpellList[1];
			Item0Icon = itemNameList[0];
			Item1Icon = itemNameList[1];
			Item2Icon = itemNameList[2];
			Item3Icon = itemNameList[3];
			Item4Icon = itemNameList[4];
			Item5Icon = itemNameList[5];
			Item6Icon = itemNameList[6];
		}
	}
}

