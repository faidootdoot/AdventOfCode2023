using AdventOfCode.Exercises.Helpers;
using FluentAssertions;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Exercises.Day04;

public class Card {
    public int Id { get; set; }
    public List<int> WinningNumbers { get; set; }
    public List<int> CardNumbers { get; set; }
    public Dictionary<int, bool> CardNumbersDictionary { get; set; }
    public List<int> MatchingNumbers { get; set; }

    public Card(string line) {

        this.Id = Utils.GetId(line, "Card");
        var startIndex = line.IndexOf(":") + 1;
        var numbers = line[startIndex..].Trim().Split("|");
        numbers.Should().HaveCount(2);

        this.WinningNumbers = numbers.ElementAt(0).Trim().Split(" ").Where(text => !string.IsNullOrEmpty(text)).Select(int.Parse).Order().ToList();
        this.CardNumbers = numbers.ElementAt(1).Trim().Split(" ").Where(text => !string.IsNullOrEmpty(text)).Select(int.Parse).Order().ToList();
        this.WinningNumbers = numbers.ElementAt(0).Trim().Split(" ").Where(text => !string.IsNullOrEmpty(text)).Select(int.Parse).ToList();
        this.CardNumbers = numbers.ElementAt(1).Trim().Split(" ").Where(text => !string.IsNullOrEmpty(text)).Select(int.Parse).ToList();
        this.CardNumbersDictionary = numbers.ElementAt(1).Trim().Split(" ").Where(text => !string.IsNullOrEmpty(text)).Select(int.Parse).ToDictionary(number => number, number => false);

        this.MatchingNumbers = this.CardNumbers.Intersect(this.WinningNumbers).Order().ToList();
        //this.MatchingNumbers = this.WinningNumbers.Intersect(this.CardNumbers).ToList();
    }

    public double CalculatePoints() {

        var count = this.MatchingNumbers.Count;
        double points = 0;
        if (count == 0) {
            points = 0;
        }
        else if (count > 0) {
            points = Math.Pow(2, count - 1);
        }
        return points;
    }

    public void Log() {
        Console.WriteLine($"\r\nCard : {this.Id}");
        Console.WriteLine("WinningNumbers");
        foreach (var number in this.WinningNumbers) {
            Console.Write(number);
            Console.Write(" ");
        }
        Console.WriteLine("\r\nCardNumbers");
        foreach (var number in this.CardNumbers) {
            Console.Write(number);
            Console.Write(" ");
        }
        Console.WriteLine("\r\nMatching numbers");
        foreach (var number in this.MatchingNumbers) {
            Console.Write(number);
            Console.Write(" ");
        }
    }
}


