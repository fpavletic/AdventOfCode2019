using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
	public static class MathExtensions
	{
		public static int Pow(this int i, int pow)
		{
			var k = 1;
			for (int j = 0; j < pow; j++, k *= i) ;
			return k;
		}

		public static int AtIndex(this int i, int index)
		{
			return i / 10.Pow(index) % 10;
		}

		public static bool InRange(this int val, int min, int max)
		{
			return val > min && val < max;
		}
	}
}
