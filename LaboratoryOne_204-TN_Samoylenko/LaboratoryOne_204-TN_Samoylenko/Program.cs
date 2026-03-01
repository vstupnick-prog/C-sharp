using System;
using System.Linq;

class Question
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Головне меню за завдань ===");
            Console.WriteLine("1. Обчислити математичний вираз");
            Console.WriteLine("2. Параметри рівностороннього трикутника");
            Console.WriteLine("3. Перевірка 4-значного числа на зростання");
            Console.WriteLine("4. Перевірка існування трикутника та площа");
            Console.WriteLine("5. Кількість чисел до n (не діляться на 2, 3, 5)");
            Console.WriteLine("6. Пошук слів з однаковим початком і кінцем");
            Console.WriteLine("7. Заміна ':' на ';' у рядку");
            Console.WriteLine("0. Вихід");
            Console.Write("\nОберіть номер завдання: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Task1(); break;
                case "2": Task2(); break;
                case "3": Task3(); break;
                case "4": Task4(); break;
                case "5": Task5(); break;
                case "6": Task6(); break;
                case "7": Task7(); break;
                case "0": exit = true; break;
                default: Console.WriteLine("Невірний вибір. Спробуйте ще раз."); break;
            }

            if (!exit)
            {
                Console.WriteLine("\nНатисніть будь-яку клавішу для повернення в меню...");
                Console.ReadKey();
            }
        }
    }

    //  ЗАВДАННЯ 

    static void Task1()
    {
        Console.WriteLine("\n[Завдання 1] Формула: x*ln(x) + y / (cos(x) - x/3)");
        double x = ReadDouble("Введіть x (x > 0): ");
        double y = ReadDouble("Введіть y: ");

        if (x <= 0)
        {
            Console.WriteLine("Помилка: ln(x) не визначено для x <= 0.");
            return;
        }

        double denominator = Math.Cos(x) - (x / 3.0);
        if (Math.Abs(denominator) < 1e-9) // Захист від ділення на 0
        {
            Console.WriteLine("Помилка: Знаменник дорівнює нулю.");
        }
        else
        {
            double res = x * Math.Log(x) + (y / denominator);
            Console.WriteLine($"Результат: {res:F4}");
        }
    }

    static void Task2()
    {
        Console.WriteLine("\n[Завдання 2] Рівносторонній трикутник");
        double a = ReadDouble("Введіть довжину сторони: ");

        if (a <= 0)
        {
            Console.WriteLine("Помилка: Сторона має бути додатною.");
        }
        else
        {
            double s = (Math.Pow(a, 2) * Math.Sqrt(3)) / 4;
            double h = (a * Math.Sqrt(3)) / 2;
            double R = a / Math.Sqrt(3);
            double r = a / (2 * Math.Sqrt(3));

            Console.WriteLine($"Площа: {s:F3}\nВисота: {h:F3}");
            Console.WriteLine($"Радіус описаного кола (R): {R:F3}");
            Console.WriteLine($"Радіус вписаного кола (r): {r:F3}");
        }
    }



    static void Task3()
    {
        Console.WriteLine("\n[Завдання 3] Перевірка зростаючої послідовності");
        int n = ReadInt("Введіть чотиризначне число: ");

        if (n < 1000 || n > 9999)
        {
            Console.WriteLine("False (число не є чотиризначним)");
            return;
        }

        int d4 = n % 10;
        int d3 = (n / 10) % 10;
        int d2 = (n / 100) % 10;
        int d1 = n / 1000;

        bool result = (d1 < d2 && d2 < d3 && d3 < d4);
        Console.WriteLine($"Результат: {result.ToString().ToLower()}");
    }

    static void Task4()
    {
        Console.WriteLine("\n[Завдання 4] Перевірка сторін трикутника");
        double a = ReadDouble("Сторона a: ");
        double b = ReadDouble("Сторона b: ");
        double c = ReadDouble("Сторона c: ");

        if (a > 0 && b > 0 && c > 0 && (a + b > c) && (a + c > b) && (b + c > a))
        {
            double p = (a + b + c) / 2;
            double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            Console.WriteLine($"Трикутник існує. Площа за формулою Герона: {area:F3}");
        }
        else
        {
            Console.WriteLine("Трикутник із такими сторонами неможливий.");
        }
    }

    static void Task5()
    {
        Console.WriteLine("\n[Завдання 5] Числа, що не діляться на 2, 3, 5");
        int n = ReadInt("Введіть натуральне число n: ");
        int count = 0;

        for (int i = 1; i <= n; i++)
        {
            if (i % 2 != 0 && i % 3 != 0 && i % 5 != 0)
                count++;
        }
        Console.WriteLine($"Кількість таких чисел: {count}");
    }

    static void Task6()
    {
        Console.WriteLine("\n[Завдання 6] Пошук слів (початок == кінець)");
        Console.Write("Введіть рядок: ");
        string input = Console.ReadLine() ?? "";

        char[] separators = { ' ', ',', '.', '!', '?', ';', ':' };
        string[] words = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine("Знайдені слова:");
        foreach (var word in words)
        {
            if (word.Length > 0 && char.ToLower(word[0]) == char.ToLower(word[word.Length - 1]))
            {
                Console.WriteLine($"- {word}");
            }
        }
    }

    static void Task7()
    {
        Console.WriteLine("\n[Завдання 7] Заміна символів");
        Console.Write("Введіть рядок з двокрапками: ");
        string input = Console.ReadLine() ?? "";

        int count = input.Count(c => c == ':');
        string result = input.Replace(':', ';');

        Console.WriteLine($"Результат: {result}");
        Console.WriteLine($"Зроблено замін: {count}");
    }

    // УТИЛІТИ ВВОДУ

    static double ReadDouble(string message)
    {
        double result;
        Console.Write(message);
        while (!double.TryParse(Console.ReadLine(), out result))
        {
            Console.Write("Помилка! Будь ласка, введіть число: ");
        }
        return result;
    }

    static int ReadInt(string message)
    {
        int result;
        Console.Write(message);
        while (!int.TryParse(Console.ReadLine(), out result))
        {
            Console.Write("Помилка! Будь ласка, введіть ціле число: ");
        }
        return result;
    }
}