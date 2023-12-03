using System;
using System.Numerics;
using System.Runtime.ExceptionServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Exercises.Day01;
internal class Day01 {

    internal Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day01\\Data.txt");

    internal enum Numbers {
        one = 1,
        two = 2,
        three = 3,
        four = 4,
        five = 5,
        six = 6,
        seven = 7,
        eight = 8,
        nine = 9
    }

    internal enum Direction {
        StartsWith,
        EndsWith,
    }

    internal void RunPart1() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();

        // Using foreach loop
        int sum = 0;
        foreach (var line in lines) {
            var firstNumber = line.First(c => Char.IsNumber(c));
            var lastNumber = line.Last(c => Char.IsNumber(c));
            int number = int.Parse($"{firstNumber}{lastNumber}");
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

    internal void RunPart2() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        int sum = 0;


        foreach (var line in lines) {

            // Find first number
            bool firstNumberFound = false;
            int firstNumber = 0;
            int i = 0;

            while (!firstNumberFound && i < line.Length) {

                (firstNumberFound, firstNumber) = GetNumberFromFigure(line, i);

                if (!firstNumberFound) {

                    (firstNumberFound, firstNumber) = GetNumberFromText(line, i, Direction.StartsWith);
                }

                i++;
            }

            // Find last number
            bool lastNumberFound = false;
            int lastNumber = 0;
            i = line.Length - 1;
            while (!lastNumberFound && i >= 0) {

                (lastNumberFound, lastNumber) = GetNumberFromFigure(line, i);

                if (!lastNumberFound) {

                    (lastNumberFound, lastNumber) = GetNumberFromText(line, i, Direction.EndsWith);
                }
                i--;
            }

            Console.WriteLine($"Line : {line} \t firstnumber : {firstNumber} \t lastnumber : {lastNumber}");

            int number = int.Parse($"{firstNumber}{lastNumber}");
            sum += number;
        }

        Console.WriteLine($"Sum is {sum}");
    }

    internal void RunPart2_1() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        int sum = 0;
        foreach (var line in lines) {

            bool firstNumberFound = false;
            bool lastNumberFound = false;
            int firstNumber = 0;
            int lastNumber = 0;
            int i = 0;
            do {

                // Find first number
                if (Char.IsNumber(line[i])) {
                    firstNumber = int.Parse(line[i].ToString());
                    firstNumberFound = true;
                }

                if (!firstNumberFound) {
                    foreach (string numberName in Enum.GetNames(typeof(Numbers))) {
                        var subLine = line[i..];
                        if (subLine.StartsWith(numberName)) {
                            if (Enum.TryParse(typeof(Numbers), numberName, true, out var dd)) {
                                firstNumber = (int)((Numbers)dd);
                            }
                            firstNumberFound = true;
                            break;
                        }
                    }
                }

                // Find last number
                var endIndex = line.Length - i - 1;
                if (Char.IsNumber(line[endIndex])) {
                    lastNumber = int.Parse(line[endIndex].ToString());
                    lastNumberFound = true;
                }

                if (!lastNumberFound) {
                    foreach (string numberName in Enum.GetNames(typeof(Numbers))) {
                        var subLine = line[..^i];
                        if (subLine.EndsWith(numberName)) {
                            if (Enum.TryParse(typeof(Numbers), numberName, true, out var dd)) {
                                lastNumber = (int)((Numbers)dd);
                            }
                            lastNumberFound = true;
                            break;
                        }
                    }
                }

                i++;

            } while (!firstNumberFound || !lastNumberFound && i < line.Length);


            int number = int.Parse($"{firstNumber}{lastNumber}");
            sum += number;

            Console.WriteLine($"Line : {line} \t firstnumber : {firstNumber} \t lastnumber : {lastNumber}");
        }

        Console.WriteLine($"Sum is {sum}");
    }

    #region Helpers
    internal static (bool isNumber, int number) GetNumberFromFigure(string line, int index) {

        int number = -1;
        bool isNumber = false;

        if (Char.IsNumber(line[index])) {

            number = int.Parse(line[index].ToString());
            isNumber = true;
        }
        return (isNumber, number);
    }

    internal static (bool isNUmber, int number) GetNumberFromText(string line, int index, Direction direction) {

        int number = -1;
        bool isNumber = false;

        foreach (string numberName in Enum.GetNames(typeof(Numbers))) {

            string subLine;
            switch (direction) {
                case Direction.StartsWith:
                    subLine = line[index..];
                    isNumber = subLine.StartsWith(numberName);
                    break;
                case Direction.EndsWith:
                    subLine = line[..(index + 1)];
                    isNumber = subLine.EndsWith(numberName);
                    break;
            }

            if (isNumber) {
                number = ConvertNumberTextToInt(numberName);
                break;
            }
        }
        return (isNumber, number);
    }

    internal static int ConvertNumberTextToInt(string numberName) {

        return (int)((Numbers)Enum.Parse(typeof(Numbers), numberName, true));
    }

    #endregion
}


