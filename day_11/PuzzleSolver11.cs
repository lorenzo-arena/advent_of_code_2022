using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class Monkey
{
	private int id;
	private List<Int64> items;
	private List<Monkey> pack;
	private string operation;
	private Int64 test;
	private int monkeyIDTestTrue;
	private int monkeyIDTestFalse;
	private Int64 inspected;
	private int relief;
	private Int64 modulus;

	public int ID
	{
		get { return id; }
	}

	public List<Monkey> Pack
	{
		set { pack = value; }
	}

	public Int64 Test
	{
		get { return test; }
	}

	public Int64 Inspected
	{
		get { return inspected; }
	}

	public int Relief
	{
		set { relief = value; }
	}

	public Int64 Modulus
	{
		set { modulus = value; }
	}

	public Monkey(int id, List<Int64> items, string operation, Int64 test, int monkeyTrue, int monkeyFalse)
	{
		this.id = id;
		this.items = items;
		this.operation = operation;
		this.test = test;
		this.monkeyIDTestTrue = monkeyTrue;
		this.monkeyIDTestFalse = monkeyFalse;
		this.pack = new List<Monkey>();
		this.inspected = 0;
		this.relief = 1;
	}

	private Int64 operateOnItem(Int64 item)
	{
		var operands = operation.Split(" ");
		Int64 a = (operands[0] == "old") ? item : Int64.Parse(operands[0]);
		Int64 b = (operands[2] == "old") ? item : Int64.Parse(operands[2]);

		switch (operands[1])
		{
		case "*":
			return a * b;
		
		case "+":
			return a + b;
		
		case "/":
			return a / b;

		case "-":
			return a - b;

		default:
			throw new ArgumentException();
		}
	}

	public void PerformTurn()
	{
		for (int idx = 0; idx < items.Count; idx++)
		{
			// Monkey inspects the item, apply the operation
			items[idx] = operateOnItem(items[idx]);

			// Monkey gets bored
			items[idx] /= relief;

			var targetMonkey = pack.Find((monkey) => monkey.id == ((items[idx] % test == 0) ? monkeyIDTestTrue : monkeyIDTestFalse));
			targetMonkey.AddItem(items[idx] % modulus);

			inspected++;
		}

		// All items have been thrown after the turn, empty the list
		items = new List<Int64>();
	}

	public void AddItem(Int64 item)
	{
		items.Add(item);
	}
}

public partial class PuzzleSolver11 : Node
{
	private Monkey stringToMonkey(string input)
	{
		var monkeyRegex = @"Monkey (\d+):\n  Starting items: (.+)\n  Operation: new = (.+)\n  Test: divisible by (\d+)\n    If true: throw to monkey (\d+)\n    If false: throw to monkey (\d+)";
		Regex r = new Regex(monkeyRegex, RegexOptions.IgnoreCase);

		Match m = r.Match(input);
		if (!m.Success)
		{
			throw new FormatException();
		}

		var id = m.Groups[1].Captures[0].Value.ToInt();
		var startingItems = m.Groups[2].Captures[0].Value.Split(", ").Select((value) => Int64.Parse(value)).ToList();
		var operation = m.Groups[3].Captures[0].Value;
		var test = m.Groups[4].Captures[0].Value.ToInt();
		var monkeyTrue = m.Groups[5].Captures[0].Value.ToInt();
		var monkeyFalse = m.Groups[6].Captures[0].Value.ToInt();

		return new Monkey(id, startingItems, operation, test, monkeyTrue, monkeyFalse);
	}

	private void solvePart1()
	{
		try
		{
			var pack = new List<Monkey>();
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_11/input.txt");
			var inputText = System.IO.File.ReadAllText(inputPath);
			var splitInput = inputText.Split("\n\n");
			Int64 testsCommon = 1;

			foreach (var monkeyText in splitInput)
			{
				var monkey = stringToMonkey(monkeyText);
				monkey.Relief = 3;
				pack.Add(monkey);
				testsCommon *= monkey.Test;
			}

			foreach (var monkey in pack)
			{
				monkey.Pack = pack;
				monkey.Modulus = testsCommon;
			}

			foreach (var _ in Enumerable.Range(0, 20))
			{
				foreach (var monkey in pack)
				{
					monkey.PerformTurn();
				}
			}

			pack.Sort((x, y) => x.Inspected.CompareTo(y.Inspected));
			var result = pack[pack.Count - 1].Inspected * pack[pack.Count - 2].Inspected;
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
			var pack = new List<Monkey>();
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_11/input.txt");
			var inputText = System.IO.File.ReadAllText(inputPath);
			var splitInput = inputText.Split("\n\n");
			Int64 testsCommon = 1;

			foreach (var monkeyText in splitInput)
			{
				var monkey = stringToMonkey(monkeyText);
				pack.Add(monkey);
				testsCommon *= monkey.Test;
			}

			foreach (var monkey in pack)
			{
				monkey.Pack = pack;
				monkey.Modulus = testsCommon;
			}

			foreach (var _ in Enumerable.Range(0, 10000))
			{
				foreach (var monkey in pack)
				{
					monkey.PerformTurn();
				}
			}

			pack.Sort((x, y) => x.Inspected.CompareTo(y.Inspected));
			var result = pack[pack.Count - 1].Inspected * pack[pack.Count - 2].Inspected;
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
