using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public partial class PuzzleSolver05 : Node
{
	private void moveStacks(List<string> stacks, int move, int from, int to, bool reverse)
	{
		var toMove = stacks[from].Substring(stacks[from].Length - move, move);
		stacks[from] = stacks[from].Substring(0, stacks[from].Length - move);

		if (reverse)
		{
			var toMoveArr = toMove.ToCharArray();
			Array.Reverse(toMoveArr);
			stacks[to] += new string(toMoveArr);
		}
		else
		{
			stacks[to] += new string(toMove);
		}
	}

	private void solvePart1()
	{
		try
		{
			// Hardcode stacks for now
			var containerInputPath = Godot.ProjectSettings.GlobalizePath("res://day_05/input_a.txt");
			var commandsInputPath = Godot.ProjectSettings.GlobalizePath("res://day_05/input_b.txt");
			string commandRegex = @"move (\d+) from (\d+) to (\d+)";
			Regex r = new Regex(commandRegex, RegexOptions.IgnoreCase);
			var result = "";
			var stacks = new List<string>();

			stacks.Add("SLW");
			stacks.Add("JTNQ");
			stacks.Add("SCHFJ");
			stacks.Add("TRMWNGB");
			stacks.Add("TRLSDHQB");
			stacks.Add("MJBVFHRL");
			stacks.Add("DWRNJM");
			stacks.Add("BZTFHNDJ");
			stacks.Add("HLQNBFT");

			foreach (var line in System.IO.File.ReadAllLines(commandsInputPath))
			{
				if (line.Length != 0)
				{
					int move = 0;
					int from = 0;
					int to = 0;
					Match m = r.Match(line);
					
					if (m.Success)
					{
						move = m.Groups[1].Captures[0].Value.ToInt();
						// Indexed are 1-based in the input
						from = m.Groups[2].Captures[0].Value.ToInt() - 1;
						to = m.Groups[3].Captures[0].Value.ToInt() - 1;
					}

					moveStacks(stacks, move, from, to, true);
				}
			}

			foreach (var stack in stacks)
			{
				result += stack[stack.Length - 1];
			}

			GD.Print($"Top containers for part 1 are {result}");
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
			// Hardcode stacks for now
			var containerInputPath = Godot.ProjectSettings.GlobalizePath("res://day_05/input_a.txt");
			var commandsInputPath = Godot.ProjectSettings.GlobalizePath("res://day_05/input_b.txt");
			string commandRegex = @"move (\d+) from (\d+) to (\d+)";
			Regex r = new Regex(commandRegex, RegexOptions.IgnoreCase);
			var result = "";
			var stacks = new List<string>();

			stacks.Add("SLW");
			stacks.Add("JTNQ");
			stacks.Add("SCHFJ");
			stacks.Add("TRMWNGB");
			stacks.Add("TRLSDHQB");
			stacks.Add("MJBVFHRL");
			stacks.Add("DWRNJM");
			stacks.Add("BZTFHNDJ");
			stacks.Add("HLQNBFT");

			foreach (var line in System.IO.File.ReadAllLines(commandsInputPath))
			{
				if (line.Length != 0)
				{
					int move = 0;
					int from = 0;
					int to = 0;
					Match m = r.Match(line);
					
					if (m.Success)
					{
						move = m.Groups[1].Captures[0].Value.ToInt();
						// Indexed are 1-based in the input
						from = m.Groups[2].Captures[0].Value.ToInt() - 1;
						to = m.Groups[3].Captures[0].Value.ToInt() - 1;
					}

					moveStacks(stacks, move, from, to, false);
				}
			}

			foreach (var stack in stacks)
			{
				result += stack[stack.Length - 1];
			}

			GD.Print($"Top containers for part 2 are {result}");
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
