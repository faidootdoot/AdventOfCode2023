using System.Text.RegularExpressions;

namespace AdventOfCode.Exercises.Day11;

public class Day11 {

    public class Location {
        public int Id { get; set; }
        public int Row { get; set; }
        public int column { get; set; }
    }


    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day11\\Data.txt");
    private List<string> lines = new();
    private const char EmptySpaceSymbol = '.';
    private const char GalaxySymbol = '#';
    private readonly List<Location> GalaxyLocations = new();
    private readonly List<int> emptyRows = new();
    private readonly List<int> emptyColumns = new();

    public Day11() {

    }

    private void Reset() {

        this.lines.Clear();
        this.GalaxyLocations.Clear();
        this.emptyRows.Clear();
        this.emptyColumns.Clear();
    }


    private bool IsEmptyColumn(int column) {

        int emptyCounter = this.lines.Count(line => line.ElementAt(column).Equals(EmptySpaceSymbol));
        return emptyCounter == this.lines.Count;

    }

    private bool IsEmptyRow(int row) {

        int emptyCounter = this.lines.ElementAt(row).Count(character => character.Equals(EmptySpaceSymbol));
        return emptyCounter == this.lines.ElementAt(0).Length;
    }

    private void InsertEmptyRow(int row) {

        string newLine = new(EmptySpaceSymbol, this.lines.ElementAt(0).Length);
        this.lines.Insert(row, newLine);
    }

    private void InsertEmptyColumn(int column) {

        for (int i = 0; i < this.lines.Count; i++) {

            var line = this.lines.ElementAt(i);
            this.lines[i] = line.Insert(column, EmptySpaceSymbol.ToString());
        }
    }

    private void ExpandEmptyRows() {

        for (int row = 0; row < this.lines.Count; row++) {
            if (this.IsEmptyRow(row)) {
                this.InsertEmptyRow(row);
                row++;
            }
        }
    }

    private void ExpandEmptyColumn() {

        for (int column = 0; column < this.lines.ElementAt(0).Length; column++) {
            if (this.IsEmptyColumn(column)) {
                this.InsertEmptyColumn(column);
                column++;
            }
        }
    }

    private void IdentifyEmptyRows() {

        for (int row = 0; row < this.lines.Count; row++) {
            if (this.IsEmptyRow(row)) {
                this.emptyRows.Add(row);
            }
        }
    }

    private void IdentifyEmptyColums() {

        for (int column = 0; column < this.lines.ElementAt(0).Length; column++) {
            if (this.IsEmptyColumn(column)) {
                this.emptyColumns.Add(column);
            }
        }
    }

    private void PrintMap(int? rows) {

        int endRow = rows == null ? this.lines.Count : rows.Value;

        for (int i = 0; i < endRow; i++) {
            Console.WriteLine(this.lines.ElementAt(i));
        }
    }

    private void PrintGalaxyLocations() {

        this.GalaxyLocations.ForEach(x => Console.WriteLine($"{x.Id.ToString().PadLeft(5)}, {x.Row.ToString().PadLeft(5)}, {x.column.ToString().PadLeft(5)}"));
    }

    private void ExpandMap() {

        this.ExpandEmptyRows();
        this.ExpandEmptyColumn();
    }

    private void IdentifyGalaxies() {

        int galaxyCount = 0;
        Regex regex = new(GalaxySymbol.ToString());

        for (int row = 0; row < this.lines.Count; row++) {

            var line = this.lines[row];
            var matches = regex.Matches(line);

            foreach (Match match in matches.Cast<Match>()) {
                var column = match.Index;
                this.GalaxyLocations.Add(new Location() {
                    Id = galaxyCount,
                    Row = row,
                    column = column
                });
                // No need to convert text numbers as till push out the string
                galaxyCount++;
            }
        }
    }

    private long CalculateShortestPath() {

        long total = 0;
        for (int controlIndex = 0; controlIndex < this.GalaxyLocations.Count; controlIndex++) {

            var controlLocation = this.GalaxyLocations[controlIndex];

            for (int relativeIndex = controlIndex + 1; relativeIndex < this.GalaxyLocations.Count; relativeIndex++) {

                var relativeLocation = this.GalaxyLocations[relativeIndex];
                total += Math.Abs(controlLocation.Row - relativeLocation.Row) + Math.Abs(controlLocation.column - relativeLocation.column);
            }
        }

        return total;
    }

    public void Something() {
        // Find all empty rows
        for (int row = 0; row < this.lines.Count; row++) {
            if (this.IsEmptyRow(row)) {
                Console.WriteLine($"Row {row} is empty");
            }
        }

        for (int Column = 0; Column < this.lines.Count; Column++) {
            if (this.IsEmptyColumn(Column)) {
                Console.WriteLine($"Column {Column} is empty");
            }
        }

        Console.WriteLine("Original");
        this.PrintMap(20);
        Console.WriteLine("Updated");
        this.PrintMap(20);

    }

    public void RunPart1() {

        this.lines = File.ReadLines(this.filePath.AbsolutePath).ToList();

        this.ExpandMap();
        this.IdentifyGalaxies();

        this.PrintMap(null);
        this.PrintGalaxyLocations();
        var totalSteps = this.CalculateShortestPath();
        Console.WriteLine("PART 1");
        Console.WriteLine($"Total steps of all shortest path is {totalSteps}");

    }

    public void RunPart2() {

        this.Reset();
        this.lines = File.ReadLines(this.filePath.AbsolutePath).ToList();

        this.IdentifyGalaxies();
        this.IdentifyEmptyColums();
        this.IdentifyEmptyRows();

        double total = 0;
        List<int> locationRows = new();
        List<int> locationColumns = new();
        for (int controlIndex = 0; controlIndex < this.GalaxyLocations.Count; controlIndex++) {

            var controlLocation = this.GalaxyLocations[controlIndex];

            for (int relativeIndex = controlIndex + 1; relativeIndex < this.GalaxyLocations.Count; relativeIndex++) {

                var relativeLocation = this.GalaxyLocations[relativeIndex];

                locationRows.Clear();
                locationRows.Add(controlLocation.Row);
                locationRows.Add(relativeLocation.Row);
                locationRows = locationRows.Order().ToList();
                locationColumns.Clear();
                locationColumns.Add(controlLocation.column);
                locationColumns.Add(relativeLocation.column);
                locationColumns = locationColumns.Order().ToList();

                total += Math.Abs(controlLocation.Row - relativeLocation.Row) + Math.Abs(controlLocation.column - relativeLocation.column);


                var emptyColumnCount = this.emptyColumns.Count(c => c > locationColumns[0] & c < locationColumns[1]);
                var emptyRowCount = this.emptyRows.Count(r => r > locationRows[0] & r < locationRows[1]);
                var emptyCount = emptyColumnCount + emptyRowCount;
                total -= emptyCount;
                total += emptyCount * 1000000;
            }
        }


        Console.WriteLine("PART 2");
        Console.WriteLine($"Total steps of all shortest path is {total}");

        Console.WriteLine("WIP");
    }

}
