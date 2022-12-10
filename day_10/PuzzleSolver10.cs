using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class CPUEmulator
{
	private int reg_x = 1;
	private int cycles = 0;
	private List<int> strengths = new List<int>();
	private string crt = "";

	public List<int> Strengths
	{
		get { return strengths; }
	}

	public string CRT
	{
		get { return crt; }
	}

	public CPUEmulator()
	{
	}

	private void addCycle()
	{
		// Draw on the CRT before incrementing, since positions in the CRT are 0-based
		crt += (Math.Abs((cycles % 40) - reg_x) > 1) ? "." : "#";
		crt += ((cycles + 1) % 40 == 0) ? "\n" : "";

		cycles++;

		// If necessary, update the strength list
		if ((cycles - 20) % 40 == 0)
		{
			strengths.Add(cycles * reg_x);
		}
	}

	public void Process(string instr)
	{
		if (instr == "noop")
		{
			addCycle();
		}
		else if (instr.StartsWith("addx "))
		{
			var add = instr.Substring("addx ".Length).ToInt();
			addCycle();
			addCycle();
			reg_x += add;
		}
		else
		{
			throw new ArgumentException();
		}
	}
}

public partial class PuzzleSolver10 : Node
{
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_10/input.txt");
			var cpu = new CPUEmulator();

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				cpu.Process(line);
			}

			var result = cpu.Strengths.Aggregate(0, (acc, strength) => acc + strength);
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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_10/input.txt");
			var cpu = new CPUEmulator();

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				cpu.Process(line);
			}

			GD.Print($"Part 2 result is:");
			GD.Print("");
			GD.Print($"{cpu.CRT}");
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
