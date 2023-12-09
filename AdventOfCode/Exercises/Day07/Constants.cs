using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Exercises.Day07;
public static class Constants {

    public static readonly string[] Labels = { "A", "K", "Q", "J", "T", "9", "8", "7", "6", "5", "4", "3", "2" };

    public enum HandType {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1
    }
}
