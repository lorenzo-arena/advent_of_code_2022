using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PuzzleSolver06 : Node
{
	const int START_OF_PACKET_LEN = 4;
	const int START_OF_MESSAGE_LEN = 14;

	private int getMarkerIndex(string text, int markerLen)
	{
		var result = -1;

		for (int idx = 0; (idx < text.Length - markerLen) && (result == -1); idx++)
		{
			var buf = text.ToCharArray().Skip(idx).Take(markerLen).ToArray();
			if (buf.Distinct().Count() == buf.Length)
			{
				result = idx;
			}
		}

		return result;
	}

	// Find the first occurrence of 4 different chars from the input
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_06/input.txt");
			var result = getMarkerIndex(System.IO.File.ReadAllText(inputPath), START_OF_PACKET_LEN) + START_OF_PACKET_LEN;

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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_06/input.txt");
			var result = getMarkerIndex(System.IO.File.ReadAllText(inputPath), START_OF_MESSAGE_LEN) + START_OF_MESSAGE_LEN;

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
