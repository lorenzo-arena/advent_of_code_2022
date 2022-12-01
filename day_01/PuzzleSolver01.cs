using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PuzzleSolver01 : Node
{
	// Sum the total calories of each of the elves, where each elf is made of multiple rows
	// and elves are separated by an empty line
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_01/input.txt");
			var inputText = System.IO.File.ReadAllText(inputPath);
			var elves = new List<int>();

			// Push the first elf
			elves.Add(0);

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				if (line != "")
				{
					elves[elves.Count - 1] += line.ToInt();
				}
				else
				{
					elves.Add(0);
				}
			}

			var max = elves.Max();
			GD.Print($"Max calories are: {max}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	// Count the calories again, but find the top three elves carrying the most calories;
	// then sum up their calories
	private void solvePart2()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_01/input.txt");
			var inputText = System.IO.File.ReadAllText(inputPath);
			var elves = new List<int>();

			// Push the first elf
			elves.Add(0);

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				if (line != "")
				{
					elves[elves.Count - 1] += line.ToInt();
				}
				else
				{
					elves.Add(0);
				}
			}

			elves.Sort();

			var result = elves[elves.Count - 1] + elves[elves.Count - 2] + elves[elves.Count - 3];
			GD.Print($"Accumulated calories for the top 3 elves are: {result}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		solvePart1();
		solvePart2();
	}
}
