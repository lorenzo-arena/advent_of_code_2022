using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public partial class PuzzleSolver18 : Node
{
	private class Cube
	{
		public int x, y, z;

		public Cube(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	private bool checkSharedSide(Cube a, Cube b)
	{
		return ((a.x == (b.x - 1)) && (a.y == b.y) && (a.z == b.z)) ||
		       ((a.x == (b.x + 1)) && (a.y == b.y) && (a.z == b.z)) ||
		       ((a.x == b.x) && (a.y == (b.y + 1)) && (a.z == b.z)) ||
		       ((a.x == b.x) && (a.y == (b.y - 1)) && (a.z == b.z)) ||
		       ((a.x == b.x) && (a.y == b.y) && (a.z == (b.z + 1))) ||
		       ((a.x == b.x) && (a.y == b.y) && (a.z == (b.z - 1)));
	}

	private int getExposedSides(List<Cube> cubes)
	{
		int sides = 0;

		for (int idx = 0; idx < cubes.Count; idx++)
		{
			// Add all possible faces, then remove one when a side is shared
			sides += 6;
			for (int innerIdx = 0; innerIdx < cubes.Count; innerIdx++)
			{
				if ((idx != innerIdx) && checkSharedSide(cubes[idx], cubes[innerIdx]))
				{
					sides--;
				}
			}
		}

		return sides;
	}

	private int getOuterSides(List<Cube> cubes)
	{
		var exposed = getExposedSides(cubes);
		var ordered = cubes.OrderBy(cube => cube.y).ToList();

		for (int idx = 0;)



		// Count the top faces
		for (int idx = ordered.Count - 1; (idx >= 0) || (ordered[idx].y != ordered[ordered.Count - 1].y); idx--)
		{
			sides++;
		}



		return sides;
	}

	private List<Cube> getCubesList(string inputText)
	{
		var list = new List<Cube>();

		using (StringReader reader = new StringReader(inputText))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				var pos = line.Split(',');
				list.Add(new Cube(pos[0].ToInt(), pos[1].ToInt(), pos[2].ToInt()));
			}
		}

		return list;
	}


	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_18/input.txt");
			var cubes = getCubesList(System.IO.File.ReadAllText(inputPath));
			var result = getExposedSides(cubes);
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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_18/input.txt");
			var cubes = getCubesList(System.IO.File.ReadAllText(inputPath));
			var result = getOuterSides(cubes);
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
