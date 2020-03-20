using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task2
{
	public class OpcodeComputer
	{
		private const int POSITION = 0;
		private const int IMMEDIATE = 1;
		private const int RELATIVE = 2;
		
		private readonly long[] _originalMemory;

		private Queue<long> _input = new Queue<long>();
		private Queue<long> _output = new Queue<long>();
		private IDictionary<long, long> _memory;
		private long _onInputPointer = -1;
		private long _relativeBase = 0;

		private IDictionary<long, Func<long, long, long>> _opcodeToFunction = new Dictionary<long, Func<long, long, long>>();

		public OpcodeComputer(int[] memory) : this(memory.Select(i => (long)i).ToArray())
		{	
		}

		public OpcodeComputer(long[] memory)
		{
			_originalMemory = (long[])memory.Clone();
			ResetMemory();

			#region OpcodeFunctions
			_opcodeToFunction[01] = (p, d) => //Add
			{
				_memory[GetIndex(p, d, 3)] = _memory.GetOrDefault(GetIndex(p, d, 1), 0) + _memory.GetOrDefault(GetIndex(p, d, 2), 0);
				return p + 4;
			};
			_opcodeToFunction[02] = (p, d) => //Mul
			{
				_memory[GetIndex(p, d, 3)] = _memory.GetOrDefault(GetIndex(p, d, 1), 0) * _memory.GetOrDefault(GetIndex(p, d, 2), 0);
				return p + 4;
			};
			_opcodeToFunction[03] = (p, d) => //Read from input
			{
				if ( _input.TryDequeue(out var input))
				{
					_memory[GetIndex(p, d, 1)] = input;
					return p + 2;
				} else
				{
					_onInputPointer = p;
					return -1;
				}
			};
			_opcodeToFunction[04] = (p, d) => //Write to output
			{
				_output.Enqueue(_memory.GetOrDefault(GetIndex(p, d, 1), 0));
				return p + 2;
			};
			_opcodeToFunction[05] = (p, d) => //Jump not zero
			{
				return _memory.GetOrDefault(GetIndex(p, d, 1), 0) != 0 ? _memory.GetOrDefault(GetIndex(p, d, 2), 0) : (p + 3);
			};
			_opcodeToFunction[06] = (p, d) => //Jump zero
			{
				return _memory.GetOrDefault(GetIndex(p, d, 1), 0) == 0 ? _memory.GetOrDefault(GetIndex(p, d, 2), 0) : (p + 3);
			};
			_opcodeToFunction[07] = (p, d) => //Less than
			{
				_memory[GetIndex(p, d, 3)] = _memory.GetOrDefault(GetIndex(p, d, 1), 0) < _memory.GetOrDefault(GetIndex(p, d, 2), 0) ? 1 : 0;
				return p + 4;
			};
			_opcodeToFunction[08] = (p, d) => //Equal to
			{
				_memory[GetIndex(p, d, 3)] = _memory.GetOrDefault(GetIndex(p, d, 1), 0) == _memory.GetOrDefault(GetIndex(p, d, 2), 0) ? 1 : 0;
				return p + 4;
			};
			_opcodeToFunction[09] = (p, d) => //Change relative base
			{
				_relativeBase += _memory.GetOrDefault(GetIndex(p, d, 1), 0);
				return p + 2;
			};
			_opcodeToFunction[99] = (p, d) => -1;
			#endregion
		}

		private long GetIndex(long instructionPointer, long parameterMode, int parameterIndex)
		{
			return parameterMode.AtIndex(parameterIndex - 1) switch
			{
				POSITION => _memory.GetOrDefault(instructionPointer + parameterIndex, 0),
				IMMEDIATE => instructionPointer + parameterIndex,
				RELATIVE => _memory.GetOrDefault(instructionPointer + parameterIndex, 0) + _relativeBase
			};
		}

		public void ResetMemory()
		{
			_memory = new Dictionary<long, long>(_originalMemory.Select((l, i) => new KeyValuePair<long, long>(i, l)));
			_input.Clear();
			_output.Clear();
			_onInputPointer = -1;
			_relativeBase = 0;
		}

		public bool IsBlocked()
		{
			return _onInputPointer != -1;
		}

		public long RunInstructions(long p = 0)
		{
			for (; p != -1; p = _opcodeToFunction[_memory[p] % 100](p, _memory[p] / 100));
			return _memory[0];
		}

		public long RunInstructions(int noun, int verb)
		{
			_memory[1] = noun;
			_memory[2] = verb;
			return RunInstructions();
		}

		public void SetInput(long val)
		{
			_input.Enqueue(val);
			if ( _onInputPointer != -1)
			{
				var p = _onInputPointer;
				_onInputPointer = -1;
				RunInstructions(p);
			}
		}

		public long GetOutput()
		{
			if ( TryGetOutput(out var val))
			{
				return val;
			}
			throw new InvalidOperationException("No output available");
		}

		public bool TryGetOutput(out long output)
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
