using AdventOfCode2019.src.Task2;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task9
{
	class SensorBoost
	{
		public static void MainTask9()
		{
			var opcodeComputer = new OpcodeComputer(File.OpenText(Util.INPUT_DIR + "task9.txt").ReadLongs().ToArray());

			opcodeComputer.SetInput(1);
			opcodeComputer.RunInstructions();
			opcodeComputer.TryGetOutput(out var output);
			Console.WriteLine(output);

			opcodeComputer.ResetMemory();
			opcodeComputer.SetInput(2);
			opcodeComputer.RunInstructions();
			opcodeComputer.TryGetOutput(out output);
			Console.WriteLine(output);
		}

	}
}
