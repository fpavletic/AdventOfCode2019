using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task2
{
	public class OpcodeComputer
	{
		private Queue<int> _input;
		private Queue<int> _output;
		private int[] _originalMemory;
		private int[] _memory;
		private int _onInputPointer = -1;

		private IDictionary<int, Func<int, int, int>> _opcodeToFunction = new Dictionary<int, Func<int, int, int>>();

		public OpcodeComputer(int[] memory)
		{
			_originalMemory = memory;
			ResetMemory();

			#region OpcodeFunctions
			_opcodeToFunction[01] = (p, d) =>
			{
				_memory[GetIndex(p, d, 3)] = _memory[GetIndex(p, d, 1)] + _memory[GetIndex(p, d, 2)];
				return p + 4;
			};
			_opcodeToFunction[02] = (p, d) =>
			{
				_memory[GetIndex(p, d, 3)] = _memory[GetIndex(p, d, 1)] * _memory[GetIndex(p, d, 2)];
				return p + 4;
			};
			_opcodeToFunction[03] = (p, d) =>
			{
				if ( _input.TryDequeue(out var input))
				{
					_memory[_memory[p + 1]] = input;
					return p + 2;
				} else
				{
					_onInputPointer = p;
					return -1;
				}
			};
			_opcodeToFunction[04] = (p, d) =>
			{
				_output.Enqueue(_memory[GetIndex(p, d, 1)]);
				return p + 2;
			};
			_opcodeToFunction[05] = (p, d) => 
			{
				return _memory[GetIndex(p, d, 1)] != 0 ? _memory[GetIndex(p, d, 2)] : (p + 3);
			};
			_opcodeToFunction[06] = (p, d) =>
			{
				return _memory[GetIndex(p, d, 1)] == 0 ? _memory[GetIndex(p, d, 2)] : (p + 3);
			};
			_opcodeToFunction[07] = (p, d) =>
			{
				_memory[GetIndex(p, d, 3)] = _memory[GetIndex(p, d, 1)] < _memory[GetIndex(p, d, 2)] ? 1 : 0;
				return p + 4;
			};
			_opcodeToFunction[08] = (p, d) =>
			{
				_memory[GetIndex(p, d, 3)] = _memory[GetIndex(p, d, 1)] == _memory[GetIndex(p, d, 2)] ? 1 : 0;
				return p + 4;
			};
			_opcodeToFunction[99] = (p, d) => -1;
			#endregion
		}

		private int GetIndex(int p, int d, int parameterIndex)
		{
			return d.AtIndex(parameterIndex - 1) == 0 ? _memory[p + parameterIndex] : p + parameterIndex;
		}

		public void ResetMemory()
		{
			_memory = (int[])_originalMemory.Clone();
			_input = new Queue<int>();
			_output = new Queue<int>();
			_onInputPointer = -1;
		}

		public bool IsBlocked()
		{
			return _onInputPointer != -1;
		}

		public int RunInstructions(int p = 0)
		{
			for (; p != -1; p = _opcodeToFunction[_memory[p] % 100](p, _memory[p] / 100)) ;
			return _memory[0];
		}

		public int RunInstructions(int noun, int verb)
		{
			_memory[1] = noun;
			_memory[2] = verb;
			return RunInstructions();
		}

		public void SetInput(int val)
		{
			_input.Enqueue(val);
			if ( _onInputPointer != -1)
			{
				var p = _onInputPointer;
				_onInputPointer = -1;
				RunInstructions(p);
			}
		}

		public bool TryGetOutput(out int output)
		{
			return _output.TryDequeue(out output);
		}

		public static void MainTask2()
		{
			int[] values = File.OpenText(Util.INPUT_DIR + "task2.txt").ReadInts().ToArray();
			var opcodeComputer = new OpcodeComputer(values);
			Console.WriteLine(opcodeComputer.RunInstructions(12, 2));

			var parameters = FindParameters(opcodeComputer, 19690720);
			Console.WriteLine(100 * parameters.noun + parameters.verb);
		}

		private static (int noun, int verb) FindParameters(OpcodeComputer opcodeComputer, int output)
		{
			for (int i = 0; i < 100; i++) {
				for (int j = 0; j < 100; j++) {
					opcodeComputer.ResetMemory();
					if (opcodeComputer.RunInstructions(i, j) == output)
					{
						return (i, j);
					}
				} 
			}
			return (-1, -1);
		}

		

	}
}
