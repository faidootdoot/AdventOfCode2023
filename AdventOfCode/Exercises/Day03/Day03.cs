using static AdventOfCode.Exercises.Day01.Day01;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Exercises.Day03;
public class Day03 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day03\\Data.txt");
    readonly char[,] dataArray = default!;

    public Day03() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        var numberOfRows = lines.Count;
        var numberOfColumns = lines.FirstOrDefault()?.Length;
        if (numberOfColumns > 0) {
            this.dataArray = new char[numberOfRows, numberOfColumns.Value];
            for (int row = 0; row < lines.Count; row++) {

                var line = lines[row];

                for (int column = 0; column < line.Length; column++) {
                    this.dataArray[row, column] = line[column];
                }
            }
        }
    }

    // Ideas:
    // Is it possible to work from the symbols out to the numbers

    public void RunPart1() {

        long total = 0;

        for (int row = 0; row < this.dataArray.GetLength(0); row++) {

            Console.WriteLine("");
            Console.Write($"Line {row} : ");

            bool hasSymbol = false;
            string numberText = string.Empty;

            for (int column = 0; column < this.dataArray.GetLength(1); column++) {

                var character = this.dataArray[row, column];
                if (Char.IsNumber(character)) {

                    numberText = $"{numberText}{character}";
                    if (!hasSymbol) {
                        hasSymbol = this.HasAdjacentSymbol(row, column);
                    }
                }
                else {

                    total = SumNumber(numberText, hasSymbol, total);
                    numberText = string.Empty;
                    hasSymbol = false;
                }

                // End of the line
                if (column == this.dataArray.GetLength(1) - 1) {
                    total = SumNumber(numberText, hasSymbol, total);
                }
            }
        }
        Console.WriteLine($"Total is {total}");
    }

    public void RunPart2() {

        for (int row = 0; row < this.dataArray.GetLength(0); row++) {

            for (int column = 0; column < this.dataArray.GetLength(1); column++) {

                var character = this.dataArray[row, column];
                if (IsGear(character)) {

                }
            }
        }
    }

    #region Helpers
    private static long SumNumber(string numberText, bool hasSymbol, long total) {
        if (numberText.Length > 0 && hasSymbol) {
            var number = int.Parse(numberText);
            Console.Write($"{number} ");
            total += number;
        }
        return total;
    }

    private static bool IsSymbol(char character) => !Char.IsNumber(character) && character != '.';
    private static bool IsGear(char character) => character == '*';

    private bool HasAdjacentSymbol(int currentRow, int currentColumn) {

        bool hasSymbol;

        hasSymbol = this.IsAboveSymbol(currentRow, currentColumn)
                    || this.IsBottomSymbol(currentRow, currentColumn)
                    || this.IsLeftSymbol(currentRow, currentColumn)
                    || this.IsRightSymbol(currentRow, currentColumn)
                    || this.IsAboveLeftSymbol(currentRow, currentColumn)
                    || this.IsAboveRightSymbol(currentRow, currentColumn)
                    || this.IsBottomLeftSymbol(currentRow, currentColumn)
                    || this.IsBottomRightSymbol(currentRow, currentColumn);

        return hasSymbol;
    }

    private bool IsBottomSymbol(int currentRow, int currentColumn) => (currentRow < this.dataArray.GetLength(0) - 1) && IsSymbol(this.dataArray[currentRow + 1, currentColumn]);
    private bool IsBottomLeftSymbol(int currentRow, int currentColumn) => (currentRow < this.dataArray.GetLength(0) - 1 && currentColumn > 0) && IsSymbol(this.dataArray[currentRow + 1, currentColumn - 1]);
    private bool IsBottomRightSymbol(int currentRow, int currentColumn) => (currentRow < this.dataArray.GetLength(0) - 1 && currentColumn < this.dataArray.GetLength(1) - 1) && IsSymbol(this.dataArray[currentRow + 1, currentColumn + 1]);
    private bool IsAboveSymbol(int currentRow, int currentColumn) => (currentRow > 0) && IsSymbol(this.dataArray[currentRow - 1, currentColumn]);
    private bool IsAboveLeftSymbol(int currentRow, int currentColumn) => (currentRow > 0 && currentColumn > 0) && IsSymbol(this.dataArray[currentRow - 1, currentColumn - 1]);
    private bool IsAboveRightSymbol(int currentRow, int currentColumn) => (currentRow > 0 && currentColumn < this.dataArray.GetLength(1) - 1) && IsSymbol(this.dataArray[currentRow - 1, currentColumn + 1]);
    private bool IsLeftSymbol(int currentRow, int currentColumn) => (currentColumn > 0) && IsSymbol(this.dataArray[currentRow, currentColumn - 1]);
    private bool IsRightSymbol(int currentRow, int currentColumn) => (currentColumn < this.dataArray.GetLength(1) - 1) && IsSymbol(this.dataArray[currentRow, currentColumn + 1]);

    #endregion
}
