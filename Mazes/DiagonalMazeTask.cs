<<<<<<< Updated upstream
﻿namespace Mazes;
=======
﻿using System;

namespace Mazes;
>>>>>>> Stashed changes

public static class DiagonalMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
<<<<<<< Updated upstream
        
    }
}
=======
        int minSide = Math.Min(width, height);
        string directionMove = height >= width ? "vertical" : "horizontal";
        var longStep = Math.Max(height, width) / (minSide - 1);
        MoveCycle(robot, minSide, longStep, directionMove);
    }

    public static void MoveCycle(Robot robot, int minSide, int longStep, string dirStep)
    {
        while (!robot.Finished)
        {
            for (int j = 0; j < longStep; j++)
            {
                robot.MoveTo(dirStep == "vertical" ? Direction.Down : Direction.Right);
            }

            if (!robot.Finished)
            {
                robot.MoveTo(dirStep == "vertical" ? Direction.Right : Direction.Down);
            }
        }
    }
}
>>>>>>> Stashed changes
