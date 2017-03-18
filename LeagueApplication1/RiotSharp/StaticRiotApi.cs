using Newtonsoft.Json;
using RiotSharp.StaticDataEndpoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Util;
using LeagueApplication1;
using System.IO;
using System.Diagnostics;

namespace RiotSharp
{
	/// <summary>
	/// Entry point for the static API.
	/// </summary>
	public class StaticRiotApi : IStaticRiotApi
	{
		private const string ChampionsCacheKey = "champions";
		private const string ChampionCacheKey = "champion";
		private const string ItemsCacheKey = "items";
		private const string ItemCacheKey = "item";
		private const string MasteriesCacheKey = "masteries";
		private const string MasteryCacheKey = "mastery";
		private const string RunesCacheKey = "runes";
		private const string RuneCacheKey = "rune";
		private const string SummonerSpellsCacheKey = "spells";
		private const string SummonerSpellCacheKey = "spell";
		private const string VersionCacheKey = "versions";

		private Cache cache;
		private readonly TimeSpan DefaultSlidingExpiry = new TimeSpan(0, 30, 0);

		public StaticRiotApi()
		{
			cache = new Cache();
		}

		/// <summary>
		/// Get a list of all champions synchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="championData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>A ChampionListStatic object containing all champions.</returns>
		public ChampionListStatic GetChampions(Region region, List<ChampionData> championData,
			Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in championData)
			{
				
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetChampion",
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
			var champs = JsonConvert.DeserializeObject<ChampionListStatic>(json);
			return champs;
		}

		/// <summary>
		/// Get a list of all champions asynchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="championData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>A ChampionListStatic object containing all champions.</returns>
		public async Task<ChampionListStatic> GetChampionsAsync(Region region,
			List<ChampionData> championData, Language language = Language.en_US)
		{
			var stringList = new List<string> { };
			foreach (var data in championData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetChampion",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
			var champs = await Task.Factory.StartNew(() =>
				JsonConvert.DeserializeObject<ChampionListStatic>(json));
			return champs;
		}

		/// <summary>
		/// Get a champion synchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="championId">Id of the champion to retrieve.</param>
		/// <param name="championData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>A champion.</returns>
		public ChampionStatic GetChampion(Region region, int championId,
			List<ChampionData> championData, Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in championData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var json = "";
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\", \"id\" : \"" + championId + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetChampion",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var test = Task.Run(async () =>
			{
				var resp = await App.lambdaClient.InvokeAsync(ir);

				var sr = new StreamReader(resp.Payload);

				json = sr.ReadToEnd();
			});
			test.Wait();
			var champ = JsonConvert.DeserializeObject<ChampionStatic>(json);
			return champ;
		}

		/// <summary>
		/// Get a champion asynchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="championId">Id of the champion to retrieve.</param>
		/// <param name="championData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>A champion.</returns>
		public async Task<ChampionStatic> GetChampionAsync(Region region, int championId,
			List<ChampionData> championData, Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in championData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\", \"id\" : \"" + championId + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetChampion",
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
			var champ = await Task.Factory.StartNew(() =>
				JsonConvert.DeserializeObject<ChampionStatic>(json));
			return champ;
		}

		/// <summary>
		/// Get a list of all items synchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="itemData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>An ItemListStatic object containing all items.</returns>
		public ItemListStatic GetItems(Region region, List<ItemData> itemData,
			Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in itemData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetItem",
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
			var items = JsonConvert.DeserializeObject<ItemListStatic>(json);
			return items;
		}

		/// <summary>
		/// Get a list of all items synchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="itemData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>An ItemListStatic object containing all items.</returns>
		public async Task<ItemListStatic> GetItemsAsync(Region region, List<ItemData> itemData,
			Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in itemData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetItem",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
			var items = await Task.Factory.StartNew(() =>
				JsonConvert.DeserializeObject<ItemListStatic>(json));
			return items;
		}

		/// <summary>
		/// Get an item synchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="itemId">Id of the item to retrieve.</param>
		/// <param name="itemData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>An item.</returns>
		public ItemStatic GetItem(Region region, int itemId, List<ItemData> itemData,
			Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in itemData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + itemId + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetItem",
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
			var item = JsonConvert.DeserializeObject<ItemStatic>(json);
			return item;
		}
	

