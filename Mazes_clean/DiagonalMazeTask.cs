namespace Mazes;

public static class DiagonalMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        while (robot.Y != height - 2)
        {
            MoveDownRightDown(robot, width - 2);
            MoveLeft(robot);
        }
	}

    public static void MoveDownRightDown(Robot robot, int width, int height)
    {
        var k = (height - 1) / (width - 1) * 1.0;
        var b = 1.0 - k;

        
    }

    public static void MoveLeft(Robot robot)
    {
        while (robot.X != 1)
        {
            robot.MoveTo(Direction.Left);
        }
    }
}
