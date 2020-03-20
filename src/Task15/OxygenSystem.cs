using AdventOfCode2019.src.Task2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.src.Task15
{
	class OxygenSystem
	{
		public static void MainTask15()
		{
			var opcodeComputer = new OpcodeComputer(File.OpenText(Util.INPUT_DIR + "task15.txt").ReadLongs().ToArray());
			for ( int i = 1; true; i++)
			{
				opcodeComputer.RunInstructions();
				var output = DepthLimitedDfs(opcodeComputer, 0, i, 0);
				if ( output != -1 )
				{
					break;
				}
				opcodeComputer.ResetMemory();
			}		
		}

		private static int DepthLimitedDfs(OpcodeComputer opcodeComputer, int j, int depthLimit, int depth)
		{

			if (depth == depthLimit) {
				return -1;
			}

			for ( int i = 1; i < 5; i++)
			{
				if (i == j) continue;
				opcodeComputer.SetInput(i);
				var output = opcodeComputer.GetOutput();

				if (output == 0) continue;

				if (output == 2)
				{
					Console.WriteLine(depthLimit);
					var visitedPositions = new HashSet<(int x, int y)>();
					var lastVisitedPositionCount = -1;
					int l;
					for ( l = depthLimit; lastVisitedPositionCount != visitedPositions.Count; l++)
					{
						lastVisitedPositionCount = visitedPositions.Count;
						Console.WriteLine(lastVisitedPositionCount);
						MapSpace(opcodeComputer, 0, l, 0, (0, 0), visitedPositions);
					}
					Console.WriteLine(l - 2);

					return 1;
				}

				int k = i % 2 == 0 ? i - 1 : (i + 1);
				var dist = DepthLimitedDfs(opcodeComputer, k, depthLimit, depth + 1);
				if ( dist != -1)
				{
					return dist + 1;
				}
				opcodeComputer.SetInput(k);
				if (opcodeComputer.GetOutput() == 0)
				{
					throw new InvalidDataException("WAT");
				}
			}
			return -1;
		}

		private static void MapSpace(OpcodeComputer opcodeComputer, int j, int depthLimit, int depth, (int x, int y) pos, HashSet<(int x, int y)> visitedPos)
		{
			visitedPos.Add(pos);

			if (depth == depthLimit) return;

			for ( int i = 1; i < 5; i++)
			{
				if (i == j) continue;
				opcodeComputer.SetInput(i);
				var output = opcodeComputer.GetOutput();

				if (output == 0) continue;

				int k = i % 2 == 0 ? i - 1 : (i + 1);
				MapSpace(opcodeComputer, k, depthLimit, depth + 1, PosAndDirToNewPos(pos, i), visitedPos);
				opcodeComputer.SetInput(k);
				if (opcodeComputer.GetOutput() == 0)
				{
					throw new InvalidDataException("WAT");
				}
			}
		}

		private static (int x, int y) PosAndDirToNewPos((int x, int y) pos, int dir)
		{
			return dir switch
			{
				1 => (pos.x, pos.y + 1),
				2 => (pos.x, pos.y - 1),
				3 => (pos.x + 1, pos.y),
				4 => (pos.x - 1, pos.y)
			};
		}

	}
}
