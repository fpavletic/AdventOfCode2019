using AdventOfCode2019.src.Task2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.src.Task11
{
	class SpacePolice
	{
		public static void MainTask11()
		{
			var opcodeComputer = new OpcodeComputer(File.OpenText(Util.INPUT_DIR + "task11.txt").ReadLongs().ToArray());
			opcodeComputer.RunInstructions();
			Console.WriteLine(PaintRegistration(opcodeComputer, 0).Count);
			opcodeComputer.ResetMemory();
			opcodeComputer.RunInstructions();
			var registration = PaintRegistration(opcodeComputer).Where(e => e.Value == 1).OrderBy(e => e.Key.x).ToList();
		}

		public static Dictionary<(int x, int y), int> PaintRegistration(OpcodeComputer opcodeComputer, int startingPanel = 1)
		{
			var coordinatesToPaintColor = new Dictionary<(int x, int y), int>()
			{
				[(0,0)] = startingPanel
			};

			(int x, int y) position = (0, 0);
			var direction = 0;

			while (opcodeComputer.IsBlocked())
			{
				opcodeComputer.SetInput(coordinatesToPaintColor.GetOrDefault(position, 0));
				coordinatesToPaintColor[position] = (int)opcodeComputer.GetOutput();
				direction += 3 + (int)opcodeComputer.GetOutput() * 2;
				direction %= 4;
				position = direction switch
				{
					0 => (position.x, position.y + 1),
					1 => (position.x + 1, position.y),
					2 => (position.x, position.y - 1),
					3 => (position.x - 1, position.y)
				};
			}

			return coordinatesToPaintColor;
		}

	}


}
