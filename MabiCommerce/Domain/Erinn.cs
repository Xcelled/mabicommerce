using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using MabiCommerce.Domain.Mapping;
using MabiCommerce.Domain.Trading;
using Newtonsoft.Json;
using QuickGraph;
using QuickGraph.Algorithms;

namespace MabiCommerce.Domain
{
	public class Erinn : INotifyPropertyChanged
	{
		public ObservableCollection<Transportation> Transports { get; private set; }
		public ObservableCollection<TradingPost> Posts { get; private set; }
		public ObservableCollection<Trade> Trades { get; private set; }
		public ObservableCollection<Modifier> Modifiers { get; private set; }

		public List<CommerceMasteryRank> CommerceMasteryRanks { get; private set; }
		public List<Region> Regions { get; private set; }
		public List<Portal> Portals { get; private set; }
		public List<MerchantLevel> MerchantLevels { get; private set; }
		public AdjacencyGraph<Waypoint, Connection> World { get; private set; }

		private long _ducats = 1000;
		private long _seasonalducats = 1001;
		public long Ducats
		{
			get { return _ducats; }
			set
			{
				_ducats = value;
				RaisePropertyChanged();
			}
		}
		public long SeasonalDucats
		{
			get { return _seasonalducats; }
			set
			{
				_seasonalducats = value;
				RaisePropertyChanged();
			}
		}

		private CommerceMasteryRank _cmRank;
		public CommerceMasteryRank CmRank
		{
			get { return _cmRank; }
			set
			{
				_cmRank = value;
				RaisePropertyChanged();
			}
		}

		private readonly ConcurrentDictionary<Waypoint, ConcurrentDictionary<Waypoint, Route>> _routeCache =
			new ConcurrentDictionary<Waypoint, ConcurrentDictionary<Waypoint, Route>>();

		public Erinn()
		{
			if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
			{
				if (!Directory.Exists("data"))
					Environment.CurrentDirectory = Path.GetFullPath("MabiCommerce");
				
				Load("data", this);
			}
		}

		public static Erinn Load(string dataDir, Action<double, string> progress = null)
		{
			var e = new Erinn();

			Load(dataDir, e, progress);

			return e;
		}

		private static void Load(string dataDir, Erinn e, Action<double, string> progress = null)
		{
			if (progress == null)
				progress = (d, s) => { };

			var total = 6.0;
			var done = 0;

			progress(done / total, "Loading transports...");
			e.Transports = new ObservableCollection<Transportation>(
				JsonConvert.DeserializeObject<List<Transportation>>(File.ReadAllText(Path.Combine(dataDir, "db/transports.json"))));
			done++;
			progress(done / total, "Loading modifiers...");
			e.Modifiers = new ObservableCollection<Modifier>(
				JsonConvert.DeserializeObject<List<Modifier>>(File.ReadAllText(Path.Combine(dataDir, "db/modifiers.json"))));
			done++;
			progress(done / total, "Loading commerce mastery ranks...");
			e.CommerceMasteryRanks = JsonConvert.DeserializeObject<List<CommerceMasteryRank>>(File.ReadAllText(Path.Combine(dataDir, "db/commerce_mastery_ranks.json")));
			done++;
			progress(done / total, "Loading posts...");
			e.Posts = new ObservableCollection<TradingPost>(
				JsonConvert.DeserializeObject<List<TradingPost>>(File.ReadAllText(Path.Combine(dataDir, "db/posts.json"))));
			done++;

			progress(done / total, "Loading regions...");
			e.Regions = JsonConvert.DeserializeObject<List<Region>>(File.ReadAllText(Path.Combine(dataDir, "db/regions.json")));
			done++;
			progress(done / total, "Loading portals...");
			e.Portals = JsonConvert.DeserializeObject<List<Portal>>(File.ReadAllText(Path.Combine(dataDir, "db/portals.json")));
			done++;
			progress(done / total, "Loading merchant levels...");
			e.MerchantLevels = JsonConvert.DeserializeObject<List<MerchantLevel>>(File.ReadAllText(Path.Combine(dataDir, "db/merchant_levels.json")));
			done++;

			progress(0, "Initializing data...");
			e.Trades = new ObservableCollection<Trade>();
			e.World = new AdjacencyGraph<Waypoint, Connection>();
			e.CmRank = e.CommerceMasteryRanks.First();

			var lowestMerch = e.MerchantLevels.OrderBy(m => m.Level).First();

			foreach (var p in e.Posts)
				p.MerchantLevel = lowestMerch;

			InitializeProfits(e);
			MapWorld(e, progress);
		}

