using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Exercises.Day07;
public class Day07 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day07\\Data.txt");
    private List<Hand> hands = new();

    public Day07() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        foreach (var line in lines) {

            var dataArray = line.Split(" ");
            this.hands.Add(new Hand(dataArray[0].Trim(), int.Parse(dataArray[1].Trim())));
        }
    }

    public void RunPart1() {

        this.hands = this.hands.OrderBy(h => h.TypeOfHand)
                               .ThenBy(h => h.CardValues[0])
                               .ThenBy(h => h.CardValues[1])
                               .ThenBy(h => h.CardValues[2])
                               .ThenBy(h => h.CardValues[3])
                               .ThenBy(h => h.CardValues[4])
                               .ToList();

        Console.WriteLine("Ordered");
        this.Print();

        long totalWinnings = 0;
        for (int i = 0; i < this.hands.Count; i++) {
            totalWinnings += this.hands[i].Bid * (i + 1);
        }

        Console.WriteLine("PART 1");
        Console.WriteLine($"Total winnings is {totalWinnings}");
    }

    public void RunPart2() {

    }

    #region Helpers

    public void Print() {
        foreach (var hand in this.hands) {
            Console.WriteLine($"{hand.Cards.PadRight(10)}{hand.Bid.ToString().PadRight(10)}{hand.TypeOfHand}");
        }
    }

    /// <summary>
    /// Get the maximum labels
    /// </summary>
    /// <param name="cards"></param>
    /// <returns></returns>
    public int MaximumMatchingLabels(string cards) {

        int maxiumum = 0;

        foreach (var label in Constants.Labels) {
            int count = Regex.Matches(cards, label).Count;
            maxiumum = maxiumum < count ? count : maxiumum;
        }

        return (maxiumum);
    }

    #endregion
}
