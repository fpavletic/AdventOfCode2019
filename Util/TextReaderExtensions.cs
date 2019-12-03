using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode2019
{
	public static class TextReaderExtensions
	{
		public static IEnumerable<int> ReadInts(this TextReader textReader, int max = -1)
		{
			int count = 0;
			while (count++ != max && ReadInt(textReader, out int num))
			{
				yield return num;
			}
		}

		public static bool ReadInt(this TextReader textReader, out int num)
		{
			if (!FindDigit(textReader))
			{
				num = 0;
				return false;
			}

			var sb = new StringBuilder();
			while (char.IsDigit((char)textReader.Peek()))
			{
				sb.Append((char)textReader.Read());
			}
			num = int.Parse(sb.ToString());
			return true;
		}

		private static bool FindDigit(TextReader textReader)
		{
			while (textReader.Peek() != -1 && !char.IsDigit((char)textReader.Peek()))
			{
				textReader.Read();
			}
			return textReader.Peek() != -1;
		}

	}
}
