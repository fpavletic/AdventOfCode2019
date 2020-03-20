using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task14
{
	class SpaceStoichiometry
	{
		private static readonly IDictionary<string, (long amount, ISet<(string type, long amount)>)> typeToMaterials =
			new Dictionary<string, (long amount, ISet<(string type, long amount)>)>();
		private static readonly IDictionary<string, long> leftovers = new Dictionary<string, long>();

		public static void MainTask14()
		{
			foreach (var equation in File.ReadAllLines(Util.INPUT_DIR + "task14.txt"))
			{
				var equationSplit = equation.Split(" => ", 2);
				var equationRight = equationSplit[1].Split(" ");
				typeToMaterials[equationRight[1]] = (long.Parse(equationRight[0]), equationSplit[0].Split(", ").Select(m => m.Split(" ")).Select(m => (m[1], long.Parse(m[0]))).ToHashSet());
			}

			Console.WriteLine(GetOreCost("FUEL", 1));

			long l = 0;
			long r = 1000000000000;
			long m = (l + r) / 2;
			while (r - l != 1)
			{
				var cost = GetOreCost("FUEL", m);
				if ( cost > 1000000000000)
				{
					r = m;
				} else
				{
					l = m;
				}
				m = (l + r) / 2;
			}
			Console.WriteLine(GetOreCost("FUEL", m));
			Console.WriteLine(m);
		}


		private static long GetOreCost(string type, long amount)
		{
			if (type == "ORE") return amount;

			if ( leftovers.TryGetValue(type, out var leftoverAmount)){
				if (leftoverAmount >= amount)
				{
					leftovers[type] = leftoverAmount - amount;
					return 0;
				}
				else
				{
					amount -= leftoverAmount;
					leftovers[type] = 0;
				}
			}

			var (processAmount, process) = typeToMaterials[type];
			var processCount = amount % processAmount == 0 ? amount / processAmount : ( amount / processAmount + 1 );
			var processCost = process.Select((m) => GetOreCost(m.type, m.amount * processCount)).Sum();
			leftovers[type] = (processCount * processAmount) - amount;
			return processCost;
		}

	}
}
