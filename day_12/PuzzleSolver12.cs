using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MapNode
{
	public MapNode Parent;
	public Vector2 Position;

	public int Height;
	public double F;

	public MapNode(Vector2 pos, int height)
	{
		Parent = null;
		Position = pos;
		Height = height;
		F = 0;
	}

	public bool CanAccess(MapNode n)
	{
		return Height >= (n.Height - 1);
	}
}

public class Map
{
	private List<List<MapNode>> map;
	private MapNode mapStart;
	private MapNode mapEnd;

	public MapNode MapStart
	{
		get { return mapStart; }
	}

	public MapNode MapEnd
	{
		get { return mapEnd; }
	}

	public int GridCols
	{
		get
		{
			return map[0].Count;
		}
	}

	public int GridRows
	{
		get
		{
			return map.Count;
		}
	}

	public Map(string input)
	{
		this.map = new List<List<MapNode>>();
		this.mapStart = new MapNode(new Vector2(0, 0), 0);
		this.mapEnd = new MapNode(new Vector2(0, 0), 'z' - 'a');

		using (StringReader reader = new StringReader(input))
		{
			string line;
			int y = 0;
			while ((line = reader.ReadLine()) != null)
			{
				int x = 0;
				var row = new List<MapNode>();

				foreach (var cell in line)
				{
					if (cell == 'S')
					{
						this.mapStart = new MapNode(new Vector2(x, y), 0);
						row.Add(this.mapStart);
					}
					else if (cell == 'E')
					{
						this.mapEnd = new MapNode(new Vector2(x, y), 'z' - 'a');
						row.Add(this.mapEnd);
					}
					else
					{
						row.Add(new MapNode(new Vector2(x, y), cell - 'a'));
					}

					x++;
				}

				this.map.Add(row);

				y++;
			}
		}
	}

	private List<MapNode> getNeighbors(MapNode p)
	{
		var list = new List<MapNode>();

		if (p.Position.x > 0)
		{
			var next = map[(int)p.Position.y][(int)p.Position.x - 1];
			if (p.CanAccess(next))
			{
				list.Add(next);
			}
		}

		if (p.Position.y > 0)
		{
			var next = map[(int)p.Position.y - 1][(int)p.Position.x];
			if (p.CanAccess(next))
			{
				list.Add(next);
			}
		}

		if (p.Position.x < (map[(int)p.Position.y].Count - 1))
		{
			var next = map[(int)p.Position.y][(int)p.Position.x + 1];
			if (p.CanAccess(next))
			{
				list.Add(next);
			}
		}

		if (p.Position.y < (map.Count - 1))
		{
			var next = map[(int)p.Position.y + 1][(int)p.Position.x];
			if (p.CanAccess(next))
			{
				list.Add(next);
			}
		}

		return list;
	}

	private void resetNodes()
	{
		for (int y = 0; y < map.Count; y++)
		{
			for (int x = 0; x < map[y].Count; x++)
			{
				map[y][x].F = 0;
				map[y][x].Parent = null;
			}
		}
	}

	public Stack<MapNode> FindPath(MapNode start, MapNode end)
	{
		var path = new Stack<MapNode>();
		var openList = new List<MapNode>();
		var closedList = new List<MapNode>();
		var g = 0;

		resetNodes();

		openList.Add(start);

		while (openList.Count > 0)
		{
			var current = openList.MinBy(n => n.F);
			closedList.Add(current);
			openList.Remove(current);

			g++;

			var neighbors = getNeighbors(current);

			for (int idx = 0; idx < neighbors.Count; idx++)
			{
				if (closedList.Contains(neighbors[idx]))
				{
					continue;
				}

				var neighborF = g +
					Math.Abs(neighbors[idx].Position.x - end.Position.x) +
					Math.Abs(neighbors[idx].Position.y - end.Position.y);
				if (!openList.Contains(neighbors[idx]))
				{
					neighbors[idx].F = neighborF;
					neighbors[idx].Parent = current;

					openList.Add(neighbors[idx]);
				}
				else if (neighborF < neighbors[idx].F)
				{
					neighbors[idx].F = neighborF;
					neighbors[idx].Parent = current;
				}
			}
		}

		if (!closedList.Contains(end))
		{
			return null;
		}
		else
		{
			var temp = end.Parent;
			while (temp != null)
			{
				path.Push(temp);
				temp = temp.Parent;
			}
		}

		return path;
	}

	public List<int> GetPossiblePathsFromHeight(int height)
	{
		var starts = new List<MapNode>();
		var lenghts = new List<int>();

		foreach (var row in map)
		{
			foreach (var node in row)
			{
				if (node.Height <= height)
				{
					starts.Add(node);
				}
			}
		}

		for (int idx = 0; idx < starts.Count; idx++)
		{
			GD.Print($"Processing start node {idx} of {starts.Count}..");
			var path = FindPath(starts[idx], this.mapEnd);

			if (path != null)
			{
				lenghts.Add(path.Count);
			}
		}

		return lenghts;
	}
}

public partial class PuzzleSolver12 : Node
{
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_12/input.txt");
			var map = new Map(System.IO.File.ReadAllText(inputPath));
			var result = map.FindPath(map.MapStart, map.MapEnd).Count;
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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_12/input.txt");
			var map = new Map(System.IO.File.ReadAllText(inputPath));
			var result = map.GetPossiblePathsFromHeight(0).Min();
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
