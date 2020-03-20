using AdventOfCode2019.src.Task2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task13
{
	class CarePackage
	{
		private const long EMPTY = 0;
		private const long WALL = 1;
		private const long BLOCK = 2;
		private const long PADDLE = 3;
		private const long BALL = 4;
		public static void MainTask13()
		{
			var memory = File.OpenText(Util.INPUT_DIR + "task13.txt").ReadLongs().ToArray();
			memory[0] = 2;
			var opcodeComputer = new OpcodeComputer(memory);
			opcodeComputer.RunInstructions();
			(long x, long y) ball = default;
			(long x, long y) paddle = default;

			long blockCount = 0;
			while (opcodeComputer.TryGetOutput(out var x))
			{
				var y = opcodeComputer.GetOutput();
				var category = opcodeComputer.GetOutput();
				switch ( category)
				{
					case BALL:
						ball = (x, y);
						break;
					case PADDLE:
						paddle = (x, y);
						break;
					case BLOCK:
						blockCount++;
						break;
					default:
						break;
				}
			}
			Console.WriteLine(blockCount);

			long score = 0;
			while (blockCount != 0)
			{
				opcodeComputer.SetInput(ball.x == paddle.x ? 0 : ball.x > paddle.x ? 1 : -1);
				while (opcodeComputer.TryGetOutput(out var x))
				{
					var y = opcodeComputer.GetOutput();
					var category = opcodeComputer.GetOutput();
					switch (category)
					{
						case EMPTY: //0
						case WALL: //1
						case BLOCK: //2
							break;
						case PADDLE: //3
							paddle = (x, y);
							break;
						case BALL: //4
							ball = (x, y);
							break;
						default:
							Console.WriteLine($"{x}, {y}");
							score = category;
							blockCount--;
							break;
					}
				}
			}
			Console.WriteLine(score);
		}
	}
}
