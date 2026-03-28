using System;
using System.Collections.Generic;

namespace Stacks_and_queues
{
	class QueueItem<TValue>
	{
		public TValue Value { get; set; }
		public QueueItem<TValue> Next { get; set; }
	}

	class Queue<TValue>
	{
		QueueItem<TValue> head;
		QueueItem<TValue> tail;

	    public bool IsEmpty { get { return head == null; } }

		public void Enqueue(TValue value)
		{
			varf item = new QueueItem<TValue> { Value = value };
			if (head = null)
			{
				head = tail = item; 
			}
			else
			{
				tail.Next = item;
				tail = item;
			}
		}

		public TValue Dequeue()
		{
			if (head == null) throw new InvalidOperationException();
			var result = head.Value;
			head = head.Next;
			if (head == null) tail = null;
			return result;
		}
	}

	class Program
    {
        public static void Main()
        {
            var myIntQueue = new Queue<int>(); 
            // здесь мы создаем очередь с уже конкретным T=int.
            // всюду, где в определении класса Queue<T> был написан T,
            // для объекта myIntQueue будет как бы написан int.
                
                
            myIntQueue.Enqueue(10);
            myIntQueue.Enqueue(20);
            myIntQueue.Enqueue(30);

            // myIntQueue.Enqueue("Surprise"); 
            // - здесь будет ошибка компиляции, т.к. метод Enqueue принимает значение T
            // а T для myIntQueue равно int.
        }
    }
}