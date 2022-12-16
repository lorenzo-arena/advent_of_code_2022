using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class PuzzleSolver16 : Node
{
	private class Valve
	{
		private string id;
		private int flow;
		private List<string> tunnels;

		public string ID
		{
			get { return this.id; }
		}

		public int Flow
		{
			get { return this.flow; }
		}

		public List<string> Tunnels
		{
			get { return this.tunnels; }
		}

		public Valve(string id, int flow, List<string> tunnels)
		{
			this.id = id;
			this.flow = flow;
			this.tunnels = tunnels;
		}
	}

	private List<Valve> createValveList(string inputText)
	{
		var list = new List<Valve>();
		var sensorsRegex = @"^Valve (.+) has flow rate=(\d+); tunnel[s]? lead[s]? to valve[s]? (.+)$";
		Regex r = new Regex(sensorsRegex, RegexOptions.IgnoreCase);

		using (StringReader reader = new StringReader(inputText))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				Match m = r.Match(line);
				if (!m.Success)
				{
					throw new ArgumentException();
				}

				GD.Print($"processing {line}");
				var id = m.Groups[1].Captures[0].Value;
				var flow = m.Groups[2].Captures[0].Value.ToInt();
				var tunnels = m.Groups[3].Captures[0].Value.Split(", ").ToList();
				var valve = new Valve(id, flow, tunnels);

				list.Add(valve);
			}
		}

		return list;
	}

	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_16/input.txt");
			var valves = createValveList(System.IO.File.ReadAllText(inputPath));
			var result = 0;
			GD.Print($"Part 1 result is {result}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	private void solvePart2()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_16/input.txt");
			var result = 0;
			GD.Print($"Part 2 result is {result}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	public override void _Ready()
	{
		solvePart1();
		solvePart2();
	}
}
