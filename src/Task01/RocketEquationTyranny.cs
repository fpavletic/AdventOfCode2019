using System;
using System.IO;

namespace AdventOfCode2019.Task1
{
	class RocketEquationTyranny
	{
		public static void MainTask1()
		{
			var sumWeightlessFuel = 0;
			var sumActual = 0;


			foreach (var weight in File.OpenText(Util.INPUT_DIR + "task1.txt").ReadInts())
			{
				sumWeightlessFuel += weight / 3 - 2;
				sumActual += CalculateFuelReq(weight);
			}
			Console.WriteLine(sumWeightlessFuel);
			Console.WriteLine(sumActual);
		}

		private static int CalculateFuelReq(int weight)
		{
			var sum = 0;
			while (weight > 8)
			{
				sum += (weight = weight / 3 - 2);
			}
			return sum;
		}
	}
}
