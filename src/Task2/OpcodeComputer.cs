using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.src.Task2
{
	class OpcodeComputer
	{
		public static void MainTask2()
		{
			int[] values = File.OpenText(Util.INPUT_DIR + "task2.txt").ReadInts().ToArray();

			Console.WriteLine(RunInstructions((int[])values.Clone(), 12, 2));

			var parameters = FindParameters(values, 19690720);
			Console.WriteLine(100 * parameters.noun + parameters.verb);

		}

		private static (int noun, int verb) FindParameters(int[] memory, int output)
		{
			var tmpMemory = new int[memory.Length];

			for (int i = 0; i < 100; i++) {
				for (int j = 0; j < 100; j++) {
					Array.Copy(memory, 0, tmpMemory, 0, memory.Length);
					if ( RunInstructions(tmpMemory, i, j) == output)
					{
						return (i, j);
					}
				} 
			}

			return (-1, -1);
		}

		private static int RunInstructions(int[] memory, int noun, int verb)
		{
			memory[1] = noun;
			memory[2] = verb;

			for (int i = 0; memory[i] != 99; i += 4)
			{
				memory[memory[i + 3]] = memory[i] == 1 ? memory[memory[i + 1]] + memory[memory[i + 2]] :
														memory[memory[i + 1]] * memory[memory[i + 2]];
			}

			return memory[0];
		}

	}
}
