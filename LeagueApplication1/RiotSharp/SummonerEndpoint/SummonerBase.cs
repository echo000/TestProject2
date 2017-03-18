using Newtonsoft.Json;
using RiotSharp.GameEndpoint;
using RiotSharp.LeagueEndpoint;
using RiotSharp.StatsEndpoint;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Util;
using System.IO;
using LeagueApplication1;

namespace RiotSharp.SummonerEndpoint
{
    /// <summary>
    /// Class representing the name and id of a Summoner in the API.
    /// </summary>
    public class SummonerBase
    {
        private const string RootUrl = "/api/lol/{0}/v1.4/summoner";
        private const string MasteriesUrl = "/{0}/masteries";
        private const string RunesUrl = "/{0}/runes";

        private const string GameRootUrl = "/api/lol/{0}/v1.3/game";
        private const string RecentGamesUrl = "/by-summoner/{0}/recent";

        private const string LeagueRootUrl = "/api/lol/{0}/v2.5/league";
        private const string LeagueBySummonerUrl = "/by-summoner/{0}";
        private const string LeagueBySummonerEntryUrl = "/entry";

        private const string StatsRootUrl = "/api/lol/{0}/v1.3/stats";
        private const string StatsSummaryUrl = "/by-summoner/{0}/summary";
        private const string StatsRankedUrl = "/by-summoner/{0}/ranked";

        private const string TeamRootUrl = "/api/lol/{0}/v2.4/team";
        private const string TeamBySummonerUrl = "/by-summoner/{0}";

        private const string IdUrl = "/{0}";

        private RateLimitedRequester requester;
        public Region Region { get; set; }

        internal SummonerBase()
        {
            requester = Requesters.RiotApiRequester;
        }

        //summoner base not default constructor
        internal SummonerBase(string id, string name, RateLimitedRequester requester, Region region)
        {
            this.requester = requester;
            Region = region;
            Name = name;
            Id = long.Parse(id);
        }

