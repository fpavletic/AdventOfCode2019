using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2019.src.Task6
{
	class UniversalOrbitMap
	{
		public static void MainTask6()
		{
			IDictionary<string, Node<string>> celestialBodyToOrbiters = new Dictionary<string, Node<string>>();
			var reader = File.OpenText(Util.INPUT_DIR + "task6.txt");
			var line = (string) null;
			while ( (line = reader.ReadLine()) != null ){
				var split = line.Split(")");
				if (!celestialBodyToOrbiters.ContainsKey(split[0]))
				{
					celestialBodyToOrbiters[split[0]] = new Node<string>(split[0]);
				}
				if (!celestialBodyToOrbiters.ContainsKey(split[1]))
				{
					celestialBodyToOrbiters[split[1]] = new Node<string>(split[0]);
				}

				var parentNode = celestialBodyToOrbiters[split[0]];
				var childNode = celestialBodyToOrbiters[split[1]];

				parentNode.AddChild(childNode);
				childNode.Parent = parentNode;
			}

			IEnumerable<Node<string>> current = celestialBodyToOrbiters["COM"].Children;
			var orbits = 0;
			for (int depth = 1; current.Count() != 0; depth++)
			{
				orbits += current.Count() * depth;
				
				foreach(var celestialBody in current)
				{
					celestialBody.Depth = depth;
				}

				current = current.SelectMany(s => s.Children);
			}
			Console.WriteLine(orbits);

			var node1 = celestialBodyToOrbiters["YOU"];
			var node2 = celestialBodyToOrbiters["SAN"];
			if ( node1.Depth < node2.Depth)
			{
				var tmp = node1;
				node1 = node2;
				node2 = tmp;
			}

			var distance = 0;
			while ( node1.Depth > node2.Depth)
			{
				node1 = node1.Parent;
				distance++;
			}

			while (node1.Parent != node2.Parent)
			{
				node1 = node1.Parent;
				node2 = node2.Parent;
				distance += 2;
			}

			Console.WriteLine(distance);
		}

		private class Node<T>
		{
			public T Value { get; private set; }
			public Node<T> Parent { get; set; }
			public int Depth { get; set; }
			public List<Node<T>> Children { get; private set; }

			public Node(T value, int depth = 0, Node<T> parent = null)
			{
				Value = value;
				Depth = depth;
				Parent = null;
				Children = new List<Node<T>>();
			}

			public void AddChild(Node<T> child)
			{
				Children.Add(child);
			}

			public override bool Equals(object obj)
			{
				return obj is Node<T> node &&
					   EqualityComparer<T>.Default.Equals(Value, node.Value);
			}

			public override int GetHashCode()
			{
				return HashCode.Combine(Value);
			}

			public static bool operator ==(Node<T> left, Node<T> right)
			{
				return EqualityComparer<Node<T>>.Default.Equals(left, right);
			}

			public static bool operator !=(Node<T> left, Node<T> right)
			{
				return !(left == right);
			}
		}
	}
}
