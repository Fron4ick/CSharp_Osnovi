using System;

namespace StrBuilder
{
    class StrBuilder
    {
        private char[] _characters;
        private int _length;
        private int _capacity;

        // Конструкторы
        public StrBuilder()
        {
            _capacity = 16;
            _characters = new char[_capacity];
            _length = 0;
        }

        public StrBuilder(string text)
        {
            if (text == null)
            {
                _capacity = 16;
                _characters = new char[_capacity];
                _length = 0;
            }
            else
            {
                _length = text.Length;
                _capacity = CalculateCapacity(_length);
                _characters = new char[_capacity];
                
                for (int i = 0; i < _length; i++)
                    _characters[i] = text[i];
            }
        }

        public StrBuilder(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("Емкость не может быть отрицательной");
                
            _capacity = capacity > 0 ? capacity : 16;
            _characters = new char[_capacity];
            _length = 0;
        }

        public StrBuilder(string text, int capacity)
        {
            if (capacity < 0)
                throw new ArgumentException("Емкость не может быть отрицательной");
                
            _capacity = capacity > 0 ? capacity : 16;
            _characters = new char[_capacity];
            
            if (text != null)
            {
                _length = Math.Min(text.Length, _capacity);
                for (int i = 0; i < _length; i++)
                    _characters[i] = text[i];
            }
            else
            {
                _length = 0;
            }
        }

        // Основной метод
        public void Main()
        {
            string r = Console.ReadLine();
            StrBuilder result = StrBuild(r);
            Console.WriteLine(result);
            Console.WriteLine($"Тип объекта: {result.GetType()}");
        }

        // Статические методы для создания объектов
        public static StrBuilder StrBuild()
        {
            return new StrBuilder();
        }

        public static StrBuilder StrBuild(string text)
        {
            return new StrBuilder(text);
        }

        public static StrBuilder StrBuild(int power)
        {
            return new StrBuilder(power);
        }

        public static StrBuilder StrBuild(string text, int power)
        {
            return new StrBuilder(text, power);
        }

        // Вспомогательные методы
        private int CalculateCapacity(int length)
        {
            int capacity = 16;
            while (capacity < length)
                capacity *= 2;
            return capacity;
        }

        // Метод для добавления строки
        public void Append(string text)
        {
            if (text == null) return;
            
            int newLength = _length + text.Length;
            if (newLength > _capacity)
            {
                // Увеличиваем емкость
                int newCapacity = CalculateCapacity(newLength);
                char[] newArray = new char[newCapacity];
                
                // Копируем старые данные
                for (int i = 0; i < _length; i++)
                    newArray[i] = _characters[i];
                    
                _characters = newArray;
                _capacity = newCapacity;
            }
            
            // Добавляем новые символы
            for (int i = 0; i < text.Length; i++)
                _characters[_length + i] = text[i];
                
            _length = newLength;
        }

        // Преобразование в строку
        public override string ToString()
        {
            return new string(_characters, 0, _length);
        }

        // Свойства для доступа к информации
        public int Length => _length;
        public int Capacity => _capacity;

        // Вывод информации о StringBuilder
        public void PrintInfo()
        {
            Console.WriteLine($"StrBuilder: '{ToString()}'");
            Console.WriteLine($"Длина: {_length}, Емкость: {_capacity}");
            Console.WriteLine($"Тип: {GetType()}");
        }
    }

    // Пример использования
    class Program
    {
        static void Main(string[] args)
        {
            // Создание через конструкторы
            StrBuilder sb1 = new StrBuilder();
            StrBuilder sb2 = new StrBuilder("Hello");
            StrBuilder sb3 = new StrBuilder(32);
            StrBuilder sb4 = new StrBuilder("World", 64);

            // Создание через статические методы
            StrBuilder sb5 = StrBuilder.StrBuild();
            StrBuilder sb6 = StrBuilder.StrBuild("Test");
            StrBuilder sb7 = StrBuilder.StrBuild(128);
            StrBuilder sb8 = StrBuilder.StrBuild("Example", 256);

            // Демонстрация работы
            sb1.Append("Hello ");
            sb1.Append("World!");
            
            sb1.PrintInfo();
            Console.WriteLine();
            
            sb2.Append(" everyone!");
            sb2.PrintInfo();
        }
    }
}
