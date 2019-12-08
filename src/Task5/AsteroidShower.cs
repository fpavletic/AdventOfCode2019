using AdventOfCode2019.src.Task2;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task5
{
	class AsteroidShower
	{
		public static void MainTask5()
		{
			int[] values = File.OpenText(Util.INPUT_DIR + "task5.txt").ReadInts().ToArray();
			var opcodeComputer = new OpcodeComputer(values);

			opcodeComputer.SetInput(1);
			opcodeComputer.RunInstructions();
			opcodeComputer.TryGetOutput(out var output);
			Console.WriteLine(output);

			opcodeComputer.ResetMemory();
			opcodeComputer.SetInput(5);
			opcodeComputer.TryGetOutput(out output);
			Console.WriteLine(output);

		}

	}
}