        /// <summary>
        /// Get an item asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="itemId">Id of the item to retrieve.</param>
        /// <param name="itemData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>An item.</returns>
        public async Task<ItemStatic> GetItemAsync(Region region, int itemId, List<ItemData> itemData,
            Language language = Language.en_US)
        {
            var stringList = new List<string>();
			foreach (var data in itemData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
             var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + itemId + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetItem",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
				var resp = await App.lambdaClient.InvokeAsync(ir);

				var sr = new StreamReader(resp.Payload);

				json = sr.ReadToEnd();
            var item = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<ItemStatic>(json));
            return item;
        }

        /// <summary>
        /// Retrieve language strings synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <param name="version">Version of the dragon API.</param>
        /// <returns>A object containing the language strings.</returns>
        /*public LanguageStringsStatic GetLanguageStrings(Region region, Language language = Language.en_US,
            string version = "5.3.1")
        {
            var wrapper = cache.Get<string, LanguageStringsStaticWrapper>(LanguageStringsCacheKey);
            if (wrapper != null && wrapper.Language == language && wrapper.Version == version)
            {
                return wrapper.LanguageStringsStatic;
            }

            var json = requester.CreateGetRequest(string.Format(LanguageStringsRootUrl, region.ToString()), RootDomain,
                new List<string> {
                    string.Format("locale={0}", language.ToString()),
                    string.Format("version={0}", version)
                });
            var languageStrings = JsonConvert.DeserializeObject<LanguageStringsStatic>(json);

            cache.Add(LanguageStringsCacheKey, new LanguageStringsStaticWrapper(languageStrings,
                language, version), DefaultSlidingExpiry);

            return languageStrings;
        }

        /// <summary>
        /// Retrieve language strings asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <param name="version">Version of the dragon API.</param>
        /// <returns>A object containing the language strings.</returns>
        public async Task<LanguageStringsStatic> GetLanguageStringsAsync(Region region,
            Language language = Language.en_US, string version = "5.3.1")
        {
            var wrapper = cache.Get<string, LanguageStringsStaticWrapper>(LanguageStringsCacheKey);
            if (wrapper != null && wrapper.Language == language && wrapper.Version == version)
            {
                return wrapper.LanguageStringsStatic;
            }

            var json = await requester.CreateGetRequestAsync(string.Format(LanguageStringsRootUrl, region.ToString()),
                RootDomain, new List<string> {
                    string.Format("locale={0}", language.ToString()),
                    string.Format("version={0}", version)
                });
            var languageStrings = await Task.Factory.StartNew(() 
                => JsonConvert.DeserializeObject<LanguageStringsStatic>(json));

            cache.Add(LanguageStringsCacheKey, new LanguageStringsStaticWrapper(languageStrings,
                language, version), DefaultSlidingExpiry);

            return languageStrings;
        }

        /// <summary>
        /// Get languages synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <returns>A list of languages.</returns>
        public List<Language> GetLanguages(Region region)
        {
            var wrapper = cache.Get<string, List<Language>>(LanguagesCacheKey);
            if (wrapper != null)
            {
                return wrapper;
            }

            var json = requester.CreateGetRequest(string.Format(LanguagesRootUrl, region.ToString()), RootDomain);
            var languages = JsonConvert.DeserializeObject<List<Language>>(json);

            cache.Add(LanguagesCacheKey, languages, DefaultSlidingExpiry);

            return languages;
        }

        /// <summary>
        /// Get languages asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <returns>A list of languages.</returns>
        public async Task<List<Language>> GetLanguagesAsync(Region region)
        {
            var wrapper = cache.Get<string, List<Language>>(LanguagesCacheKey);
            if (wrapper != null)
            {
                return wrapper;
            }

            var json = await requester.CreateGetRequestAsync(string.Format(LanguagesRootUrl, region.ToString()),
                RootDomain);
            var languages = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<List<Language>>(json));

            cache.Add(LanguagesCacheKey, languages, DefaultSlidingExpiry);

            return languages;
        }

        /// <summary>
        /// Get maps synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <param name="version">Version of the dragon API.</param>
        /// <returns>A list of objects representing maps.</returns>
        public List<MapStatic> GetMaps(Region region, Language language = Language.en_US, string version = "5.3.1")
        {
            var wrapper = cache.Get<string, MapsStaticWrapper>(MapCacheKey);
            if (wrapper != null && wrapper.Language == language && wrapper.Version == version)
            {
                return wrapper.MapsStatic.Data.Values.ToList();
            }

            var json = requester.CreateGetRequest(string.Format(MapRootUrl, region.ToString()), RootDomain,
                new List<string> {
                    string.Format("locale={0}", language.ToString()),
                    string.Format("version={0}", version)
                });
            var maps = JsonConvert.DeserializeObject<MapsStatic>(json);

            cache.Add(MapCacheKey, new MapsStaticWrapper(maps, language, version), DefaultSlidingExpiry);

            return maps.Data.Values.ToList();
        }

        /// <summary>
        /// Get maps asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <param name="version">Version of the dragon API.</param>
        /// <returns>A list of objects representing maps.</returns>
        public async Task<List<MapStatic>> GetMapsAsync(Region region, Language language = Language.en_US,
            string version = "5.3.1")
        {
            var wrapper = cache.Get<string, MapsStaticWrapper>(MapCacheKey);
            if (wrapper != null && wrapper.Language == language && wrapper.Version == version)
            {
                return wrapper.MapsStatic.Data.Values.ToList();
            }

            var json = await requester.CreateGetRequestAsync(string.Format(MapRootUrl, region.ToString()), RootDomain,
                new List<string> {
                    string.Format("locale={0}", language.ToString()),
                    string.Format("version={0}", version)
                });
            var maps = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<MapsStatic>(json));

            cache.Add(MapCacheKey, new MapsStaticWrapper(maps, language, version), DefaultSlidingExpiry);

            return maps.Data.Values.ToList();
        }*/

