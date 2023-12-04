using AdventOfCode.Exercises.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Exercises.Day04;
public class Day04 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day04\\Data.txt");
    private readonly List<Card> cards = new();

    public Day04() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        foreach (var line in lines) {

            this.cards.Add(new Card(line));
        }
    }

    public void RunPart1() {

        var points = this.cards.Select(card => card.CalculatePoints()).ToList();

        //#if DEBUG
        //        this.cards.ForEach(card => card.Log());
        //        Console.WriteLine($"Points");
        //        points.ForEach(points=> Console.Write($"{points} "));
        //#endif

        Console.WriteLine($"PART 1");
        var total = points.Sum();
        Console.WriteLine($"Total is {total}");
    }

    public void RunPart2() {

        List<int> instances = new();
        this.cards.ForEach(x => { instances.Add(1); });

        for (int cardIndex = 0; cardIndex < this.cards.Count; cardIndex++) {

            var count = this.cards.ElementAt(cardIndex).MatchingNumbers.Count;
            for (int instanceIndex = cardIndex; (instanceIndex < cardIndex + count) && instanceIndex < this.cards.Count; instanceIndex++) {
                instances[instanceIndex + 1] = instances[instanceIndex + 1] + instances[cardIndex];
            }
        }

        Console.WriteLine($"PART 2");
        var total = instances.Sum();
        Console.WriteLine($"Total is {total}");
    }
}
