using System;
using System.Drawing;

namespace Fractals;

internal static class DragonFractalTask
{
    public static void DrawDragonFractal(Pixels pixels, int iterationsCount, int seed)
    {
        var random = new Random(seed);
        double x = 1, y = 0;

        pixels.SetPixel(x, y);
        GenFracPoint(pixels, random, iterationsCount, ref x, ref y);
    }
    
    private static void GenFracPoint(Pixels pixels, Random random, int iterationsCount, ref double x, ref double y)
    {
        for (int i = 0; i < iterationsCount; i++)
        {
            TransformPoint(random, ref x, ref y);
            pixels.SetPixel(x, y);
        }
    }

    private static void TransformPoint(Random random, ref double x, ref double y)
    {
        double newX, newY;
        
        if (random.Next(2) == 0) ApplyFirstTransformation(x, y, out newX, out newY);
        else ApplySecondTransformation(x, y, out newX, out newY);

        x = newX;
        y = newY;
    }

    private static void ApplyFirstTransformation(double x, double y, out double newX, out double newY)
    {
        newX = (x - y) / 2.0;
        newY = (x + y) / 2.0;
    }

    private static void ApplySecondTransformation(double x, double y, out double newX, out double newY)
    {
        newX = (-x - y) / 2.0 + 1;
        newY = (x - y) / 2.0;
    }
}
