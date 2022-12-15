using Godot;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Utils;
using System.Numerics;

public partial class PuzzleSolver15 : Node
{
	private class SensorBeacon
	{
		Point SensorPos;
		Point BeaconPos;

		public SensorBeacon(Point sensorPos, Point beaconPos)
		{
			this.SensorPos = sensorPos;
			this.BeaconPos = beaconPos;
		}

		public List<Point> GetCoveredPositionsInRow(bool includeSensor, bool includeBeacon, int row)
		{
			var list = new List<Point>();
			var beaconDist = Math.Abs(SensorPos.x - BeaconPos.x) + Math.Abs(SensorPos.y - BeaconPos.y);
			var rowDist = Math.Abs(SensorPos.y - row);

			if (rowDist > beaconDist)
			{
				return list;
			}

			var size = beaconDist - rowDist;
			for (int x = -size; x <= size; x++)
			{
				int realX = SensorPos.x + x;

				if ((realX == SensorPos.x) && (row == SensorPos.y) && !includeSensor)
				{
					continue;
				}

				if ((realX == BeaconPos.x) && (row == BeaconPos.y) && !includeBeacon)
				{
					continue;
				}

				list.Add(new Point(realX, row));
			}

			return list;
		}

		public bool IsPositionCovered(Point pos)
		{
			var beaconDist = Math.Abs(SensorPos.x - BeaconPos.x) + Math.Abs(SensorPos.y - BeaconPos.y);
			var posDist = Math.Abs(SensorPos.x - pos.x) + Math.Abs(SensorPos.y - pos.y);
			return posDist <= beaconDist;
		}

		private bool checkBoundaries(Point p, int xLowerLimit, int xUpperLimit, int yLowerLimit, int yUpperLimit)
		{
			return (p.x >= xLowerLimit) && (p.x <= xUpperLimit) &&
			       (p.y >= yLowerLimit) && (p.y <= yUpperLimit);
		}

		public List<Point> GetOuterBorders(int xLowerLimit, int xUpperLimit, int yLowerLimit, int yUpperLimit)
		{
			var list = new List<Point>();
			var beaconDist = Math.Abs(SensorPos.x - BeaconPos.x) + Math.Abs(SensorPos.y - BeaconPos.y);
			var boundaryDist = beaconDist + 1;
			for (int x = -boundaryDist; x <= boundaryDist; x++)
			{
				var points = new List<Point>();

				// Only a single point exists on the boundary extreme
				if (Math.Abs(x) == boundaryDist)
				{
					points.Add(new Point(SensorPos.x + x, SensorPos.y));
				}
				else
				{
					points.Add(new Point(SensorPos.x + x, SensorPos.y + (boundaryDist - Math.Abs(x))));
					points.Add(new Point(SensorPos.x + x, SensorPos.y - (boundaryDist - Math.Abs(x))));
				}

				foreach (var point in points)
				{
					if (checkBoundaries(point, xLowerLimit, xUpperLimit, yLowerLimit, yUpperLimit))
					{
						list.Add(point);
					}
				}
			}

			return list;
		}
	}

	private List<SensorBeacon> parseInput(string inputText)
	{
		var list = new List<SensorBeacon>();
		var sensorsRegex = @"^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$";
		Regex r = new Regex(sensorsRegex, RegexOptions.IgnoreCase);

		using (StringReader reader = new StringReader(inputText))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				Match m = r.Match(line);
				if (!m.Success)
				{
					throw new ArgumentException();
				}

				var sensorPos = new Point(m.Groups[1].Captures[0].Value.ToInt(), m.Groups[2].Captures[0].Value.ToInt());
				var beaconPos = new Point(m.Groups[3].Captures[0].Value.ToInt(), m.Groups[4].Captures[0].Value.ToInt());
				var sensor = new SensorBeacon(sensorPos, beaconPos);
				list.Add(sensor);
			}
		}

		return list;
	}

	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_15/input.txt");
			var sensors = parseInput(System.IO.File.ReadAllText(inputPath));
			var coveredPositions = new List<Point>();
			//var rowIdx = 10;
			var rowIdx = 2000000;
			foreach (var sensor in sensors)
			{
				foreach (var pos in sensor.GetCoveredPositionsInRow(false, false, rowIdx))
				{
					coveredPositions.Add(pos);
				}
			}
			var result = coveredPositions.Distinct().ToList().Count;
			GD.Print($"Part 1 result is {result}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	private Point? getBeaconPos(List<SensorBeacon> sensors, int maxBound)
	{
		// Here the idea is, instead of checking each cell inside the bounds, to check (for each sensor)
		// the cells which are immediately outside of its reach but inside the bounds. For each point,
		// check if it is in reach of all the other sensors
		for (int idx = 0; idx < sensors.Count; idx++)
		{
			var borders = sensors[idx].GetOuterBorders(0, maxBound, 0, maxBound);

			foreach (var border in borders)
			{
				var covered = false;
				for (int checkIdx = 0; (checkIdx < sensors.Count) && !covered; checkIdx++)
				{
					if ((checkIdx != idx) && sensors[checkIdx].IsPositionCovered(border))
					{
						covered = true;
					}
				}

				if (!covered)
				{
					return new Point(border.x, border.y);
				}
			}
		}

		return null;
	}

	private void solvePart2()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_15/input.txt");
			var sensors = parseInput(System.IO.File.ReadAllText(inputPath));
			//var pos = getBeaconPos(sensors, 20);
			var pos = getBeaconPos(sensors, 4000000);
			BigInteger tuningFrequency = pos.Value.x;
			tuningFrequency *= 4000000;
			tuningFrequency += pos.Value.y;
			GD.Print($"Part 2 result is {tuningFrequency}");
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
