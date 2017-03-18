using System;
namespace LeagueApplication1
{
	public interface ISaveAndLoad
	{
		void SaveText(string filename, string text);
		string LoadText(string filename);
		bool FileExists(string filename);
	}
}
