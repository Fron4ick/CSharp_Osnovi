using System;
using System.Collections.Generic;
using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

public class Terrain : ICreature
{
	public string GetImageFileName() => "Terrain.png";
	
	public int GetDrawingPriority() => 0;
	
	public CreatureCommand Act(int x, int y)
	{
		return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
	}
	
	public bool DeadInConflict(ICreature conflictedObject) => true;
}

public class Player : ICreature
{
	public string GetImageFileName() => "Player.png";
	
	public int GetDrawingPriority() => 1;
	
	public CreatureCommand Act(int x, int y)
	{
		var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };
		
		switch (Game.KeyPressed)
		{
			case Key.Left:
				if (x > 0) command.DeltaX = -1;
				break;
			case Key.Right:
				if (x < Game.MapWidth - 1) command.DeltaX = 1;
				break;
			case Key.Up:
				if (y > 0) command.DeltaY = -1;
				break;
			case Key.Down:
				if (y < Game.MapHeight - 1) command.DeltaY = 1;
				break;
		}
		
		var targetX = x + command.DeltaX;
		var targetY = y + command.DeltaY;
		var targetCreature = Game.Map[targetX, targetY];
		
		if (targetCreature is Terrain)
		{
			command.TransformTo = null;
		}
		else if (targetCreature is Gold)
		{
			Game.Scores += 10;
			command.TransformTo = null;
		}
		else if (targetCreature is Sack)
		{
			command.DeltaX = 0;
			command.DeltaY = 0;
		}
		
		return command;
	}
	
	public bool DeadInConflict(ICreature conflictedObject) => conflictedObject is Sack;
}

public class Sack : ICreature
{
	private static Dictionary<Sack, int> fallDistances = new();
	
	public string GetImageFileName() => "Sack.png";
	
	public int GetDrawingPriority() => 2;
	
	public CreatureCommand Act(int x, int y)
	{
		var command = new CreatureCommand { DeltaX = 0, DeltaY = 0 };
		
		if (!fallDistances.ContainsKey(this))
			fallDistances[this] = 0;
		
		if (y < Game.MapHeight - 1)
		{
			var below = Game.Map[x, y + 1];
			if (below == null)
			{
				command.DeltaY = 1;
				fallDistances[this]++;
			}
			else if (below is Player)
			{
				command.DeltaY = 1;
				fallDistances[this]++;
			}
			else
			{
				if (fallDistances[this] > 1)
				{
					command.TransformTo = new Gold();
				}
				fallDistances.Remove(this);
			}
		}
		else
		{
			if (fallDistances[this] > 1)
			{
				command.TransformTo = new Gold();
			}
			fallDistances.Remove(this);
		}
		
		return command;
	}
	
	public bool DeadInConflict(ICreature conflictedObject) => false;
}

public class Gold : ICreature
{
	public string GetImageFileName() => "Gold.png";
	
	public int GetDrawingPriority() => 2;
	
	public CreatureCommand Act(int x, int y)
	{
		return new CreatureCommand { DeltaX = 0, DeltaY = 0 };
	}
	
	public bool DeadInConflict(ICreature conflictedObject) => false;
}