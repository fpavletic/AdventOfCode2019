using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.src.Task12
{
	class NBodyProblem
	{
		public static void MainTask12()
		{
			int[][] pos = new int[][]
			{
				new int[] { -3, 10, -1},
				new int[] {-12, -10, -5},
				new int[] {-9, 0, 10},
				new int[] {7, -5, -3}
			};

			int[][] vel = new int[][]
			{
				new int[] {0, 0, 0},
				new int[] {0, 0, 0},
				new int[] {0, 0, 0},
				new int[] {0, 0, 0}
			};

			for (int iter = 0; iter < 1000; iter++)
			{

				for (int i = 0; i < 4; i++)
				{
					for (int j = i + 1; j < 4; j++)
					{
						for (int k = 0; k < 3; k++)
						{
							var vk = pos[i][k] > pos[j][k] ? -1 : (pos[i][k] == pos[j][k] ? 0 : 1);
							vel[i][k] += vk;
							vel[j][k] -= vk;
						}
					}
				}

				for (int i = 0; i < 4; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						pos[i][j] += vel[i][j];
					}
				}
			}

			var energy = Enumerable.Range(0, 4).Select(i =>
			{
				return Enumerable.Range(0, 3)
				.Select(j => (Math.Abs(pos[i][j]), Math.Abs(vel[i][j])))
				.Aggregate((0, 0), (a, s) => a = (a.Item1 + s.Item1, a.Item2 + s.Item2));

			}).Aggregate(0, (a, s) => a += s.Item1 * s.Item2);
			Console.WriteLine(energy);

			var axisCycleLen = new long[] { -1, -1, -1 };
			for ( int i = 0; i < 3; i++)
			{
				axisCycleLen[i] = getAxisCycleLen(pos.Select(m => m[i]).ToArray(), vel.Select(m => m[i]).ToArray());
			}

			Console.WriteLine(getMinCommonMultiple(axisCycleLen));
		}

		private static long getAxisCycleLen(int[] pos, int[] vel)
		{
			var originalPos = (int[])pos.Clone();
			var originalVel = (int[])vel.Clone();
			for (int iter = 0; true; iter++)
			{
				for (int i = 0; i < 4; i++)
				{
					for (int j = i + 1; j < 4; j++)
					{
						var v = pos[i] > pos[j] ? -1 : (pos[i] == pos[j] ? 0 : 1);
						vel[i] += v;
						vel[j] -= v;
					}
				}

				for (int i = 0; i < 4; i++)
				{
					pos[i] += vel[i];
				}

				if (pos.SequenceEqual(originalPos) && vel.SequenceEqual(originalVel))
				{
					return iter + 1;
				}
			}
		}

		private static long getMinCommonMultiple(params long[] values)
		{
			var lcm = 1L;

			var divisor = 2L;
			var counter = 0;

			while ( counter != values.Length)
			{
				counter = 0;
				var divisible = false;
				for (int i = 0; i <values.Length; i++ )
				{
					if ( values[i] == 1L)
					{
						counter++;
					} else if (values[i] % divisor == 0L)
					{
						divisible = true;
						values[i] /= divisor;
					}
				}

				if ( divisible)
				{
					lcm *= divisor;
				} else
				{
					divisor++;
				}
			}

			return lcm;
		}
	}
}
