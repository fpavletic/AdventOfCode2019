using AdventOfCode2019.src.Task2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task7
{
	class AmplificationCircuit
	{

		private static int[] _values;

		private OpcodeComputer _computer;
		private long _mode;

		public AmplificationCircuit(OpcodeComputer computer, int mode)
		{
			_computer = computer;
			_mode = mode;
			Reset();
		}

		public void Reset()
		{
			_computer.ResetMemory();
			_computer.SetInput(_mode);
			_computer.RunInstructions();
		}

		public bool Amplify(long input, out long output)
		{
			if ( _computer.IsBlocked())
			{
				_computer.SetInput(input);
				return _computer.TryGetOutput(out output);
			}
			output = 0;
			return false;
		}

		public static void MainTask7()
		{

			var inputs = Enumerable.Range(0, 5).ToHashSet<int>();
			_values = File.OpenText(Util.INPUT_DIR + "task7.txt").ReadInts().ToArray();

			var amplifiers = Enumerable.Range(0, 5).Select(i => new AmplificationCircuit(new OpcodeComputer(_values), i)).ToArray();
			Console.WriteLine(AmplificationCircuitRecursion(inputs, amplifiers, new int[5], HandeSimple));

			amplifiers = Enumerable.Range(5, 5).Select(i => new AmplificationCircuit(new OpcodeComputer(_values), i)).ToArray();
			Console.WriteLine(AmplificationCircuitRecursion(inputs, amplifiers, new int[5], HandleFeedback));

		}

		private static long AmplificationCircuitRecursion(ISet<int> inputs, AmplificationCircuit[] amplifiers, int[] indices, Func<AmplificationCircuit[], int[], long> baseCaseHandler)
		{
			if (!inputs.Any())
			{
				return baseCaseHandler(amplifiers, indices);
			}
			else
			{
				var maxAmp = 0l;
				foreach ( var input in inputs)
				{
					indices[inputs.Count() - 1] = input;
					var newInputs = new HashSet<int>(inputs);
					newInputs.Remove(input);

					var amp = AmplificationCircuitRecursion(newInputs, amplifiers, indices, baseCaseHandler);
					if (amp > maxAmp) maxAmp = amp;
				}
				return maxAmp;
			}
		}

		private static long HandleFeedback(AmplificationCircuit[] amplifiers, int[] indices) {
			foreach(var amplifier in amplifiers)
			{
				amplifier.Reset();
			}

			var input = 0l;
			var index = 0;
			var amplifierBlocked = true;
			while (amplifierBlocked)
			{
				amplifierBlocked = amplifiers[indices[index]]._computer.IsBlocked();
				if (amplifiers[indices[index]].Amplify(input, out var output))
				{
					input = output;
				}
				index = (index + 1 ) % indices.Length;
			}
			return input;
		}

		private static long HandeSimple(AmplificationCircuit[] amplifiers, int[] indices)
		{
			foreach (var amplifier in amplifiers)
			{
				amplifier.Reset();
			}

			long input = 0;
			for ( int i = 0; i < indices.Length; i++)
			{
				if (amplifiers[indices[i]].Amplify(input, out var output)){
					input = output;
				}
			}
			return input;
		}

	}
}
