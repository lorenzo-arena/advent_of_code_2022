using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class ListExtended
{
	public List<object> List;
	public ListExtended Parent;

	public ListExtended()
	{
		List = new List<object>();
		Parent = null;
	}
}

public class ListParser
{
	public static List<object> Parse(string input)
	{
		if (!input.StartsWith("["))
		{
			throw new ArgumentException();
		}

		var tmpInput = input.Substring(1);
		var list = new ListExtended();
		var currList = list;
		string numberRegex = @"^(\d+).*";
		Regex r = new Regex(numberRegex, RegexOptions.IgnoreCase);

		while (true)
		{
			// Check if we're starting a new list or closing the current one
			if (tmpInput.StartsWith("["))
			{
				var newList = new ListExtended();
				newList.Parent = currList;
				currList = newList;
				tmpInput = tmpInput.Substring(1);
				continue;
			}
			else if (tmpInput.StartsWith("]"))
			{
				if (currList.Parent != null)
				{
					currList.Parent.List.Add(currList.List);
					currList = currList.Parent;
					tmpInput = tmpInput.Substring(1);
					tmpInput = tmpInput.StartsWith(',') ? tmpInput.Substring(1) : tmpInput;
					continue;
				}
				else
				{
					// The primary list is being closed
					break;
				}
			}

			// A number should match at this point
			Match m = r.Match(tmpInput);
			if (!m.Success)
			{
				throw new ArgumentException();
			}

			currList.List.Add(m.Groups[1].Captures[0].Value.ToInt());
			tmpInput = tmpInput.Substring(m.Groups[1].Captures[0].Value.Length);
			tmpInput = tmpInput.StartsWith(',') ? tmpInput.Substring(1) : tmpInput;
		}

		return list.List;
	}
}

public partial class PuzzleSolver13 : Node
{
	private enum CheckResult
	{
		CHECK_SUCCESS,
		CHECK_TIE,
		CHECK_FAIL
	};

	private CheckResult checkNumbers(int first, int second)
	{
		if (first > second)
		{
			return CheckResult.CHECK_FAIL;
		}
		else if (first == second)
		{
			return CheckResult.CHECK_TIE;
		}

		return CheckResult.CHECK_SUCCESS;
	}

	private CheckResult checkOrder(List<object> first, List<object> second)
	{
		for (int idx = 0; idx < Math.Min(first.Count, second.Count); idx++)
		{
			var checkRes = CheckResult.CHECK_FAIL;

			if (first[idx] is Int32 && second[idx] is Int32)
			{
				// If both value are integers, the lower integer should come first
				checkRes = checkNumbers((int)first[idx], (int)second[idx]);
			}
			else if (first[idx] is List<object> && second[idx] is List<object>)
			{
				// If both values are lists, compare the first value of each list and so on
				checkRes = checkOrder((List<object>)first[idx], (List<object>)second[idx]);
			}
			else if (first[idx] is Int32 && second[idx] is List<object>)
			{
				// If exactly one value is an integer convert the integer to a list which contains
				// that integer as its only value and retry the comparison
				var tempList = new List<object>();
				tempList.Add(first[idx]);
				checkRes = checkOrder(tempList, (List<object>)second[idx]);
			}
			else if (first[idx] is List<object> && second[idx] is Int32)
			{
				// If exactly one value is an integer convert the integer to a list which contains
				// that integer as its only value and retry the comparison
				var tempList = new List<object>();
				tempList.Add(second[idx]);
				checkRes = checkOrder((List<object>)first[idx], tempList);
			}

			switch (checkRes)
			{
			case CheckResult.CHECK_SUCCESS:
				return CheckResult.CHECK_SUCCESS;

			case CheckResult.CHECK_TIE:
				continue;

			case CheckResult.CHECK_FAIL:
				return CheckResult.CHECK_FAIL;
			}
		}

		if (first.Count < second.Count)
		{
			return CheckResult.CHECK_SUCCESS;
		}
		else if (first.Count == second.Count)
		{
			return CheckResult.CHECK_TIE;
		}

		return CheckResult.CHECK_FAIL;
	}

	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_13/input.txt");
			var inputLists = System.IO.File.ReadAllText(inputPath).Split("\n\n");
			var rightOrder = new List<int>();

			for (int idx = 0; idx < inputLists.Length; idx++)
			{
				var lists = inputLists[idx].Split("\n");
				var firstList = ListParser.Parse(lists[0]);
				var secondList = ListParser.Parse(lists[1]);
				var res = checkOrder(firstList, secondList);
				if (res == CheckResult.CHECK_SUCCESS)
				{
					rightOrder.Add(idx + 1);
				}
			}

			var result = rightOrder.Aggregate(0, (acc, value) => acc + value);
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
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_13/input.txt");
			var inputLists = System.IO.File.ReadAllText(inputPath).Split("\n\n");
			var parsedLists = new List<List<object>>();

			foreach (var splitLists in inputLists)
			{
				parsedLists.Add(ListParser.Parse(splitLists.Split("\n")[0]));
				parsedLists.Add(ListParser.Parse(splitLists.Split("\n")[1]));
			}

			var firstSeparator = new List<object>();
			var firstSeparatorContent = new List<object>();
			firstSeparatorContent.Add(2);
			firstSeparator.Add(firstSeparatorContent);

			var secondSeparator = new List<object>();
			var secondSeparatorContent = new List<object>();
			secondSeparatorContent.Add(6);
			secondSeparator.Add(secondSeparatorContent);

			parsedLists.Add(firstSeparator);
			parsedLists.Add(secondSeparator);

			parsedLists.Sort((a, b) => {
				switch (checkOrder(a, b))
				{
				case CheckResult.CHECK_SUCCESS:
					return -1;
				
				case CheckResult.CHECK_TIE:
					return 0;

				case CheckResult.CHECK_FAIL:
				default:
					return 1;
				}
			});

			var result = (parsedLists.IndexOf(firstSeparator) + 1) * (parsedLists.IndexOf(secondSeparator) + 1);
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
