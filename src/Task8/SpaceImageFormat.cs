using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2019.src.Task8
{
	class SpaceImageFormat
	{
		private const int IMG_WIDTH = 25;
		private const int IMG_HEIGTH = 6;
		private const int NUM_OFFSET = 48;

		public static void MainTask8()
		{
			Console.WriteLine(HandleLayers<int>(HandleProduct));
			var image = (HandleLayers<char[]>(HandleImage));
			for ( int i = 0; i < IMG_WIDTH; i++)
			{
				for ( int j = 0; j < IMG_HEIGTH; j++)
				{
					Console.Write(image[i + j * IMG_WIDTH]);
				}
				Console.WriteLine();
			}
		}

		private static T HandleLayers<T>(Func<char[], T> layerHandler)
		{
			var buffer = new char[IMG_WIDTH * IMG_HEIGTH];
			var textReader = File.OpenText(Util.INPUT_DIR + "task8.txt");

			var result = default(T);

			while (textReader.ReadBlock(buffer) != 0)
			{
				result = layerHandler(buffer);
			}

			return result;
		}

		private static readonly char[] image = Enumerable.Repeat('?', IMG_WIDTH * IMG_HEIGTH).ToArray();
		private static char[] HandleImage(char[] buffer)
		{
			for (int i = 0; i < IMG_WIDTH * IMG_HEIGTH; i++)
			{
				if (image[i] == '?') image[i] = (int)buffer[i] switch
				{
					48 => '.',
					49 => '#',
					50 => '?'
				};
			}
			return image;
		}

		private static int minZeros = int.MaxValue;
		private static int countProduct = 0;
		private static int HandleProduct(char[] buffer)
		{

			var a = buffer.Aggregate(new int[3], (a, c) =>
			{
				a[c - NUM_OFFSET]++;
				return a;
			});

			if (a[0] < minZeros)
			{
				minZeros = a[0];
				countProduct = a[1] * a[2];
			}

			return countProduct;
		}

	}
}
