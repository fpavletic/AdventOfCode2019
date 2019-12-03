using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task3
{
	public class CrossedWires
	{
		public static void MainTask3()
		{
			var reader = File.OpenText(Util.INPUT_DIR + "task3.txt");
			var line1 = reader.ReadLine().Split(",").Select(s => (s[0], int.Parse(s.Substring(1))));
			var line2 = reader.ReadLine().Split(",").Select(s => (s[0], int.Parse(s.Substring(1))));

			IDictionary<(int x, int y), int> positionToLine = new Dictionary<(int x, int y), int>();
			int stepCount = 1;

			ForEachStep(line1, (x, y) => {
				if (!positionToLine.ContainsKey((x, y)))
				{
					positionToLine[(x, y)] = stepCount;
				}
				stepCount++;
			});

			//Task one
			var minDistance = int.MaxValue;
			ForEachStep(line2, (x, y) => {
				if (positionToLine.ContainsKey((x, y)))
				{
					var distance = Math.Abs(x) + Math.Abs(y);
					minDistance = minDistance > distance ? distance : minDistance;
				}
			});
			Console.WriteLine(minDistance);

			//Task two
			stepCount = 1;
			var minStepCount = int.MaxValue;
			ForEachStep(line2, (x, y) =>
			{
				if (positionToLine.TryGetValue((x, y), out var otherStepCount))
				{
					minStepCount = minStepCount > stepCount + otherStepCount ? stepCount + otherStepCount : minStepCount;
				}
				stepCount++;
			});
			Console.WriteLine(minStepCount);
		}

		private static void ForEachStep(IEnumerable<(char direction, int length)> line, Action<int, int> consumer)
		{
			int x = 0;
			int y = 0;
			foreach (var section in line)
			{
				for (int i = 0; i < section.Item2; i++)
				{
					(x, y) = section.Item1 switch
					{
						'U' => (x, y + 1),
						'D' => (x, y - 1),
						'L' => (x - 1, y),
						'R' => (x + 1, y)
					};

					consumer(x, y);
				}
			}
		}
	}
}
