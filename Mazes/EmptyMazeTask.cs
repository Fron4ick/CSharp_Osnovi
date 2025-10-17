namespace Mazes;

public static class EmptyMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        MoveDown(robot, height - 2);
        MoveRight(robot, width - 2);
	}

    public static void MoveDown(Robot robot, int steps)
    {
        for (int i = 1; i < steps; i++)
        {
            robot.MoveTo(Direction.Down);
        }
    }

    public static void MoveRight(Robot robot, int steps)
    {
        for (int i = 1; i < steps; i++)
        {
            robot.MoveTo(Direction.Right);
        }
    }
}
