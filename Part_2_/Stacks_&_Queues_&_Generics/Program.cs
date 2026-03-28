using System;

namespace MagneticMaze;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        using var game = new Core.GameManager();
        game.Run();
    }
}

