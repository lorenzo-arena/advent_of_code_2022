using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public class Head
{
	private Vector2 pos = new Vector2(0, 0);

	public Vector2 Position
	{
		get { return pos; }
	}

	public Head()
	{
	}

	public void MoveRight()
	{
		pos.x += 1;
	}

	public void MoveLeft()
	{
		pos.x -= 1;
	}

	public void MoveUp()
	{
		pos.y += 1;
	}

	public void MoveDown()
	{
		pos.y -= 1;
	}
}

public class Tail
{
	private Vector2 pos = new Vector2(0, 0);
	private List<Vector2> history = new List<Vector2>();

	public Vector2 Position
	{
		get { return pos; }
	}

	public List<Vector2> History
	{
		get { return history; }
	}

	public Tail()
	{
		this.history.Add(this.pos);
	}

	public void CheckMove(Vector2 headPos)
	{
		if ((Math.Abs(pos.x - headPos.x) <= 1) && (Math.Abs(pos.y - headPos.y) <= 1))
		{
			return;
		}
		else if (Math.Abs(pos.x - headPos.x) == 0)
		{
			// Same column, move vertically to reach the head
			this.pos.y += (pos.y < headPos.y) ? 1 : -1;
		}
		else if (Math.Abs(pos.y - headPos.y) == 0)
		{
			// Same row, move horizontally to reach the head
			this.pos.x += (pos.x < headPos.x) ? 1 : -1;
		}
		else
		{
			// Move diagonally to keep up
			this.pos.x += (pos.x < headPos.x) ? 1 : -1;
			this.pos.y += (pos.y < headPos.y) ? 1 : -1;
		}

		this.history.Add(this.pos);
	}
}

public class Rope
{
	private Head head = new Head();
	private List<Tail> tails = new List<Tail>();
	private int size;

	public List<Vector2> LastTailHistory
	{
		get { return tails.Last().History; }
	}

	public Rope(int size)
	{
		if (size < 2)
		{
			throw new ArgumentException();
		}

		this.size = size;
		foreach (var _ in Enumerable.Range(0, size - 1))
		{
			this.tails.Add(new Tail());
		}
	}

	private void updateTails()
	{
		tails[0].CheckMove(head.Position);
		foreach (var idx in Enumerable.Range(1, size - 2))
		{
			tails[idx].CheckMove(tails[idx - 1].Position);
		}
	}

	public void MoveRight()
	{
		head.MoveRight();
		updateTails();
	}

	public void MoveLeft()
	{
		head.MoveLeft();
		updateTails();
	}

	public void MoveUp()
	{
		head.MoveUp();
		updateTails();
	}

	public void MoveDown()
	{
		head.MoveDown();
		updateTails();
	}
}

public partial class PuzzleSolver09 : Node
{
	private void processMove(string move, ref Rope rope)
	{
		if (move.StartsWith("R "))
		{
			var size = move.Substring("R ".Length).ToInt();
			foreach (var _ in Enumerable.Range(0, size))
			{
				rope.MoveRight();
			}
		}
		else if (move.StartsWith("L "))
		{
			var size = move.Substring("L ".Length).ToInt();
			foreach (var _ in Enumerable.Range(0, size))
			{
				rope.MoveLeft();
			}
		}
		else if (move.StartsWith("U "))
		{
			var size = move.Substring("U ".Length).ToInt();
			foreach (var _ in Enumerable.Range(0, size))
			{
				rope.MoveUp();
			}
		}
		else if (move.StartsWith("D "))
		{
			var size = move.Substring("D ".Length).ToInt();
			foreach (var _ in Enumerable.Range(0, size))
			{
				rope.MoveDown();
			}
		}
	}

	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_09/input.txt");
			var rope = new Rope(2);

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				processMove(line, ref rope);
			}

			var result = rope.LastTailHistory.Distinct().ToList().Count;
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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_09/input.txt");
			var rope = new Rope(10);

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				processMove(line, ref rope);
			}

			var result = rope.LastTailHistory.Distinct().ToList().Count;
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