		private static void MapWorld(Erinn e, Action<double, string> progress)
		{
			foreach (var region in e.Regions)
				e.World.AddVerticesAndEdgeRange(region.RegionGraph.Edges);

			foreach (var portal in e.Portals)
			{
				var startRegion = e.Regions.FirstOrDefault(r => r.Id.Equals(portal.StartRegionId, StringComparison.OrdinalIgnoreCase));
				var endRegion = e.Regions.FirstOrDefault(r => r.Id.Equals(portal.EndRegionId, StringComparison.OrdinalIgnoreCase));

				var startWp = startRegion.Waypoints[portal.StartWaypointId.ToLowerInvariant()];
				var endWp = endRegion.Waypoints[portal.EndWaypointId.ToLowerInvariant()];

				e.World.AddEdge(new Connection(startWp, endWp, portal.Time));
			}

			foreach (var p in e.Posts)
			{
				var region = e.Regions.FirstOrDefault(r => r.Id.Equals(p.WaypointRegion, StringComparison.OrdinalIgnoreCase));
				var wp = region.Waypoints[p.WaypointId.ToLowerInvariant()];

				p.Waypoint = wp;
			}

			double total = e.Posts.Count * (e.Posts.Count - 1);
			var done = 0;

			foreach (var p in e.Posts)
			{
				foreach (var p2 in e.Posts)
				{
					if (p2 == p)
						continue;

					progress(done / total, "Caching route data...");
					e.Route(p.Waypoint, p2.Waypoint);
					done++;
				}
			}
		}

