using FluentAssertions;
using FluentAssertions.Data;
using System.Diagnostics.Metrics;
using System.Numerics;
using static AdventOfCode.Exercises.Day01.Day01;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Exercises.Day03;
public class Day03 {

    private readonly List<string> lines;
    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day03\\Data.txt");
    readonly char[,] dataArray = default!;
    readonly bool[,] usedArray = default!;

    public Day03() {

        this.lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        var numberOfRows = this.lines.Count;
        var numberOfColumns = this.lines.FirstOrDefault()?.Length;
        if (numberOfColumns > 0) {
            this.dataArray = new char[numberOfRows, numberOfColumns.Value];
            this.usedArray = new bool[numberOfRows, numberOfColumns.Value];
            for (int row = 0; row < this.lines.Count; row++) {

                var line = this.lines[row];

                for (int column = 0; column < line.Length; column++) {
                    this.dataArray[row, column] = line[column];
                    this.usedArray[row, column] = false;
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

        long total = 0;
        List<int> numberList = new();
        for (int row = 0; row < this.dataArray.GetLength(0); row++) {

            for (int column = 0; column < this.dataArray.GetLength(1); column++) {

                var character = this.dataArray[row, column];
                if (IsGear(character)) {

                    int adjacentRow;
                    int adjacentColumn;
                    numberList.Clear();

                    // Row above
                    if (row > 0) {

                        if (column > 0) {
                            adjacentRow = row - 1;
                            adjacentColumn = column - 1;
                            if (this.IsNumber(adjacentRow, adjacentColumn)) {
                                numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                            }
                        }

                        adjacentRow = row - 1;
                        adjacentColumn = column;
                        if (this.IsNumber(adjacentRow, adjacentColumn)) {
                            numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                        }

                        if (column < this.dataArray.GetLength(1)) {
                            adjacentRow = row - 1;
                            adjacentColumn = column + 1;
                            if (this.IsNumber(adjacentRow, adjacentColumn)) {
                                numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                            }
                        }
                    }

                    // Same row
                    if (column > 0) {
                        adjacentRow = row;
                        adjacentColumn = column - 1;
                        if (this.IsNumber(adjacentRow, adjacentColumn)) {
                            numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                        }
                    }

                    if (column < this.dataArray.GetLength(1)) {
                        adjacentRow = row;
                        adjacentColumn = column + 1;
                        if (this.IsNumber(adjacentRow, adjacentColumn)) {
                            numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                        }
                    }

                    // Row below
                    if (row < this.dataArray.GetLength(0)) {
                        if (column > 0) {
                            adjacentRow = row + 1;
                            adjacentColumn = column - 1;
                            if (this.IsNumber(adjacentRow, adjacentColumn)) {
                                numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                            }
                        }
                        adjacentRow = row + 1;
                        adjacentColumn = column;
                        if (this.IsNumber(adjacentRow, adjacentColumn)) {
                            numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                        }
                        if (column < this.dataArray.GetLength(1)) {
                            adjacentRow = row + 1;
                            adjacentColumn = column + 1;
                            if (this.IsNumber(adjacentRow, adjacentColumn)) {
                                numberList.Add(this.ExtractNumber(adjacentRow, adjacentColumn));
                            }
                        }
                    }

                    if (numberList.Count == 2) {
                        total += (numberList[0] * numberList[1]);
                    }
                }
            }
        }

        Console.WriteLine($"PART 2");
        Console.WriteLine($"Total is {total}");
    }

    private bool IsNumber(int row, int column) {
        return (char.IsNumber(this.dataArray[row, column]) && !this.usedArray[row, column]);
    }

    private int ExtractNumber(int row, int column) {

        var boundaryIndexes = this.FindNumberBoundaryIndexes(row, column);
        this.MarkAsUsed(row, boundaryIndexes.startColumnIndex, boundaryIndexes.endColumnIndex);
        return this.GetNumber(row, boundaryIndexes.startColumnIndex, boundaryIndexes.endColumnIndex);
    }

    private (int startColumnIndex, int endColumnIndex) FindNumberBoundaryIndexes(int row, int column) {

        int endColumnIndex = column;
        while (endColumnIndex < this.dataArray.GetLength(1) && this.IsNumber(row, endColumnIndex) ) {
            endColumnIndex++;
        }
        int startColumnIndex = column - 1;
        while (startColumnIndex >= 0 && this.IsNumber(row, startColumnIndex)) {
            startColumnIndex--;
        }

        startColumnIndex++;
        endColumnIndex--;
        return (startColumnIndex, endColumnIndex);
    }

    private int GetNumber(int row, int startColumnIndex, int endColumnIndex) {

        string numberText = string.Empty;
        for (int column = startColumnIndex; column <= endColumnIndex; column++) {

            numberText += this.dataArray[row, column];
        }
        return int.Parse(numberText);
    }

    private void MarkAsUsed(int row, int startColumnIndex, int endColumnIndex) {

        for (int column = startColumnIndex; column <= endColumnIndex; column++) {

            this.usedArray[row, column] = true;
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

    //private bool IsBottomNumber(int currentRow, int currentColumn) => (currentRow < this.dataArray.GetLength(0) - 1) && char.IsNumber((this.dataArray[currentRow + 1, currentColumn]));
    //private bool IsBottomLeftNumber(int currentRow, int currentColumn) => (currentRow < this.dataArray.GetLength(0) - 1 && currentColumn > 0) && char.IsNumber((this.dataArray[currentRow + 1, currentColumn - 1]));
    //private bool IsBottomRightNumber(int currentRow, int currentColumn) => (currentRow < this.dataArray.GetLength(0) - 1 && currentColumn < this.dataArray.GetLength(1) - 1) && char.IsNumber((this.dataArray[currentRow + 1, currentColumn + 1]));
    //private bool IsAboveNumber(int currentRow, int currentColumn) => (currentRow > 0) && char.IsNumber((this.dataArray[currentRow - 1, currentColumn]));
    //private bool IsAboveLeftNumber(int currentRow, int currentColumn) => (currentRow > 0 && currentColumn > 0) && char.IsNumber((this.dataArray[currentRow - 1, currentColumn - 1]));
    //private bool IsAboveRightNumber(int currentRow, int currentColumn) => (currentRow > 0 && currentColumn < this.dataArray.GetLength(1) - 1) && char.IsNumber((this.dataArray[currentRow - 1, currentColumn + 1]));
    //private bool IsLeftNumber(int currentRow, int currentColumn) => (currentColumn > 0) && char.IsNumber((this.dataArray[currentRow, currentColumn - 1]));
    //private bool IsRightNumber(int currentRow, int currentColumn) => (currentColumn < this.dataArray.GetLength(1) - 1) && char.IsNumber((this.dataArray[currentRow, currentColumn + 1]));

    private (bool isNumber, char character) IsBottomNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow + 1, currentColumn];
        var isNumber = (currentRow < this.dataArray.GetLength(0) - 1) && char.IsNumber(character);
        return (isNumber, character);
    }

    private (bool isNumber, char character) IsBottomLeftNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow + 1, currentColumn - 1];
        var isNumber = (currentRow < this.dataArray.GetLength(0) - 1 && currentColumn > 0) && char.IsNumber(character);
        return (isNumber, character);
    }

    private (bool isNumber, char character) IsBottomRightNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow + 1, currentColumn + 1];
        var isNumber = (currentRow < this.dataArray.GetLength(0) - 1 && currentColumn < this.dataArray.GetLength(1) - 1) && char.IsNumber(character);
        return (isNumber, character);
    }
    private (bool isNumber, char character) IsAboveNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow - 1, currentColumn];
        var isNumber = (currentRow > 0) && char.IsNumber(character);
        return (isNumber, character);
    }

    private (bool isNumber, char character) IsAboveLeftNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow - 1, currentColumn - 1];
        var isNumber = (currentRow > 0 && currentColumn > 0) && char.IsNumber(character);
        return (isNumber, character);
    }

    private (bool isNumber, char character) IsAboveRightNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow - 1, currentColumn + 1];
        var isNumber = (currentRow > 0 && currentColumn < this.dataArray.GetLength(1) - 1) && char.IsNumber(character);
        return (isNumber, character);
    }

    private (bool isNumber, char character) IsLeftNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow, currentColumn - 1];
        var isNumber = (currentColumn > 0) && char.IsNumber(character);
        return (isNumber, character);
    }

    private (bool isNumber, char character) IsRightNumber(int currentRow, int currentColumn) {
        var character = this.dataArray[currentRow, currentColumn + 1];
        var isNumber = (currentColumn < this.dataArray.GetLength(1) - 1) && char.IsNumber(character);
        return (isNumber, character);
    }

    #endregion
}