        /// <summary>
        /// Get a list of all masteries synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="masteryData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>An MasteryListStatic object containing all masteries.</returns>
        public MasteryListStatic GetMasteries(Region region, MasteryData masteryData = MasteryData.basic,
            Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, MasteryListStaticWrapper>(MasteriesCacheKey);
            if (wrapper == null || language != wrapper.Language || masteryData != wrapper.MasteryData)
            {
                var args = "{\"data\" : \"" + masteryData.ToString() + "\", \"region\" : \"" + region.ToString() + "\"}";
				var ir = new Amazon.Lambda.Model.InvokeRequest
				{
					FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetMastery",
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
                var masteries = JsonConvert.DeserializeObject<MasteryListStatic>(json);
                wrapper = new MasteryListStaticWrapper(masteries, language, masteryData);
                cache.Add(MasteriesCacheKey, wrapper, DefaultSlidingExpiry);
            }
            return wrapper.MasteryListStatic;
        }

        /// <summary>
        /// Get a list of all masteries asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="masteryData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>An MasteryListStatic object containing all masteries.</returns>
        public async Task<MasteryListStatic> GetMasteriesAsync(Region region,
            MasteryData masteryData = MasteryData.basic, Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, MasteryListStaticWrapper>(MasteriesCacheKey);
            if (wrapper != null && language == wrapper.Language && masteryData == wrapper.MasteryData)
            {
                return wrapper.MasteryListStatic;
            }
            var args = "{\"data\" : \"" + masteryData.ToString() + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetMastery",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
				var resp = await App.lambdaClient.InvokeAsync(ir);

				var sr = new StreamReader(resp.Payload);

				json = sr.ReadToEnd();
            var masteries = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<MasteryListStatic>(json));
            wrapper = new MasteryListStaticWrapper(masteries, language, masteryData);
            cache.Add(MasteriesCacheKey, wrapper, DefaultSlidingExpiry);
            return wrapper.MasteryListStatic;
        }

