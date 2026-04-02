using System;

namespace OOPLab
{
    // Абстрактний базовий клас
    abstract class Pair
    {
        public double First { get; set; }
        public double Second { get; set; }

        protected Pair(double first, double second)
        {
            First = first;
            Second = second;
        }

        public abstract Pair Add(Pair other);
        public abstract Pair Sub(Pair other);
        public abstract Pair Mul(Pair other);
        public abstract Pair Div(Pair other);
    }

    // Клас "Нечіткі числа" через (значення, похибка)
    class FuzzyNumber : Pair
    {
        public FuzzyNumber(double value, double error) : base(value, error) { }


        public override Pair Add(Pair other) =>
            new FuzzyNumber(this.First + other.First, this.Second + other.Second);


        public override Pair Sub(Pair other) =>
            new FuzzyNumber(this.First - other.First, this.Second + other.Second);


        public override Pair Mul(Pair other) =>
            new FuzzyNumber(this.First * other.First, 
                           Math.Abs(this.First * other.Second) + Math.Abs(other.First * this.Second));

        public override Pair Div(Pair other)
        {
            if (other.First == 0) throw new DivideByZeroException();
            // Спрощена формула ділення для нечітких чисел
            double newVal = this.First / other.First;
            double newErr = Math.Abs(newVal) * (this.Second / Math.Abs(this.First) + other.Second / Math.Abs(other.First));
            return new FuzzyNumber(newVal, newErr);
        }

        public override string ToString() => $"Value: {First} (±{Second})";
    }

    // Клас "Дробові числа" (Ціла частина - long, дробова - ulong)
    class Fraction : Pair
    {
        // First - ціла частина, Second - дробова
        public Fraction(long whole, ulong fractional) : base(whole, fractional) { }

        // Конвертація в double для простіших розрахунків
        private double ToDouble()
        {
            double fracPart = Second;
            while (fracPart >= 1) fracPart /= 10; 
            return First >= 0 ? First + fracPart : First - fracPart;
        }

        // Створення об'єкта назад із double
        private static Fraction FromDouble(double val)
        {
            long whole = (long)Math.Truncate(val);
            // Беремо 6 знаків після коми для точності
            double fractionPart = Math.Abs(val - whole);
            ulong frac = (ulong)Math.Round(fractionPart * 1000000);

            while (frac > 0 && frac % 10 == 0) frac /= 10;
            return new Fraction(whole, frac);
        }

        public override Pair Add(Pair other) => FromDouble(this.ToDouble() + ((Fraction)other).ToDouble());
        public override Pair Sub(Pair other) => FromDouble(this.ToDouble() - ((Fraction)other).ToDouble());
        public override Pair Mul(Pair other) => FromDouble(this.ToDouble() * ((Fraction)other).ToDouble());
        public override Pair Div(Pair other) => FromDouble(this.ToDouble() / ((Fraction)other).ToDouble());

        public override string ToString() => $"{First}.{Second}";
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            Pair fuzzy1 = new FuzzyNumber(10.0, 0.5); 
            Pair fuzzy2 = new FuzzyNumber(5.0, 0.2);  
            
            Console.WriteLine("____Fuzzy Numbers____");
            Console.WriteLine($"A: {fuzzy1}");
            Console.WriteLine($"B: {fuzzy2}");
            Console.WriteLine($"A + B = {fuzzy1.Add(fuzzy2)}");
            Console.WriteLine($"A * B = {fuzzy1.Mul(fuzzy2)}");


            Pair frac1 = new Fraction(12, 5); 
            Pair frac2 = new Fraction(-2, 25); 

            Console.WriteLine("\n____Fractions____");
            Console.WriteLine($"F1: {frac1}");
            Console.WriteLine($"F2: {frac2}");
            Console.WriteLine($"F1 + F2 = {frac1.Add(frac2)}");
            Console.WriteLine($"F1 / F2 = {frac1.Div(frac2)}");

            Console.ReadKey();
        }
    }
}