using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2019
{
	public static class IDictionaryExtensions
	{

		public static V GetOrDefault<K,V>(this IDictionary<K,V> dict, K key, V defaultVal)
		{
			if (!dict.TryGetValue(key, out var val))
			{
				val = defaultVal;
			}
			return val;
		}
	}
}
