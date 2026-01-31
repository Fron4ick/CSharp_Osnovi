using System;
using System.Collections.Generic;
using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

public class Terrain : ICreature
{
    public string GetImageFileName() => "Terrain.png";
    public int GetDrawingPriority() => 0;
    public CreatureCommand Act(int x, int y) => new CreatureCommand();
    public bool DeadInConflict(ICreature conflictedObject) => true;
}

public class Player : ICreature
{
    public string GetImageFileName() => "Player.png";
    public int GetDrawingPriority() => 1;

    public CreatureCommand Act(int x, int y)
    {
        var command = GetCommandFromKey();
        
        var targetX = x + command.DeltaX;
        var targetY = y + command.DeltaY;

        if (!IsWithinBounds(targetX, targetY) || !CanMoveTo(targetX, targetY))
        {
            return new CreatureCommand();
        }

        return command;
    }

    private CreatureCommand GetCommandFromKey()
    {
        return Game.KeyPressed switch
        {
            Key.Left => new CreatureCommand { DeltaX = -1 },
            Key.Right => new CreatureCommand { DeltaX = 1 },
            Key.Up => new CreatureCommand { DeltaY = -1 },
            Key.Down => new CreatureCommand { DeltaY = 1 },
            _ => new CreatureCommand()
        };
    }

    private bool IsWithinBounds(int x, int y) 
        => x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;

    private bool CanMoveTo(int x, int y)
    {
        var target = Game.Map[x, y];
        
        if (target is Sack) return false;
        
        if (target is Gold) Game.Scores += 10;
        
        return true;
    }

    public bool DeadInConflict(ICreature conflictedObject) => conflictedObject is Sack;
}

public class Sack : ICreature
{
    private int fallDistance = 0;

    public string GetImageFileName() => "Sack.png";
    public int GetDrawingPriority() => 2;

    public CreatureCommand Act(int x, int y)
    {
        if (CanFallDown(x, y))
        {
            fallDistance++;
            return new CreatureCommand { DeltaY = 1 };
        }

        return TryTransformToGold();
    }

    private bool CanFallDown(int x, int y)
    {
        var targetY = y + 1;
        if (targetY >= Game.MapHeight) return false;

        var target = Game.Map[x, targetY];
        
        return target == null || (target is Player && fallDistance > 0);
    }

    private CreatureCommand TryTransformToGold()
    {
        var command = new CreatureCommand 
        { 
            TransformTo = fallDistance > 1 ? new Gold() : null 
        };
        
        fallDistance = 0;
        return command;
    }

    public bool DeadInConflict(ICreature conflictedObject) => false;
}

public class Gold : ICreature
{
    public string GetImageFileName() => "Gold.png";
    public int GetDrawingPriority() => 3;
    public CreatureCommand Act(int x, int y) => new();
    public bool DeadInConflict(ICreature conflictedObject) => true;
}