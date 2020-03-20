using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.src.Task10
{
	class MonitoringStation
	{

		public static void MainTask10()
		{
			var asteroids = new HashSet<(int x, int y)>();
			var input = File.ReadAllLines(Util.INPUT_DIR + "task10.txt");
			for (int i = 0; i < input.Length; i++)
			{
				var row = input[i];
				for (int j = 0; j < row.Length; j++) {
					if (row[j] == '#') asteroids.Add((j, i));
				}
			}

			var visibleAsteroidLines = new Dictionary<(double k, double l, bool greater), SimplePriorityQueue<(int x, int y)>>();
			int visibleAsteroidCount;

			int bestVisibleAsteroidCount = int.MinValue;
			Dictionary<(double k, double l, bool greater), SimplePriorityQueue<(int x, int y)>> bestAsteroidVisibleLines = null;

			foreach (var (x0, y0) in asteroids)
			{
				visibleAsteroidLines.Clear();
				visibleAsteroidCount = 0;
				foreach (var (x1, y1) in asteroids)
				{
					if (x1 == x0 && y1 == y0) continue;

					double k, l;
					bool greater;
					if (x1 == x0)
					{
						k = double.MaxValue;
						l = 0;
						greater = y1 < y0;
					} else
					{
						k = (- y0 + y1) / (double)(x0 - x1);
						l = y0 - x0 * k;
						greater = x1 > x0;
					}

					if (!visibleAsteroidLines.ContainsKey((k, l, greater)))
					{
						visibleAsteroidCount++;
						visibleAsteroidLines[(k, l, greater)] = new SimplePriorityQueue<(int x, int y)>();
					}
					visibleAsteroidLines[(k, l, greater)].Enqueue((x1, y1), Math.Abs(x0 - x1) + Math.Abs(y0 - y1));
				}
				if (visibleAsteroidCount > bestVisibleAsteroidCount)
				{
					bestVisibleAsteroidCount = visibleAsteroidCount;
					bestAsteroidVisibleLines = visibleAsteroidLines;
					visibleAsteroidLines = new Dictionary<(double k, double l, bool greater), SimplePriorityQueue<(int x, int y)>>();
				}
			}

			Console.WriteLine(bestVisibleAsteroidCount);

			var keys = bestAsteroidVisibleLines.Keys.ToList();
			keys.Sort((l1, l2) => {
				if (l1.greater == l2.greater) return -l1.k.CompareTo(l2.k);
				if (l1.greater) return -1;
				else return 1;
			});
			int offset = 0;
			for (int i = 0; i < 200 && keys.Any(); i++)
			{
				var key = keys[(i + offset) % keys.Count];
				var line = bestAsteroidVisibleLines[key];
				var asteroid = line.Dequeue();
				if (!line.Any())
				{
					keys.RemoveAt((i + offset--) % keys.Count);
					bestAsteroidVisibleLines.Remove(key);
				}
				Console.WriteLine($"{i}, {key.k}, {key.greater}: {asteroid}");
			}


		}

	}
}
