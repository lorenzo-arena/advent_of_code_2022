using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PuzzleSolver03 : Node
{
	private int getPriority(char item)
	{
		if ((int)item >= (int)'a' && (int)item <= (int)'z')
		{
			return (int)item - (int)'a' + 1;
		}
		else if ((int)item >= (int)'A' && (int)item <= (int)'Z')
		{
			return (int)item - (int)'A' + 27;
		}
		else
		{
			throw new ArgumentException();
		}
	}

	// Each string must be split into two parts (rucksacks); then there will be exactly one
	// item (char) shared by the split strings; the priority must be found for that item and
	// added to the score
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_03/input.txt");
			var score = 0;
			
			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				var dict = new Dictionary<char, bool>();
				var firstRucksack = line.Substr(0, line.Length / 2);

				foreach (var item in firstRucksack)
				{
					if (!dict.ContainsKey(item))
					{
						dict.Add(item, true);
					}
				}

				var secondRucksack = line.Substr(line.Length / 2, line.Length - 1);

				foreach (var item in secondRucksack)
				{
					if (dict.ContainsKey(item))
					{
						score += getPriority(item);
						break;
					}
				}
			}

			GD.Print($"Final score for part 1 is {score}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	// The only shared item mustb e found which is in common for groups of 3 elves (3 rows);
	// priority must be computed with the same rule used for part 1
	private void solvePart2()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_03/input.txt");
			var score = 0;
			var elfIndex = 0;
			var firstElf = new Dictionary<char, bool>();
			var secondElf = new Dictionary<char, bool>();
			var thirdElf = new Dictionary<char, bool>();

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				if (elfIndex == 0)
				{
					foreach (var item in line)
					{
						if (!firstElf.ContainsKey(item))
						{
							firstElf.Add(item, true);
						}
					}

					elfIndex++;
				}
				else if (elfIndex == 1)
				{
					foreach (var item in line)
					{
						if (!secondElf.ContainsKey(item))
						{
							secondElf.Add(item, true);
						}
					}

					elfIndex++;
				}
				else if (elfIndex == 2)
				{
					foreach (var item in line)
					{
						if (!thirdElf.ContainsKey(item))
						{
							thirdElf.Add(item, true);
						}
					}

					// Process the group
					var resultDict = firstElf.Intersect(secondElf).Intersect(thirdElf);

					score += getPriority(resultDict.First().Key);

					// Reset group
					firstElf = new Dictionary<char, bool>();
					secondElf = new Dictionary<char, bool>();
					thirdElf = new Dictionary<char, bool>();

					elfIndex = 0;
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
