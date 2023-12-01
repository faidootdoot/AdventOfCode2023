namespace AdventOfCode.Exercises.Day01;
internal class Day01 {

    internal static void Run() {

        var filePath = new Uri(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day01\\Data.txt");
        var lines = File.ReadLines(filePath.AbsolutePath).ToList();

        // Using foreach loop
        int sum = 0;
        foreach (var line in lines) {
            var firstNumber = line.First(c => Char.IsNumber(c));
            var lastNumber = line.Last(c => Char.IsNumber(c));
            int number = int.Parse($"{firstNumber}{lastNumber}");
            //Console.WriteLine($"Line is {line}: firstNumber is {firstNumber}: lastNumber is {lastNumber}: number is {number}: sum is {sum}");
            sum += number;
        }
        Console.WriteLine($"Sum is {sum}");

        // Using list foreach
        int sumFromLinq = 0;
        lines.ForEach(line => {
            sumFromLinq += int.Parse($"{line.First(c => Char.IsNumber(c))}{line.Last(c => Char.IsNumber(c))}");
        });
        Console.WriteLine($"Sum (linq) is {sumFromLinq}");
    }
}