        /// <summary>
        /// Summoner ID.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Summoner name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Get rune pages for this summoner synchronously.
        /// </summary>
        /// <returns>A list of rune pages.</returns>
        public List<RunePage> GetRunePages()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getRunes",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";

			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json).Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get rune pages for this summoner asynchronously.
        /// </summary>
        /// <returns>A list of rune pages.</returns>
        public async Task<List<RunePage>> GetRunePagesAsync()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getRunes",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<Dictionary<string, RunePages>>(json))).Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get mastery pages for this summoner synchronously.
        /// </summary>
        /// <returns>A list of mastery pages.</returns>
        public List<MasteryPage> GetMasteryPages()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getMasteries",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";

			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<Dictionary<long, MasteryPages>>(json)
                .Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get mastery pages for this summoner asynchronously.
        /// </summary>
        /// <returns>A list of mastery pages.</returns>
        public async Task<List<MasteryPage>> GetMasteryPagesAsync()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getMasteries",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<Dictionary<long, MasteryPages>>(json))).Values.FirstOrDefault().Pages;
        }

        /// <summary>
        /// Get the 10 most recent games for this summoner synchronously.
        /// </summary>
        /// <returns>A list of the 10 most recent games.</returns>
        public List<Game> GetRecentGames()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getGame",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";

			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<RecentGames>(json).Games;
        }

        /// <summary>
        /// Get the 10 most recent games for this summoner asynchronously.
        /// </summary>
        /// <returns>A list of the 10 most recent games.</returns>
        public async Task<List<Game>> GetRecentGamesAsync()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getGame",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RecentGames>(json))).Games;
        }

        /// <summary>
        /// Retrieve the league items for this specific summoner and not the entire league.
        /// </summary>
        /// <returns>A list of league items for each league the summoner is in.</returns>
        public List<League> GetLeagues()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getLeague",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json)[Id];
        }

        /// <summary>
        /// Retrieve the league items for this specific summoner and not the entire league asynchronously.
        /// </summary>
        /// <returns>A list of league items for each league the summoner is in.</returns>
        public async Task<List<League>> GetLeaguesAsync()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getLeague",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json)))[Id];
        }

        /// <summary>
        /// Retrieves leagues data for this summoner, including leagues for all of this summoner's teams synchronously.
        /// </summary>
        /// <returns>List of leagues.</returns>
        public List<League> GetEntireLeagues()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getEntireLeague",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json)[Id];
        }

        /// <summary>
        /// Retrieves leagues data for this summoner, including leagues for all of this summoner's
        /// teams asynchronously.
        /// </summary>
        /// <returns>List of leagues.</returns>
        public async Task<List<League>> GetEntireLeaguesAsync()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getEntireLeague",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<Dictionary<long, List<League>>>(json)))[Id];
        }

        /// <summary>
        /// Get player stats summaries for this summoner synchronously, for the current season.
        /// One summary is returned per queue type.
        /// </summary>
        /// <returns>A list of player stats summaries.</returns>
        public List<PlayerStatsSummary> GetStatsSummaries()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getStatSummary",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json).PlayerStatSummaries;
        }

        /// <summary>
        /// Get player stats summaries for this summoner synchronously. One summary is returned per queue type.
        /// </summary>
        /// <param name="season">Season for which you want the stats.</param>
        /// <returns>A list of player stats summaries.</returns>
        public List<PlayerStatsSummary> GetStatsSummaries(StatsEndpoint.Season season)
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + ", \"season\" : \"" + season + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getStatSummary",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json).PlayerStatSummaries;
        }

        /// <summary>
        /// Get player stats summaries for this summoner asynchronously, for the current season.
        /// One summary is returned per queue type.
        /// </summary>
        /// <returns>A list of player stats summaries.</returns>
        public async Task<List<PlayerStatsSummary>> GetStatsSummariesAsync()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getStatSummary",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json))).PlayerStatSummaries;
        }

        /// <summary>
        /// Get player stats summaries for this summoner asynchronously. One summary is returned per queue type.
        /// </summary>
        /// <param name="season">Season for which you want the stats.</param>
        /// <returns>A list of player stats summaries.</returns>
        public async Task<List<PlayerStatsSummary>> GetStatsSummariesAsync(StatsEndpoint.Season season)
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + ", \"season\" : \"" + season + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getStatSummary",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<PlayerStatsSummaryList>(json))).PlayerStatSummaries;
        }

        /// <summary>
        /// Get ranked stats for this summoner synchronously, for the current season.
        /// Includes statistics for Twisted Treeline and Summoner's Rift.
        /// </summary>
        /// <returns>A list of champions stats.</returns>
        public List<ChampionStats> GetStatsRanked()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getRankedStats",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<RankedStats>(json).ChampionStats;
        }

		/// <summary>
		/// Get ranked stats for this summoner synchronously.
		/// Includes statistics for Twisted Treeline and Summoner's Rift.
		/// </summary>
		/// <param name="season">Season for which you want the stats.</param>
		/// <returns>A list of champions stats.</returns>
		public List<ChampionStats> GetStatsRanked(StatsEndpoint.Season season)
		{
			var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + ", \"season\" : \"" + season + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getRankedStats",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var test = Task.Run(async () =>
		   {
			   var resp = await App.lambdaClient.InvokeAsync(ir);

			   var sr = new StreamReader(resp.Payload);

			   json = sr.ReadToEnd();
		   });
			test.Wait();
            return JsonConvert.DeserializeObject<RankedStats>(json).ChampionStats;
        }

        /// <summary>
        /// Get ranked stats for this summoner asynchronously, for the current season.
        /// Includes statistics for Twisted Treeline and Summoner's Rift.
        /// </summary>
        /// <returns>A list of champions stats.</returns>
        public async Task<List<ChampionStats>> GetStatsRankedAsync()
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getRankedStats",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RankedStats>(json))).ChampionStats;
        }

        /// <summary>
        /// Get ranked stats for this summoner asynchronously.
        /// Includes statistics for Twisted Treeline and Summoner's Rift.
        /// </summary>
        /// <param name="season">Season for which you want the stats.</param>
        /// <returns>A list of champions stats.</returns>
        public async Task<List<ChampionStats>> GetStatsRankedAsync(StatsEndpoint.Season season)
        {
            var args = "{\"id\" : \"" + Id + "\"," + " \"region\" : \"" + Region.ToString() + ", \"season\" : \"" + season + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:getRankedStats",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
            return (await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RankedStats>(json))).ChampionStats;
        }
    }
}
