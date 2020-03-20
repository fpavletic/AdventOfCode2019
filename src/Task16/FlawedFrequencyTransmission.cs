using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.src.Task16
{
	class FlawedFrequencyTransmission
	{
		private static readonly int[] PATTERN = new int[] { 0, 1, 0, -1 };

		public static void Main()
		{
			var input = File.OpenText(Util.INPUT_DIR + "task16.txt").ReadToEnd().ToCharArray().Select(c => (byte)(c - 48)).ToArray();
			Console.WriteLine(Part1(input));

		}

		private static string Path2(byte[] input)
		{

		}

		private static string Part1(byte[] input)
		{
			input = (byte[])input.Clone();
			for (int k = 0; k < 100; k++)
			{
				var output = new byte[input.Length];
				for (int i = 0; i < input.Length; i++)
				{
					var val = 0;
					for (int j = 0; j < input.Length; j++)
					{
						val += input[j] * PATTERN[((j + 1) / (i + 1)) % 4];
					}
					output[i] = (byte)(val % 10);
				}
				input = output;
			}
			return input.Take(8).Aggregate(new StringBuilder(), (s, i) => s.Append(i)).ToString();
		}

	}
}
