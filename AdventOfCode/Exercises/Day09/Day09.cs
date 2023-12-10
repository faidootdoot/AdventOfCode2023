
namespace AdventOfCode.Exercises.Day09;

public class Day09 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day09\\Data.txt");
    private readonly List<Data> DataList = new();
    private readonly List<List<int>> dataList = new();

    public Day09() {
        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        foreach (var line in lines) {

            Data data = new();
            data.ListOfNumberLists.Add(line.Split(" ").Select(int.Parse).ToList());
            this.DataList.Add(data);
        }
    }

    public void RunPart1() {

        long totalLast = 0;
        long totalPrevious = 0;
        foreach (var data in this.DataList) {

            // Calculate the differences
            var differences = data.ListOfNumberLists.FirstOrDefault()!;
            do {
                differences = this.CalculateDifferencesList(differences);
                data.ListOfNumberLists.Add(differences);
            } while (differences.Count(num => num == 0) != differences.Count);

            // Don't know why the append is not working?
            // DOn't really need to append and prepend 0 as can add to the end insert on the fly, but that maybe bad programming
            data.ListOfNumberLists.ForEach(list => list.Add(0));
            data.ListOfNumberLists.ForEach(list => list.Insert(0, 0));

            for (int rowIndex = data.ListOfNumberLists.Count - 2; rowIndex >= 0; rowIndex--) {

                var row = data.ListOfNumberLists.ElementAt(rowIndex);
                var previousRowLastNumber = data.ListOfNumberLists.ElementAt(rowIndex + 1).Last();
                var previousRowFirstNumber = data.ListOfNumberLists.ElementAt(rowIndex + 1).First();

                // Calculate the next sequence
                row[^1] = row[^2] + previousRowLastNumber;

                // Calculate the previous sequence
                row[0] = row.ElementAt(1) - previousRowFirstNumber;

            if (rowIndex == 0) {
                totalLast += row.Last();
                totalPrevious += row[0];
            }
        }

        Print(data.ListOfNumberLists);
    }

    Console.WriteLine($"PART 1");
        Console.WriteLine($"Last total is {totalLast}");
        Console.WriteLine($"Previous total is {totalPrevious}");

    }

public void RunPart2() { }

public List<int> CalculateDifferencesList(List<int> history) {

    var differences = history.Take(history.Count - 1).Select((v, i) => history[i + 1] - v).ToList();

    //Console.WriteLine("");
    //history.ForEach(history => Console.Write(history.ToString().PadRight(5)));
    //Console.WriteLine("");
    //differences.ForEach(diff => Console.Write($"{diff} "));

    return differences;
}

public static void Print(List<int> listOfNumbers) {
    listOfNumbers.ForEach(diff => Console.Write($"{diff} "));
    Console.WriteLine("");
}

public static void Print(List<List<int>> deepListOfNumbers) {
    deepListOfNumbers.ForEach(diffList => Print(diffList));
}
}