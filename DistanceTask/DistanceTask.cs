using System;

namespace DistanceTask;

public static class DistanceTask
{
    public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
    {
        double abx = bx - ax, aby = by - ay;
        double aPx = x - ax,  aPy = y - ay;

        double abLen2 = abx * abx + aby * aby;
        if (abLen2 == 0) 
            return Math.Sqrt(aPx * aPx + aPy * aPy);

        double t = (aPx * abx + aPy * aby) / abLen2;
        if (t < 0) t = 0;
        else if (t > 1) t = 1;

        double nx = ax + t * abx;
        double ny = ay + t * aby;

        double dx = x - nx, dy = y - ny;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
