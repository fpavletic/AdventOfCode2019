using System;
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

		public static int ReadString(this TextReader textReader, out string output, int max = -1)
		{
			if (max == -1)
			{
				output = textReader.ReadToEnd();
				return output.Length;
			}

			var buffer = new char[max];
			var outputLen = textReader.ReadBlock(buffer, 0, max);
			output = new string(buffer, 0, outputLen);
			return outputLen;
		}

		public static int ReadInt(this TextReader textReader)
		{
			if ( textReader.ReadInt(out var val))
			{
				return val;
			}
			throw new InvalidOperationException("Cant find any more integers");
		}

		public static bool ReadInt(this TextReader textReader, out int num)
		{
			if (!FindDigitOrMinus(textReader))
			{
				num = 0;
				return false;
			}

			var sb = new StringBuilder();
			while (IsDigitOrMinus((char)textReader.Peek()))
			{
				sb.Append((char)textReader.Read());
			}
			num = int.Parse(sb.ToString());
			return true;
		}

		private static bool FindDigitOrMinus(TextReader textReader)
		{
			while (textReader.Peek() != -1 && !IsDigitOrMinus((char)textReader.Peek()))
			{
				textReader.Read();
			}
			return textReader.Peek() != -1;
		}

		private static bool IsDigitOrMinus(char character)
		{
			return char.IsDigit(character) || character == '-';
		}

	}
}
