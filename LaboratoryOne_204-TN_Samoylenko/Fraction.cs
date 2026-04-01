using System;
using System.Collections.Generic;
using System.Linq;

namespace FractionApp {
    // Клас "Звичайний дріб"
    public class Fraction {
        public long Numerator { get; private set; }   // P - ціле
        public long Denominator { get; private set; } // Q - натуральне

        // Конструктор
        public Fraction(long numerator, long denominator) {
            if (denominator == 0)
                throw new ArgumentException("Знаменник не може бути нулем.");
            
            Numerator = numerator;
            Denominator = denominator;
            Simplify(); // Автоматичне скорочення при створенні
            }

        // Конструктор копіювання
        public Fraction(Fraction other) {
            this.Numerator = other.Numerator;
            this.Denominator = other.Denominator;
            }

        // Метод скорочення дробу
        public void Simplify() {
            long common = GCD(Math.Abs(Numerator), Math.Abs(Denominator));
            Numerator /= common;
            Denominator /= common;

            if (Denominator < 0) { // Знаменник має бути натуральним (Q > 0) 
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }

        // Найбільший спільний дільник для скорочення
        private long GCD(long a, long b) {
            while (b != 0) {
                a %= b;
                long temp = a;
                a = b;
                b = temp;
            }
            return a;
        }

        // Арифметичні операції
        public static Fraction operator +(Fraction a, Fraction b) => new Fraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);

        public static Fraction operator -(Fraction a, Fraction b) => new Fraction(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);

        public static Fraction operator *(Fraction a, Fraction b) => new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

        public static Fraction operator /(Fraction a, Fraction b) => new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);

        // Піднесення до ступеня n
        public Fraction Power(int n) {
            return new Fraction((long)Math.Pow(Numerator, n), (long)Math.Pow(Denominator, n));
        }

        // Операції порівняння
        public static bool operator > (Fraction a, Fraction b) => a.Numerator * b.Denominator > b.Numerator * a.Denominator;
        public static bool operator < (Fraction a, Fraction b) => a.Numerator * b.Denominator < b.Numerator * a.Denominator;
        public static bool operator >= (Fraction a, Fraction b) => a.Numerator * b.Denominator >= b.Numerator * a.Denominator;
        public static bool operator <= (Fraction a, Fraction b) => a.Numerator * b.Denominator <= b.Numerator * a.Denominator;
        public static bool operator == (Fraction a, Fraction b) => a.Numerator == b.Numerator && a.Denominator == b.Denominator;
        public static bool operator != (Fraction a, Fraction b) => !(a == b);

        public override string ToString() => $"{Numerator}/{Denominator}";
        
        public override bool Equals(object obj) => obj is Fraction f && this == f;
        public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);
    }

    // Клас-контейнер для динамічного масиву дробів
    public class FractionArray {
        private Fraction[] items;

        public int Length => items.Length;

        // Конструктор довільної розмірності
        public FractionArray(int size) {
            items = new Fraction[size];
        }

        // Деструктор (у C# це фіналізатор)
        ~FractionArray() {
            // У керованому C# тут зазвичай нічого не робиться, 
            // якщо немає некерованих ресурсів (файлів, сокетів).
            Console.WriteLine("Об'єкт FractionArray знищено.");
        }

        // Індексатор для доступу та зміни окремих елементів
        public Fraction this[int index] {
            get => items[index];
            set => items[index] = value;
        }
            
        // Метод виводу
        public void Display() {
            Console.WriteLine(string.Join("; ", items.Select(f => f?.ToString() ?? "null")));
        }

        // Сортування (Метод бульбашки для наочності)
        public void Sort() {
            for (int i = 0; i < items.Length - 1; i++) {
                for (int j = 0; j < items.Length - i - 1; j++) {
                    if (items[j] > items[j + 1]) {
                        var temp = items[j];
                        items[j] = items[j + 1];
                        items[j + 1] = temp;
                    }
                }
            }
        }
    }

    class Program {
        static void Main() {
            // 1. Створення масиву довільної розмірності
            Console.Write("Введіть кількість дробів: ");
            int n = int.Parse(Console.ReadLine());
            FractionArray array = new FractionArray(n);

            // Заповнення (приклад)
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                array[i] = new Fraction(rnd.Next(1, 10), rnd.Next(1, 10));
            }

            Console.WriteLine("\nПочатковий масив:");
            array.Display();

            // 2. Сортування
            array.Sort();
            Console.WriteLine("\nВідсортований масив:");
            array.Display();

            // 3. Демонстрація операцій над першими двома дробами (якщо є)
            if (n >= 2) {
                Fraction f1 = array[0];
                Fraction f2 = array[1];
                Console.WriteLine($"\nОперації над {f1} та {f2}:");
                Console.WriteLine($"Додавання: {f1 + f2}");
                Console.WriteLine($"Віднімання: {f1 - f2}");
                Console.WriteLine($"Множення: {f1 * f2}");
                Console.WriteLine($"Ділення: {f1 / f2}");
                Console.WriteLine($"Перший дріб у квадраті: {f1.Power(2)}");
                Console.WriteLine($"Чи рівні вони? {f1 == f2}");
            }
        }
    }
}