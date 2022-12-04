using Godot;
using System;
using System.Linq;

// TODO : maybe this would be a good problem to render

public partial class PuzzleSolver04 : Node
{
	// The input contains a list of sections assigned to each elf; the score must be equal
	// to the rows where a section fully contains another section
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_04/input.txt");
			var score = 0;

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				var sections = line.Split(',');
				var firstSections = sections[0].Split('-').Select(section => section.ToInt()).ToArray();
				var secondSections = sections[1].Split('-').Select(section => section.ToInt()).ToArray();

				if ((firstSections[0] <= secondSections[0] && firstSections[1] >= secondSections[1]) ||
				    (secondSections[0] <= firstSections[0] && secondSections[1] >= firstSections[1]))
				{
					score += 1;
				}
			}

			GD.Print($"Final score for part 1 is {score}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	// The input contains a list of sections assigned to each elf; the score must be equal
	// to the rows where a section overlaps with another section
	private void solvePart2()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_04/input.txt");
			var score = 0;

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				var sections = line.Split(',');
				var firstSections = sections[0].Split('-').Select(section => section.ToInt()).ToArray();
				var secondSections = sections[1].Split('-').Select(section => section.ToInt()).ToArray();

				if ((firstSections[0] <= secondSections[0] && firstSections[1] >= secondSections[0]) ||
				    (secondSections[0] <= firstSections[0] && secondSections[1] >= firstSections[0]))
				{
					score += 1;
				}
			}

			GD.Print($"Final score for part 2 is {score}");
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
