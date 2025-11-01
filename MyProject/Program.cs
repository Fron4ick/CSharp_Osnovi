using System;

namespace Summator
{
    class Summator
    {
        static void Main()
        {
            int sum = 0;
            for (int i = int.Parse(Console.ReadLine()); i != 0; i = int.Parse(Console.ReadLine()))
            {
                sum += i;
            }
            Console.WriteLine(sum);
        }
    }
}