		private static void InitializeProfits(Erinn e)
		{
			foreach (var p in e.Posts)
			{
				var otherPosts = e.Posts.Where(x => x != p).ToList();

				foreach (var i in p.Items)
				{
					foreach (var o in otherPosts)
					{
						i.Profits.Add(new Profit(o, 0));
					}
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}

		public Route Route(Waypoint start, Waypoint end)
		{
			if (!_routeCache.ContainsKey(start))
				_routeCache[start] = new ConcurrentDictionary<Waypoint, Route>();

			if (_routeCache[start].ContainsKey(end))
				return _routeCache[start][end];

			return (_routeCache[start][end] = CalculateRoute(start, end));
		}

		private Route CalculateRoute(Waypoint start, Waypoint end)
		{
			IEnumerable<Connection> shortestPathResult;
			if (!(World.ShortestPathsDijkstra(e => e.Time.TotalSeconds, start)(end, out shortestPathResult)))
				throw new Exception("Cannot locate a path from " + start + " to " + end);

			var waypoints = shortestPathResult.ToList();

			return new Route(waypoints);
		}

		private void GetLoads(ConcurrentBag<Load> loads, TradingPost post, Load currentLoad, long currentDucts)
		{
			if (currentLoad.RemainingSlots == 0)
				return;

			var candidates = post.Items.Where(i => i.Status == ItemStatus.Available && !currentLoad.Slots.ContainsKey(i)).ToList();

			Parallel.ForEach(candidates, i => FillLoadWithItem(loads, post, currentLoad, currentDucts, i));
		}

		private void FillLoadWithItem(ConcurrentBag<Load> loads, TradingPost post, Load currentLoad, long currentDucats, Item item)
		{
			var canAdd = (int)(currentDucats / item.Price);

			var added = AddItemToLoad(loads, post, currentLoad, currentDucats, item, canAdd);

			var excess = added % item.QuantityPerSlot;

			if (added < item.QuantityPerSlot || excess == 0)
				return;

			AddItemToLoad(loads, post, currentLoad, currentDucats, item, added - excess);
		}

		private int AddItemToLoad(ConcurrentBag<Load> loads, TradingPost post, Load currentLoad, long currentDucats, Item item, int requested)
		{
			var loadCopy = currentLoad.Copy();

			var added = loadCopy.AddItem(item, Math.Min(item.Stock, requested));

			if (added != 0)
			{
				loads.Add(loadCopy);

				GetLoads(loads, post, loadCopy, currentDucats - item.Price * added);
			}

			return added;
		}

		public ConcurrentBag<Trade> CalculateTrades(TradingPost post)
		{
			var newTrades = new ConcurrentBag<Trade>();

			var s = new System.Diagnostics.Stopwatch();
			s.Start();

			var mods = GetModifierCombinations(Modifiers);

			var cmMod = new Modifier(-1, "Commerce Mastery", 0, 0, 0,
					CmRank.Bonus, CmRank.Bonus, CmRank.Bonus, CmRank.Bonus, new List<int>(), new List<int>());

			foreach (var m in mods)
				m.Add(cmMod);

			mods.Add(new List<Modifier> { cmMod });

			Parallel.ForEach(Transports.Where(t => t.Enabled), t =>
				{
					var allowedMods = mods.Where(combination =>
						!combination.Any(m => 
							m.TransportationBlacklist.Contains(t.Id))
						);

					Parallel.ForEach(allowedMods, m =>
					{
						var baseLoad = new Load(t.Slots + m.Sum(i => i.ExtraSlots),
							t.Weight + m.Sum(i => i.ExtraWeight));

						var loads = new ConcurrentBag<Load>();
						GetLoads(loads, post, baseLoad, SeasonalDucats);

						foreach (var load in loads)
							foreach (var dst in Posts.Where(p => p != post))
							{
								newTrades.Add(new Trade(t, Route(post.Waypoint, dst.Waypoint), load, post, dst, m));
							}
					});
				});

			s.Stop();

			System.Diagnostics.Debug.WriteLine("Calculated {0} possible trades ({1} items, {2} destinations, {3} means of transport, {4} modifier combinations) in {5}", newTrades.Count,
				post.Items.Count(i => i.Status == ItemStatus.Available), Posts.Count - 1, Transports.Count(t => t.Enabled), mods.Count, s.Elapsed);

			return newTrades;
		}

		// Given A, B, C
		// Produces A, AB, B, AC, ABC, BC, C
		private static List<List<Modifier>> GetModifierCombinations(IList<Modifier> modifiers)
		{
			var combinations = new List<List<Modifier>>();

			for (var append = 1; append < modifiers.Count; append++)
			{
				// Add another "base" element
				if (modifiers[append - 1].Enabled)
					combinations.Add(new List<Modifier>(modifiers.Count) { modifiers[append - 1] });

				var toAdd = modifiers[append];
				if (!toAdd.Enabled)
					continue;

				for (int i = 0, count = combinations.Count; i < count; i++)
				{
					var existing = combinations[i];

					// If we'd produce a conflict (illegal state), skip adding it.
					if (existing.Any(m => toAdd.ConflictsWith.Contains(m.Id)))
						continue;

					var newSet = new List<Modifier>(existing);
					newSet.Add(toAdd);
					combinations.Add(newSet);
				}
			}

			// Add the last modifier by itself (since for loop is 1 based)
			if (modifiers.Last().Enabled)
				combinations.Add(new List<Modifier> { modifiers.Last() });

			return combinations;
		}
	}
}
