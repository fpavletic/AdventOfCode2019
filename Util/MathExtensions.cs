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

		public static long Pow(this long l, int pow)
		{
			var k = 1L;
			for (int j = 0; j < pow; j++, k *= l) ;
			return k;
		}

		public static long AtIndex(this long l, int index)
		{
			return l / 10.Pow(index) % 10;
		}

		public static long Max(params long[] values)
		{
			var max = long.MinValue;
			foreach ( var value in values)
			{
				if (value > max)
				{
					max = value;
				}
			}
			return max;
		}
	}
}
