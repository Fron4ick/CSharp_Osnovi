using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IEnumeratorLazinessTests
{
    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("ТЕСТ 1: Проверка метода WithoutDeferredExecution");
            TestWithoutDeferredExecution();
            
            Console.WriteLine("ТЕСТ 2: Проверка метода WithDeferredExecution");
            TestWithDeferredExecution();
            
            Console.WriteLine("ТЕСТ 3: Проверка метода DoubleEnumeration");
            TestDoubleEnumeration();
        }

        #region

        /// ПЛОХОЙ метод: не ленивый и с двойным перечислением
        /// 
        static IEnumerable<int> BadMethod(IEnumerable<int> source)
        {
            var list = source.ToList();
            
            Console.WriteLine("BadMethod: вычисляю результаты...");

            for (int i = 0; i < list.Count; i++) // Каждый foreach = новый for
            {
                yield return list[i] * 2;
            }
        }

        /// ХОРОШИЙ метод: ленивый и с однократным перечислением
        static IEnumerable<int> GoodMethod(IEnumerable<int> source)
        {
            Console.WriteLine("GoodMethod: начал работу (но это выполнится ТОЛЬКО при первом foreach)");

            foreach (var item in source) // source будет перечислен ТОЛЬКО во время foreach
            {
                Console.WriteLine($"GoodMethod: обрабатываю элемент {item}");
                yield return item * 2;
            }
            
            Console.WriteLine("GoodMethod: закончил работу");
        }
        
        #endregion

        #region проверка ленивости
        
        static void TestWithoutDeferredExecution()
        {
            int[] source = { 1, 2, 3 };
            
            var countedSource = new CountingEnumerable<int>(source);
            
            // вызов бэдметода
            var result = BadMethod(countedSource); 
            
            Console.WriteLine($"   countedSource был перечислен {countedSource.EnumerationCount} раз через BadMethod");
            
            Console.WriteLine("\nТОЛЬКО СЕЙЧАС начинаем первый foreach");
            foreach (var item in result)
            {
                Console.WriteLine($"   получил: {item}");
            }
            
            Console.WriteLine($"\n6. ИТОГ: источник перечислен {countedSource.EnumerationCount} раз (BadMethod перечислил его при вызове, а не при итерации)");
        }

        static void TestWithDeferredExecution()
        {
            Console.WriteLine("1. СОЗДАЮ источник данных (массив)");
            int[] source = { 1, 2, 3 };
            
            Console.WriteLine("2. СОЗДАЮ счетчик обращений к источнику");
            var countedSource = new CountingEnumerable<int>(source);
            
            Console.WriteLine("3. ВЫЗЫВАЮ GoodMethod (ХОРОШИЙ метод)");
            // !!! КЛЮЧЕВОЙ МОМЕНТ !!! GoodMethod НЕ выполняет код сейчас
            // Он только возвращает итератор (state machine), но не запускает его
            var result = GoodMethod(countedSource);
            
            Console.WriteLine("4. GoodMethod НЕ ВЫПОЛНИЛСЯ (код отложен)");
            Console.WriteLine($"   countedSource еще НЕ перечислен: {countedSource.EnumerationCount} раз");
            
            Console.WriteLine("\n5. ТОЛЬКО СЕЙЧАС начинаем первый foreach - код НАЧИНАЕТ выполняться");
            foreach (var item in result)
            {
                Console.WriteLine($"   получил: {item}");
            }
            
            Console.WriteLine($"\n6. ИТОГ: источник перечислен ровно 1 раз (во время foreach)");
        }
        
        #endregion

        #region ========== ТЕСТ 2: ПРОВЕРКА ДВОЙНОГО ПЕРЕЧИСЛЕНИЯ ==========
        
        /// <summary>
        /// Метод с проблемой двойного перечисления
        /// </summary>
        static IEnumerable<int> MethodWithDoubleEnumeration(IEnumerable<int> source)
        {
            // ПРОБЛЕМА: мы перечисляем source дважды
            // 1-й раз: для подсчета суммы
            int sum = source.Sum(); // <-- ПЕРВОЕ ПЕРЕЧИСЛЕНИЕ
            
            // 2-й раз: для возврата результатов
            foreach (var item in source) // <-- ВТОРОЕ ПЕРЕЧИСЛЕНИЕ
            {
                yield return item * sum;
            }
        }

        /// <summary>
        /// Исправленный метод (однократное перечисление)
        /// </summary>
        static IEnumerable<int> FixedMethod(IEnumerable<int> source)
        {
            // ПРАВИЛЬНО: перечисляем source ТОЛЬКО ОДИН РАЗ
            List<int>? cachedItems = null; // Будем кэшировать при первом перечислении
            
            foreach (var item in source) // ЕДИНСТВЕННОЕ перечисление
            {
                if (cachedItems == null)
                {
                    cachedItems = new List<int>(); // Создаем кэш при первом элементе
                }
                
                cachedItems.Add(item);
                // Можем сразу yield return, если логика не требует подсчета суммы
                // Но в нашем случае сумма нужна ДО обработки элементов
            }
            
            // Теперь у нас есть сумма и кэшированные данные
            if (cachedItems != null)
            {
                int sum = cachedItems.Sum(); // Считаем сумму по кэшу (нет перечисления!)
                
                foreach (var item in cachedItems) // Идем по кэшу (нет перечисления!)
                {
                    yield return item * sum;
                }
            }
        }

        static void TestDoubleEnumeration()
        {
            Console.WriteLine("СОЗДАЮ источник с подсчетом обращений");
            var countedSource = new CountingEnumerable<int>(new[] { 1, 2, 3 });
            
            Console.WriteLine("ЗАПУСКАЮ MethodWithDoubleEnumeration");
            var result = MethodWithDoubleEnumeration(countedSource);
            
            Console.WriteLine("НАЧИНАЮ foreach (первый проход)");
            foreach (var item in result)
            {
                Console.WriteLine($"   получил: {item}");
            }
            
            Console.WriteLine($"\nИТОГ: источник перечислен {countedSource.EnumerationCount} раза!");
            Console.WriteLine("(Sum() - первое перечисление, foreach - второе)");
        }
        
        #endregion

        #region ========== ВСПОМОГАТЕЛЬНЫЙ КЛАСС: СЧЕТЧИК ПЕРЕЧИСЛЕНИЙ ==========
        
        /// <summary>
        /// Класс-обертка для подсчета количества перечислений IEnumerable
        /// Это ключевой инструмент для тестирования!
        /// </summary>
        class CountingEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> _source;
            public int EnumerationCount { get; private set; }

            public CountingEnumerable(IEnumerable<T> source)
            {
                _source = source;
            }

            public IEnumerator<T> GetEnumerator()
            {
                // КАЖДЫЙ вызов GetEnumerator() увеличивает счетчик
                // Это значит, что КАЖДЫЙ foreach вызовет новый GetEnumerator()
                EnumerationCount++;
                Console.WriteLine($"   [СЧЕТЧИК] GetEnumerator() вызван! (всего: {EnumerationCount})");
                return _source.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
        
        #endregion
    }

    /*
     * ============================================================================
     * ТЕОРЕТИЧЕСКАЯ ЧАСТЬ: КАК ЭТО РАБОТАЕТ ПОД КАПОТОМ
     * ============================================================================
     * 
     * 1. ЧТО ТАКОЕ IEnumerable?
     *    
     *    IEnumerable - это интерфейс с одним методом: GetEnumerator().
     *    GetEnumerator() возвращает IEnumerator - объект с методами MoveNext() и Current.
     *    
     *    Проще говоря: IEnumerable - это "фабрика итераторов". Это НЕ данные,
     *    это способ ПОЛУЧИТЬ итератор для обхода данных.
     *    
     * 2. ЧТО ТАКОЕ yield return?
     *    
     *    Когда вы пишете метод с yield return, компилятор создает скрытый класс
     *    (state machine), который реализует и IEnumerable, и IEnumerator.
     *    
     *    Примерно так:
     *    
     *    IEnumerable<int> MyMethod() { yield return 1; }
     *    
     *    Превращается в:
     *    
     *    class MyMethod_Generated : IEnumerable<int>, IEnumerator<int>
     *    {
     *        private int state;      // Текущее состояние (0=начало, -1=конец)
     *        private int current;    // Текущий элемент для Current
     *        
     *        public IEnumerator<int> GetEnumerator() 
     *        { 
     *            // Создается НОВЫЙ экземпляр с state=0
     *            return new MyMethod_Generated(); 
     *        }
     *        
     *        public bool MoveNext()
     *        {
     *            switch(state)
     *            {
     *                case 0: state = -1; 
     *                        current = 1; 
     *                        state = 1; 
     *                        return true;
     *                case 1: state = -1; 
     *                        return false;
     *            }
     *        }
     *    }
     *    
     * 3. ПОЧЕМУ ToList() - ЭТО ПЛОХО В МЕТОДАХ-ОБРАБОТЧИКАХ?
     *    
     *    ToList() делает:
     *    - Вызывает GetEnumerator() у source
     *    - Проходит по всем элементам (MoveNext/Current)
     *    - Копирует их в новый List<T>
     *    
     *    Это означает, что источник ПЕРЕЧИСЛЯЕТСЯ ПРЯМО СЕЙЧАС.
     *    А если источник - это бесконечная последовательность? Будет бесконечный цикл!
     *    
     * 4. КАК ПРАВИЛЬНО ПИСАТЬ МЕТОДЫ С IEnumerable?
     *    
     *    - НИКОГДА не вызывайте ToList(), ToArray() и т.п. без необходимости
     *    - Используйте yield return для ленивой обработки
     *    - Если нужно сохранить данные (например, для подсчета суммы), 
     *      кэшируйте их при ПЕРВОМ проходе
     *    - Помните: foreach по source = одно перечисление
     *    
     * 5. ТЕСТИРОВАНИЕ ЛЕНИВОСТИ:
     *    
     *    var counted = new CountingEnumerable(source);
     *    var result = MyMethod(counted);
     *    
     *    // Проверка: counted.EnumerationCount должен быть 0 (еще не перечисляли)
     *    Assert.Equal(0, counted.EnumerationCount);
     *    
     *    foreach(var x in result) { ... } // Теперь должно быть 1
     *    
     * 6. ТЕСТИРОВАНИЕ ДВОЙНОГО ПЕРЕЧИСЛЕНИЯ:
     *    
     *    var counted = new CountingEnumerable(source);
     *    var result = MyMethod(counted);
     *    
     *    // Проходим foreach один раз
     *    foreach(var x in result) { ... }
     *    
     *    // EnumerationCount должен быть 1, а не 2
     *    Assert.Equal(1, counted.EnumerationCount);
     *    
     *    // Если мы пройдем foreach второй раз - может быть 2 (зависит от метода)
     *    // Но источник должен перечисляться только один раз за ВЫЗОВ метода
     *    // А не один раз за foreach по результату!
     */
}


