using System;
using System.IO;

namespace AdventOfCode2019.src.Task4
{
	public class SecureContainer
	{
		public static void MainTask4()
		{
			var textReader = File.OpenText(Util.INPUT_DIR + "task4.txt");
			Console.WriteLine(CountPossible(0, 6, -1, (textReader.ReadInt(), textReader.ReadInt())));
		}
		
		public static int CountPossible(int num, int digitsLeft, int lastDigit, (int min, int max) range, bool hadMatchingPair = false, int matchingSequenceLen = 0)
		{
			if (digitsLeft == 0)
			{ 
				return (hadMatchingPair || matchingSequenceLen == 2 ) && num.InRange(range.min, range.max) ? 1 : 0;
			}

			int sum = 0;
			for (int i = lastDigit; i <= 9; i++)
			{
				sum += CountPossible(num * 10 + i, digitsLeft - 1, i, range, hadMatchingPair || (i != lastDigit && matchingSequenceLen == 2), (i == lastDigit ? matchingSequenceLen + 1 : 1));
			}
			return sum;
		}


	}
}
