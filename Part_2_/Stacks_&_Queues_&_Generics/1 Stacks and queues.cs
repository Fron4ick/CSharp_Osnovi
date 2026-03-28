using System;
using System.Collections.Generic;

namespace Stacks_and_queues
{
    class Stack
    {
        List<int> list = new List<int>();
        public void Push(int value)
        {
            list.Add(value);
        }
        public int Pop()
        {
            if (list.Count == 0) throw new InvalidOperationException();
            var result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }
    }
    
    class Queue
    {
        List<int> list = new List<int>();
        public void Enqueue(int value)
        {
            list.Add(value);
        }
        public int Dequeue()
        {
            if (list.Count == 0) throw new InvalidOperationException();
            var result = list[0];
            list.RemoveAt(0); // в этом месте реализация неэффективна,
            // поскольку RemoveAt имеет линейную от размеров листа сложность
            return result;}
    }

    public class Program
    {
        public static void Main()
        {
            var stack = new Stack();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            Console.WriteLine(stack.Pop());
            Console.WriteLine(stack.Pop());
            Console.WriteLine(stack.Pop());

            var queue = new Queue();
            for (int i = 0; i < 3; i++) queue.Enqueue(i);
            for (int i = 0; i < 3; i++) Console.WriteLine(queue.Dequeue());
        }
    }
}