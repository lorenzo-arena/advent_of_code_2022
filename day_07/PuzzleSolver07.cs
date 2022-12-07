using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class Folder
{
#nullable enable
	private Folder? parent = null;
	private List<Folder> children = new List<Folder>();
	private int space = 0;
	private string name = "";

	public Folder? Parent
	{
		get { return parent; }
	}

	public List<Folder> Children
	{
		get { return children; }
	}

	public int Space
	{
		get { return space; }
	}

	public string Name
	{
		get { return name; }
	}

	public Folder(string name, Folder? parent)
	{
		this.name = name;
		this.parent = parent;
	}

	public void AddChild(Folder folder)
	{
		this.children.Add(folder);
	}

	public void IncreaseSpace(int space)
	{
		this.space += space;
		if (parent != null)
		{
			this.parent.IncreaseSpace(space);
		}
	}
}

public partial class PuzzleSolver07 : Node
{
	const string cdCmdStarter = "$ cd ";
	const string lsCmdStarter = "$ cd ";

	private Folder? buildDirectoryTree(string listingPath)
	{
		Folder? currentFolder = null;
		Folder? rootFolder = null;
		Regex fileSizeRegex = new Regex(@"(\d+) .+", RegexOptions.IgnoreCase);

		foreach (var line in System.IO.File.ReadAllLines(listingPath))
		{
			if (line.StartsWith(cdCmdStarter))
			{
				var folderName = line.Substring(cdCmdStarter.Length);

				if (folderName == "..")
				{
					if (currentFolder == null)
					{
						throw new ArgumentNullException();
					}

					currentFolder = currentFolder.Parent;
				}
				else
				{
					var newFolder = new Folder(folderName, currentFolder);

					if (currentFolder == null)
					{
						rootFolder = newFolder;
					}
					else
					{
						currentFolder.AddChild(newFolder);
					}

					currentFolder = newFolder;
				}
			}
			else if (line.StartsWith(lsCmdStarter))
			{
				// Do nothing for now
			}
			else
			{
				Match m = fileSizeRegex.Match(line);
				if (m.Success && currentFolder != null)
				{
					currentFolder.IncreaseSpace(m.Groups[1].Captures[0].Value.ToInt());
				}
			}
		}

		return rootFolder;
	}

	private void getFoldersWithSizeLessThan(int size, Folder folder, ref List<Folder> folders)
	{
		if (folder.Space <= size)
		{
			folders.Add(folder);
		}

		foreach (var child in folder.Children)
		{
			getFoldersWithSizeLessThan(size, child, ref folders);
		}
	}

	private void getFoldersWithSizeMoreThan(int size, Folder folder, ref List<Folder> folders)
	{
		if (folder.Space >= size)
		{
			folders.Add(folder);
		}

		foreach (var child in folder.Children)
		{
			getFoldersWithSizeMoreThan(size, child, ref folders);
		}
	}

	// Parse the command listing, compute the size of each directory; find all the directories
	// with a total size of at most 100k and calculate the sum of their total sizes
	private void solvePart1()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_07/input.txt");
			var result = 0;
			var rootFolder = buildDirectoryTree(inputPath);

			if (rootFolder == null)
			{
				throw new ArgumentNullException();
			}

			var targetFolders = new List<Folder>();
			getFoldersWithSizeLessThan(100000, rootFolder, ref targetFolders);

			result = targetFolders.Select(folder => folder.Space).Aggregate(0, (acc, space) => acc + space);

			GD.Print($"Part 1 result is {result}");
		}
		catch (Exception e)
		{
			GD.Print(e.ToString());
		}
	}

	const int TOTAL_DISK_SPACE = 70000000;
	const int UPDATE_DISK_SPACE = 30000000;

	// Parse the listing and find the directories which would reduce the used space down to
	// something that will allow updating; search for the smallest folder of those
	private void solvePart2()
	{
		try
		{
			var inputPath = Godot.ProjectSettings.GlobalizePath("res://day_07/input.txt");
			var result = 0;
			var rootFolder = buildDirectoryTree(inputPath);

			if (rootFolder == null)
			{
				throw new ArgumentNullException();
			}

			var occupiedSpace = rootFolder.Space;
			var targetFolders = new List<Folder>();
			getFoldersWithSizeMoreThan(occupiedSpace + UPDATE_DISK_SPACE - TOTAL_DISK_SPACE,
				rootFolder, ref targetFolders);

			result = targetFolders.Min(folder => folder.Space);

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
#nullable disable
