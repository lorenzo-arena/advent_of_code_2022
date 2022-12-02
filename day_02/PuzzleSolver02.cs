using Godot;
using System;

public partial class PuzzleSolver02 : Node
{
	// TODO : refactor to move logic into data

	enum Move
	{
		Rock,
		Paper,
		Scissors
	}

	enum Condition
	{
		Lose,
		Draw,
		Win
	}

	const char rockOpponentChar = 'A';
	const char paperOpponentChar = 'B';
	const char scissorsOpponentChar = 'C';

	const char rockMoveChar = 'X';
	const char paperMoveChar = 'Y';
	const char scissorsMoveChar = 'Z';

	const char loseChar = 'X';
	const char drawChar = 'Y';
	const char winChar = 'Z';

	const int loseScore = 0;
	const int drawScore = 3;
	const int winScore = 6;

	const int rockScore = 1;
	const int paperScore = 2;
	const int scissorsScore = 3;

	private Move charToMove(char moveChar)
	{
		if (moveChar == rockOpponentChar || moveChar == rockMoveChar)
		{
			return Move.Rock;
		}
		else if (moveChar == paperOpponentChar || moveChar == paperMoveChar)
		{
			return Move.Paper;
		}
		else if (moveChar == scissorsOpponentChar || moveChar == scissorsMoveChar)
		{
			return Move.Scissors;
		}

		throw new ArgumentException();
	}

	private Condition charToCondition(char conditionChar)
	{
		if (conditionChar == loseChar)
		{
			return Condition.Lose;
		}
		else if (conditionChar == drawChar)
		{
			return Condition.Draw;
		}
		else if (conditionChar == winChar)
		{
			return Condition.Win;
		}

		throw new ArgumentException();
	}

	private int moveToScore(Move move)
	{
		switch (move)
		{
			case Move.Rock:
			{
				return rockScore;
			}

			case Move.Paper:
			{
				return paperScore;
			}

			case Move.Scissors:
			{
				return scissorsScore;
			}
		}

		throw new ArgumentException();
	}

	private int conditionToScore(Condition condition)
	{
		switch (condition)
		{
			case Condition.Lose:
			{
				return loseScore;
			}

			case Condition.Draw:
			{
				return drawScore;
			}

			case Condition.Win:
			{
				return winScore;
			}
		}

		throw new ArgumentException();
	}

	private int resultToScore(Move myMove, Move opponentMove)
	{
		switch (opponentMove)
		{
			case Move.Rock:
			{
				switch (myMove)
				{
					case Move.Rock:
					{
						return conditionToScore(Condition.Draw);
					}

					case Move.Paper:
					{
						return conditionToScore(Condition.Win);
					}

					case Move.Scissors:
					{
						return conditionToScore(Condition.Lose);
					}
				}
				break;
			}

			case Move.Paper:
			{
				switch (myMove)
				{
					case Move.Rock:
					{
						return conditionToScore(Condition.Lose);
					}

					case Move.Paper:
					{
						return conditionToScore(Condition.Draw);
					}

					case Move.Scissors:
					{
						return conditionToScore(Condition.Win);
					}
				}
				break;
			}

			case Move.Scissors:
			{
				switch (myMove)
				{
					case Move.Rock:
					{
						return conditionToScore(Condition.Win);
					}

					case Move.Paper:
					{
						return conditionToScore(Condition.Lose);
					}

					case Move.Scissors:
					{
						return conditionToScore(Condition.Draw);
					}
				}
				break;
			}
		}

		throw new ArgumentException();
	}

	private int getScore(string movesStr)
	{
		var moves = movesStr.Split(" ");
		var opponentMove = charToMove(moves[0][0]);
		var myMove = charToMove(moves[1][0]);

		return moveToScore(myMove) + resultToScore(myMove, opponentMove);
	}

	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_02/input.txt");
			var score = 0;

			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				if (line != "")
				{
					score += getScore(line);
				}
			}

			GD.Print($"Final score is {score}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	private Move getMoveForCondition(Move move, Condition condition)
	{
		switch (move)
		{
			case Move.Rock:
			{
				switch (condition)
				{
					case Condition.Lose:
					{
						return Move.Scissors;
					}

					case Condition.Draw:
					{
						return Move.Rock;
					}

					case Condition.Win:
					{
						return Move.Paper;
					}
				}
				break;
			}

			case Move.Paper:
			{
				switch (condition)
				{
					case Condition.Lose:
					{
						return Move.Rock;
					}

					case Condition.Draw:
					{
						return Move.Paper;
					}

					case Condition.Win:
					{
						return Move.Scissors;
					}
				}
				break;
			}

			case Move.Scissors:
			{
				switch (condition)
				{
					case Condition.Lose:
					{
						return Move.Paper;
					}

					case Condition.Draw:
					{
						return Move.Scissors;
					}

					case Condition.Win:
					{
						return Move.Rock;
					}
				}
				break;
			}
		}

		throw new ArgumentException();
	}

	private int getScoreOnCondition(string movesStr)
	{
		var moves = movesStr.Split(" ");
		var opponentMove = charToMove(moves[0][0]);
		var condition = charToCondition(moves[1][0]);

		return moveToScore(getMoveForCondition(opponentMove, condition)) + conditionToScore(condition);
	}

	private void solvePart2()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_02/input.txt");
			var score = 0;
			
			foreach (var line in System.IO.File.ReadAllLines(inputPath))
			{
				if (line != "")
				{
					score += getScoreOnCondition(line);
				}
			}

			GD.Print($"Final score for part 2 is {score}");
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