        /// <summary>
        /// Get a mastery synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="masteryId">Id of the mastery to retrieve.</param>
        /// <param name="masteryData">Data to retrieve.</param>
        /// <param name="language">Language of th data to be retrieved.</param>
        /// <returns>A mastery.</returns>
        public MasteryStatic GetMastery(Region region, int masteryId, MasteryData masteryData = MasteryData.basic,
            Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, MasteryStaticWrapper>(MasteryCacheKey + masteryId);
            if (wrapper != null && wrapper.Language == language && wrapper.MasteryData == masteryData)
            {
                return wrapper.MasteryStatic;
            }
            else
            {
                var listWrapper = cache.Get<string, MasteryListStaticWrapper>(MasteriesCacheKey);
                if (listWrapper != null && listWrapper.Language == language && listWrapper.MasteryData == masteryData)
                {
                    if (listWrapper.MasteryListStatic.Masteries.ContainsKey(masteryId))
                    {
                        return listWrapper.MasteryListStatic.Masteries[masteryId];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var args = "{\"data\" : \"" + masteryData.ToString() + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + masteryId + "\"}";
					var ir = new Amazon.Lambda.Model.InvokeRequest
					{
						FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetMastery",
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
                    var mastery = JsonConvert.DeserializeObject<MasteryStatic>(json);
                    cache.Add(MasteryCacheKey + masteryId, new MasteryStaticWrapper(mastery, language, masteryData),
                        DefaultSlidingExpiry);
                    return mastery;
                }
            }
        }

        /// <summary>
        /// Get a mastery asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="masteryId">Id of the mastery to retrieve.</param>
        /// <param name="masteryData">Data to retrieve.</param>
        /// <param name="language">Language of th data to be retrieved.</param>
        /// <returns>A mastery.</returns>
        public async Task<MasteryStatic> GetMasteryAsync(Region region, int masteryId,
            MasteryData masteryData = MasteryData.basic, Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, MasteryStaticWrapper>(MasteryCacheKey + masteryId);
            if (wrapper != null && wrapper.Language == language && wrapper.MasteryData == masteryData)
            {
                return wrapper.MasteryStatic;
            }
            var listWrapper = cache.Get<string, MasteryListStaticWrapper>(MasteriesCacheKey);
            if (listWrapper != null && listWrapper.Language == language && listWrapper.MasteryData == masteryData)
            {
                return listWrapper.MasteryListStatic.Masteries.ContainsKey(masteryId) ? listWrapper.MasteryListStatic.Masteries[masteryId] : null;
            }
            var args = "{\"data\" : \"" + masteryData.ToString() + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + masteryId + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetMastery",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
				var resp = await App.lambdaClient.InvokeAsync(ir);

				var sr = new StreamReader(resp.Payload);

				json = sr.ReadToEnd();
            var mastery = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<MasteryStatic>(json));
            cache.Add(MasteryCacheKey + masteryId, new MasteryStaticWrapper(mastery, language, masteryData),
                DefaultSlidingExpiry);
            return mastery;
        }

        /// <summary>
        /// Retrieve realm data synchronously.
        /// </summary>
        /// <param name="region">Region corresponding to data to retrieve.</param>
        /// <returns>A realm object containing the requested information.</returns>
        /*public RealmStatic GetRealm(Region region)
        {
            var wrapper = cache.Get<string, RealmStaticWrapper>(RealmCacheKey);
            if (wrapper != null)
            {
                return wrapper.RealmStatic;
            }

            var json = requester.CreateGetRequest(string.Format(RealmRootUrl, region.ToString()), RootDomain);
            var realm = JsonConvert.DeserializeObject<RealmStatic>(json);

            cache.Add(RealmCacheKey, new RealmStaticWrapper(realm), DefaultSlidingExpiry);

            return realm;
        }

        /// <summary>
        /// Retrieve realm data asynchronously.
        /// </summary>
        /// <param name="region">Region corresponding to data to retrieve.</param>
        /// <returns>A realm object containing the requested information.</returns>
        public async Task<RealmStatic> GetRealmAsync(Region region)
        {
            var wrapper = cache.Get<string, RealmStaticWrapper>(RealmCacheKey);
            if (wrapper != null)
            {
                return wrapper.RealmStatic;
            }

            var json = await requester.CreateGetRequestAsync(string.Format(RealmRootUrl, region.ToString()), RootDomain);
            var realm = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<RealmStatic>(json));

            cache.Add(RealmCacheKey, new RealmStaticWrapper(realm), DefaultSlidingExpiry);

            return realm;
        }*/

        /// <summary>
        /// Get a list of all runes synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="runeData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>A RuneListStatic object containing all runes.</returns>
        public RuneListStatic GetRunes(Region region, RuneData runeData = RuneData.basic
            , Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, RuneListStaticWrapper>(RunesCacheKey);
            if (wrapper == null || language != wrapper.Language || runeData != wrapper.RuneData)
            {
                var args = "{\"data\" : \"" + runeData.ToString() + "\", \"region\" : \"" + region.ToString() + "\"}";
				var ir = new Amazon.Lambda.Model.InvokeRequest
				{
					FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetRune",
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
                var runes = JsonConvert.DeserializeObject<RuneListStatic>(json);
                wrapper = new RuneListStaticWrapper(runes, language, runeData);
                cache.Add(RunesCacheKey, wrapper, DefaultSlidingExpiry);
            }
            return wrapper.RuneListStatic;
        }

        /// <summary>
        /// Get a list of all runes asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="runeData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>A RuneListStatic object containing all runes.</returns>
        public async Task<RuneListStatic> GetRunesAsync(Region region, RuneData runeData = RuneData.basic,
            Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, RuneListStaticWrapper>(RunesCacheKey);
            if (wrapper != null && !(language != wrapper.Language | runeData != wrapper.RuneData))
            {
                return wrapper.RuneListStatic;
            }
            var args = "{\"data\" : \"" + runeData.ToString() + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetRune",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
				var resp = await App.lambdaClient.InvokeAsync(ir);

				var sr = new StreamReader(resp.Payload);

				json = sr.ReadToEnd();
            var runes = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RuneListStatic>(json));
            wrapper = new RuneListStaticWrapper(runes, language, runeData);
            cache.Add(RunesCacheKey, wrapper, DefaultSlidingExpiry);
            return wrapper.RuneListStatic;
        }

        /// <summary>
        /// Get a rune synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="runeId">Id of the rune to retrieve.</param>
        /// <param name="runeData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>A rune.</returns>
        public RuneStatic GetRune(Region region, int runeId, RuneData runeData = RuneData.basic,
            Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, RuneStaticWrapper>(RuneCacheKey + runeId);
            if (wrapper != null && wrapper.Language == language && wrapper.RuneData == RuneData.all)
            {
                return wrapper.RuneStatic;
            }
            else
            {
                var listWrapper = cache.Get<string, RuneListStaticWrapper>(RunesCacheKey);
                if (listWrapper != null && listWrapper.Language == language && listWrapper.RuneData == runeData)
                {
                    if (listWrapper.RuneListStatic.Runes.ContainsKey(runeId))
                    {
                        return listWrapper.RuneListStatic.Runes[runeId];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    var args = "{\"data\" : \"" + runeData.ToString() + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + runeId + "\"}";
					var ir = new Amazon.Lambda.Model.InvokeRequest
					{
						FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetRune",
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
                    var rune = JsonConvert.DeserializeObject<RuneStatic>(json);
                    cache.Add(RuneCacheKey + runeId, new RuneStaticWrapper(rune, language, runeData),
                        DefaultSlidingExpiry);
                    return rune;
                }
            }
        }

        /// <summary>
        /// Get a rune asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="runeId">Id of the rune to retrieve.</param>
        /// <param name="runeData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>A rune.</returns>
        public async Task<RuneStatic> GetRuneAsync(Region region, int runeId, RuneData runeData = RuneData.basic,
            Language language = Language.en_US)
        {
            var wrapper = cache.Get<string, RuneStaticWrapper>(RuneCacheKey + runeId);
            if (wrapper != null && wrapper.Language == language && wrapper.RuneData == RuneData.all)
            {
                return wrapper.RuneStatic;
            }
            var listWrapper = cache.Get<string, RuneListStaticWrapper>(RunesCacheKey);
            if (listWrapper != null && listWrapper.Language == language && listWrapper.RuneData == runeData)
            {
                return listWrapper.RuneListStatic.Runes.ContainsKey(runeId) ?
                    listWrapper.RuneListStatic.Runes[runeId] : null;
            }
            var args = "{\"data\" : \"" + runeData.ToString() + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + runeId + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetRune",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
				var resp = await App.lambdaClient.InvokeAsync(ir);

				var sr = new StreamReader(resp.Payload);

				json = sr.ReadToEnd();
            var rune = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<RuneStatic>(json));
            cache.Add(RuneCacheKey + runeId, new RuneStaticWrapper(rune, language, runeData), DefaultSlidingExpiry);
            return rune;
        }

		/// <summary>
		/// Get a list of all summoner spells synchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="summonerSpellData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>A SummonerSpellListStatic object containing all summoner spells.</returns>
		public SummonerSpellListStatic GetSummonerSpells(Region region,
			List<SummonerSpellData> summonerSpellData, Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in summonerSpellData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetSummonerSpell",
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
			var spells = JsonConvert.DeserializeObject<SummonerSpellListStatic>(json);
			return spells;
		}

        /// <summary>
        /// Get a list of all summoner spells asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve the data.</param>
        /// <param name="summonerSpellData">Data to retrieve.</param>
        /// <param name="language">Language of the data to be retrieved.</param>
        /// <returns>A SummonerSpellListStatic object containing all summoner spells.</returns>
        public async Task<SummonerSpellListStatic> GetSummonerSpellsAsync(Region region,
            List<SummonerSpellData> summonerSpellData, Language language = Language.en_US)
        {
            var stringList = new List<string>();
			foreach (var data in summonerSpellData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
            var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetSummonerSpell",
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
            var spells = await Task.Factory.StartNew(() =>
                JsonConvert.DeserializeObject<SummonerSpellListStatic>(json));
            return spells;
        }

		/// <summary>
		/// Get a summoner spell synchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="summonerSpell">Summoner spell to retrieve.</param>
		/// <param name="summonerSpellData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>A summoner spell.</returns>
		public SummonerSpellStatic GetSummonerSpell(Region region, SummonerSpell summonerSpell,
			List<SummonerSpellData> summonerSpellData, Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in summonerSpellData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + (int)summonerSpell + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetSummonerSpell",
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
			var spell = JsonConvert.DeserializeObject<SummonerSpellStatic>(json);
			return spell;
		}

		/// <summary>
		/// Get a summoner spell asynchronously.
		/// </summary>
		/// <param name="region">Region from which to retrieve the data.</param>
		/// <param name="summonerSpell">Summoner spell to retrieve.</param>
		/// <param name="summonerSpellData">Data to retrieve.</param>
		/// <param name="language">Language of the data to be retrieved.</param>
		/// <returns>A summoner spell.</returns>
		public async Task<SummonerSpellStatic> GetSummonerSpellAsync(Region region, SummonerSpell summonerSpell,
			List<SummonerSpellData> summonerSpellData, Language language = Language.en_US)
		{
			var stringList = new List<string>();
			foreach (var data in summonerSpellData)
			{
				stringList.Add(data.ToString());
			}
			var champData = Util.BuildNamesString(stringList);
			var args = "{\"data\" : \"" + champData + "\", \"region\" : \"" + region.ToString() + ", \"id\" : \"" + (int)summonerSpell + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetSummonerSpell",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
			var resp = await App.lambdaClient.InvokeAsync(ir);

			var sr = new StreamReader(resp.Payload);

			json = sr.ReadToEnd();
			var spell = await Task.Factory.StartNew(() =>
				JsonConvert.DeserializeObject<SummonerSpellStatic>(json));
			return spell;
		}

        /// <summary>
        /// Retrieve static api version data synchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve data.</param>
        /// <returns>A list of versions as strings.</returns>
        public List<string> GetVersions(Region region)
        {
            var wrapper = cache.Get<string, List<string>>(VersionCacheKey);
            if (wrapper != null)
            {
                return wrapper;
            }

            var args = "{\"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetVersions",
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
            var version = JsonConvert.DeserializeObject<List<string>>(json);

            cache.Add(VersionCacheKey, version, DefaultSlidingExpiry);

            return version;
        }

        /// <summary>
        /// Retrieve static api version data asynchronously.
        /// </summary>
        /// <param name="region">Region from which to retrieve data.</param>
        /// <returns>A list of versions as strings.</returns>
        public async Task<List<string>> GetVersionsAsync(Region region)
        {
            var wrapper = cache.Get<string, List<string>>(VersionCacheKey);
            if (wrapper != null)
            {
                return wrapper;
            }

            var args = "{\"region\" : \"" + region.ToString() + "\"}";
			var ir = new Amazon.Lambda.Model.InvokeRequest
			{
				FunctionName = "arn:aws:lambda:us-east-1:907841483528:function:staticGetVersion",
				PayloadStream = AWSSDKUtils.GenerateMemoryStreamFromString(args)
			};
			var json = "";
				var resp = await App.lambdaClient.InvokeAsync(ir);

				var sr = new StreamReader(resp.Payload);

				json = sr.ReadToEnd();
            var version = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<List<string>>(json));

            cache.Add(VersionCacheKey, version, DefaultSlidingExpiry);

            return version;
        }
    }
}
