using System;

namespace Rectangles;

public static class RectanglesTask
{
    // Пересекаются ли два прямоугольника (пересечение только по границе также считается пересечением)
    public static bool AreIntersected(Rectangle r1, Rectangle r2)
    {
        bool xOverlap = Math.Max(r1.Left, r2.Left) <= Math.Min(r1.Right, r2.Right);
        bool yOverlap = Math.Max(r1.Top, r2.Top) <= Math.Min(r1.Bottom, r2.Bottom);

        return xOverlap && yOverlap;
    }

    // Площадь пересечения прямоугольников
    public static int IntersectionSquare(Rectangle r1, Rectangle r2)
    {
        int overlapWidth = Math.Min(r1.Right, r2.Right) - Math.Max(r1.Left, r2.Left);
        int overlapHeight = Math.Min(r1.Bottom, r2.Bottom) - Math.Max(r1.Top, r2.Top);
        int intersectionSquare = overlapWidth * overlapHeight;

        return (overlapWidth <= 0 || overlapHeight <= 0) ? 0 : overlapWidth * overlapHeight;
    }

    // Если один из прямоугольников целиком находится внутри другого — вернуть номер (с нуля) внутреннего.
    // Иначе вернуть -1
    // Если прямоугольники совпадают, можно вернуть номер любого из них.
    public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
    {
        bool r1InR2 = r1.Left >= r2.Left && r1.Right <= r2.Right
                    && r1.Top >= r2.Top && r1.Bottom <= r2.Bottom;
        if (r1InR2) return 0; // r1 in r2

        bool r2InR1 = r2.Left >= r1.Left && r2.Right <= r1.Right
                    && r2.Top >= r1.Top && r2.Bottom <= r1.Bottom;
        if (r2InR1) return 1; // r2 in r1

        return -1;
    }
}
