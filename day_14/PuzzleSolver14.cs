using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public partial class PuzzleSolver14 : Node
{
	private enum TileType
	{
		AIR,
		ROCK,
		SAND,
	}

	private class CaveMap
	{
		private List<List<TileType>> cave;

		public CaveMap(int width, int height)
		{
			cave = new List<List<TileType>>();
			foreach (var _ in Enumerable.Range(0, height))
			{
				cave.Add(Enumerable.Repeat(TileType.AIR, width).ToList());
			}
		}

		public void AddRocks(Vector2 start, Vector2 end)
		{
			if (start.x == end.x)
			{
				for (int idx = (int)Math.Min(start.y, end.y); idx <= (int)Math.Max(start.y, end.y); idx++)
				{
					cave[idx][(int)start.x] = TileType.ROCK;
				}
			}
			else if (start.y == end.y)
			{
				for (int idx = (int)Math.Min(start.x, end.x); idx <= (int)Math.Max(start.x, end.x); idx++)
				{
					cave[(int)start.y][idx] = TileType.ROCK;
				}
			}
			else
			{
				// At least one coordinate has to be in common as we're drawing a line
				throw new ArgumentException();
			}
		}

		public void AddFloor()
		{
			var floorIdx = 0;
			for (int idx = 0; idx < cave.Count; idx++)
			{
				if (cave[idx].IndexOf(TileType.ROCK) != -1)
				{
					floorIdx = idx;
				}
			}
			floorIdx += 2;

			cave[floorIdx] = Enumerable.Repeat(TileType.ROCK, cave[floorIdx].Count).ToList();
		}

		private void removeSand()
		{
			this.cave = this.cave.Select(row => row.Select(tile => (tile == TileType.SAND) ? TileType.AIR : tile).ToList()).ToList();
		}

		private Vector2? getNextSandPosition(Vector2 sourcePos)
		{
			var rowIdx = (int)sourcePos.x;
			var colIdx = (int)sourcePos.y;
			var column = cave.Select(row => row[colIdx]).Skip(rowIdx).ToList();
			var solidIdx = column.FindIndex(tile => tile != TileType.AIR);

			if (solidIdx == -1)
			{
				return null;
			}

			// A solid has been found, check if space is available on left or on right
			if ((colIdx == 0) || ((colIdx > 0) && (cave[solidIdx + rowIdx][colIdx - 1] == TileType.AIR)))
			{
				return getNextSandPosition(new Vector2(solidIdx + rowIdx, colIdx - 1));
			}

			if ((colIdx == cave.Count - 1) || ((colIdx < cave.Count) && (cave[solidIdx + rowIdx][colIdx + 1] == TileType.AIR)))
			{
				return getNextSandPosition(new Vector2(solidIdx + rowIdx, colIdx + 1));
			}

			return new Vector2(colIdx, solidIdx + rowIdx - 1);
		}

		public int StartSimulation(Vector2 sourcePos)
		{
			var sand = 0;
			removeSand();

			while (true)
			{
				var nextPos = getNextSandPosition(sourcePos);
				if (!nextPos.HasValue)
				{
					break;
				}

				cave[(int)nextPos.Value.y][(int)nextPos.Value.x] = TileType.SAND;
				sand++;

				// Sand has reached the top of the cave
				if ((int)nextPos.Value.y == 0)
				{
					break;
				}
			}

			return sand;
		}
	}

	private CaveMap buildMap(string inputText)
	{
		var map = new CaveMap(1000, 1000);

		using (StringReader reader = new StringReader(inputText))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				var rocks = line.Split(" -> ");
				for (int idx = 0; idx < rocks.Length - 1; idx++)
				{
					var a = rocks[idx].Split(',').Select(v => v.ToInt()).Take(2).ToArray();
					var b = rocks[idx + 1].Split(',').Select(v => v.ToInt()).Take(2).ToArray();
					map.AddRocks(new Vector2(a[0], a[1]), new Vector2(b[0], b[1]));
				}
			}
		}

		return map;
	}

	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_14/input.txt");
			var map = buildMap(System.IO.File.ReadAllText(inputPath));
			var result = map.StartSimulation(new Vector2(0, 500));
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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_14/input.txt");
			var map = buildMap(System.IO.File.ReadAllText(inputPath));
			map.AddFloor();
			var result = map.StartSimulation(new Vector2(0, 500));
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
