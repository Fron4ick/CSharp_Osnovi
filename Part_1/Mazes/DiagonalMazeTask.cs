using System;

namespace Mazes;

public static class DiagonalMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        var isVerticalPrimary = height >= width;
        var primaryDirection = isVerticalPrimary ? Direction.Down : Direction.Right;
        var secondaryDirection = isVerticalPrimary ? Direction.Right : Direction.Down;
        
        var longStep = Math.Max(height, width) / (Math.Min(width, height) - 1);
        
        MoveInDiagonalPattern(robot, longStep, primaryDirection, secondaryDirection);
    }

    private static void MoveInDiagonalPattern(Robot robot, int longStep, 
        Direction primary, Direction secondary)
    {
        while (!robot.Finished)
        {
            MoveMultipleSteps(robot, longStep, primary);
            if (!robot.Finished) robot.MoveTo(secondary);
        }
    }

    private static void MoveMultipleSteps(Robot robot, int steps, Direction direction)
    {
        for (int step = 0; step < steps && !robot.Finished; step++)
            robot.MoveTo(direction);
    }
}
