using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class ForestMap
{
	private List<List<int>> cells = new List<List<int>>();

	public ForestMap(string input)
	{
		using (StringReader reader = new StringReader(input))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				cells.Add(new List<int>(line.ToCharArray().Select(x => (int)Char.GetNumericValue(x))));
			}
		}
	}

	public int GetNumberOfVisibleTrees()
	{
		var visible = 0;

		// All of the tree on the edges are visible; add the borders then remove the corner which are
		// counted two times
		visible += cells[0].Count * 2;
		visible += cells.Count * 2;
		visible -= 4;

		for (int i = 1; i < cells.Count - 1; i++)
		{
			for (int j = 1; j < cells[i].Count - 1; j++)
			{
				var cell = cells[i][j];

				// Test on the right
				if (cells[i].Skip(j + 1).Take(cells[i].Count - j - 1).All(x => x < cell))
				{
					visible++;
					continue;
				}

				// Test on the left
				if (cells[i].Take(j).All(x => x < cell))
				{
					visible++;
					continue;
				}

				var currCol = cells.Select(x => x[j]).ToList();

				// Test up
				if (currCol.Take(i).All(x => x < cell))
				{
					visible++;
					continue;
				}

				// Test down
				if (currCol.Skip(i + 1).Take(cells.Count - i - 1).All(x => x < cell))
				{
					visible++;
					continue;
				}
			}
		}

		return visible;
	}

	public int GetMaxScenicScore()
	{
		var maxScore = 0;

		// We don't care for trees on the edge since they have a scenic score of 0
		for (int i = 1; i < cells.Count - 1; i++)
		{
			for (int j = 1; j < cells[i].Count - 1; j++)
			{
				var cell = cells[i][j];
				var cellScore = 1;

				// Test on the right; add 1 to the score since indexes will be 0-based
				var rightRow = cells[i].Skip(j + 1).Take(cells[i].Count - j - 1).ToList();
				var rightScore = rightRow.FindIndex(x => x >= cell);
				cellScore *= (rightScore >= 0 ? rightScore + 1 : rightRow.Count);

				// Test on the left; left and up lists must be reversed since the
				// indexes are counted while going away from the cell
				var leftRow = cells[i].Take(j).Reverse().ToList();
				var leftScore = leftRow.FindIndex(x => x >= cell);
				cellScore *= (leftScore >= 0 ? leftScore + 1 : leftRow.Count);

				var currCol = cells.Select(x => x[j]).ToList();

				// Test up
				var upCol = currCol.Take(i).Reverse().ToList();
				var upScore = upCol.FindIndex(x => x >= cell);
				cellScore *= (upScore >= 0 ? upScore + 1 : upCol.Count);

				// Test down
				var downCol = currCol.Skip(i + 1).Take(cells.Count - i - 1).ToList();
				var downScore = downCol.FindIndex(x => x >= cell);
				cellScore *= (downScore >= 0 ? downScore + 1 : downCol.Count);

				if (cellScore > maxScore)
				{
					maxScore = cellScore;
				}
			}
		}

		return maxScore;
	}
}

public partial class PuzzleSolver08 : Node
{
	// Build the map from the input and find the number of visible trees; a tree is visible if all of the
	// other trees between it and an edge of the grid are shorter than it
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_08/input.txt");
			var inputText = System.IO.File.ReadAllText(inputPath);
			var map = new ForestMap(inputText);
			var result = map.GetNumberOfVisibleTrees();

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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_08/input.txt");
			var inputText = System.IO.File.ReadAllText(inputPath);
			var map = new ForestMap(inputText);
			var result = map.GetMaxScenicScore();

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
