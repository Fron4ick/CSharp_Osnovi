namespace Mazes;

public static class SnakeMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        while (robot.Y != height - 2)
        {
            MoveDownRightDown(robot, width - 2);
            MoveLeft(robot);
        }
	}

    public static void MoveDownTwo(Robot robot)
    {
        for (int i = 0; i < 2; i++)
        {
            robot.MoveTo(Direction.Down);
        }
    }

    public static void MoveDownRightDown(Robot robot, int width)
    {
        if (robot.Y != 1) MoveDownTwo(robot);
        while (robot.X != width)
        {
            robot.MoveTo(Direction.Right);
        }
        MoveDownTwo(robot);
    }

    public static void MoveLeft(Robot robot)
    {
        while (robot.X != 1)
        {
            robot.MoveTo(Direction.Left);
        }
    }
}
