using System;

class Train
{
    public static void Main()
    {
        TestMove(1, 5, 7, 8);
        TestMove(1, 7, 7, 9);
        TestMove(7, 8, 3, 5); ///
        TestMove(7, 8, -3, 100);
        TestMove(6, 15, 9, 12);
    }

    public static void TestMove(int a, int b, int c, int d)
    {
        var (x1, x2) = FindIntersection(a, b, c, d);
        
        if (x1 <= x2)
            Console.WriteLine($"The intersection of segments a,b and c,d belongs to the segment ({x1}, {x2})");
        else
            Console.WriteLine("There is no intersection");
    }

    public static (int, int) FindIntersection(int a, int b, int c, int d)
    {
        int[] array = { a, b, c, d };
        Array.Sort(array);

        if (((array[1] == b && array[2] == c)
        || (array[1] == d && array[2] == a)) 
        && array[1] != array[2])
            return (1, 0);
        else
        {
            return (array[1], array[2]);
        }
    }
}
