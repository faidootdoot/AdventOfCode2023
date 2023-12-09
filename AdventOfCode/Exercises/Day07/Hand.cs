using System.Text.RegularExpressions;
using static AdventOfCode.Exercises.Day07.Constants;

namespace AdventOfCode.Exercises.Day07;

public class Hand {

    public Hand(string cards, int bid) {
        this.Cards = cards;
        this.Bid = bid;
        this.CalculateCardValues();
        this.CalculateCardsCount();
        //this.CalculateHandTypePart1();
        this.CalculateHandTypePart2();
    }

    public Dictionary<string, int> CardsCount { get; set; } = new();
    public List<int> CardValues { get; set; } = new();
    public string Cards { get; init; }
    public int Bid { get; init; }
    public int TypeOfHand { get; set; } = default!;

    public void CalculateCardValues() {

        foreach (var character in this.Cards) {
            this.CardValues.Add(this.GetCardValue(character));
        }
    }
    public int GetCardValue(char character) {
        return character switch {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 1, //11
            'T' => 10,
            _ => int.Parse(character.ToString()),
        };
    }

    public void CalculateCardsCount() {

        var distinctCards = this.Cards.Distinct().ToList();
        foreach (var character in distinctCards) {

            var key = character.ToString();
            int count = Regex.Matches(this.Cards, key).Count;
            this.CardsCount[key] = count;
        }
    }

    public void CalculateHandTypePart1() {

        if (this.CardsCount.Any(x => x.Value == 5)) {
            this.TypeOfHand = (int)(HandType.FiveOfAKind);
        }
        else if (this.CardsCount.Any(x => x.Value == 4)) {
            this.TypeOfHand = (int)(HandType.FourOfAKind);
        }
        else if (this.CardsCount.Count(x => x.Value == 3) == 1 && this.CardsCount.Count(x => x.Value == 2) == 1) {
            this.TypeOfHand = (int)(HandType.FullHouse);
        }
        else if (this.CardsCount.Count(x => x.Value == 3) == 1 && !this.CardsCount.Any(x => x.Value == 2)) {
            this.TypeOfHand = (int)(HandType.ThreeOfAKind);
        }
        else if (this.CardsCount.Count(x => x.Value == 2) == 2) {
            this.TypeOfHand = (int)(HandType.TwoPair);
        }
        else if (this.CardsCount.Count(x => x.Value == 2) == 1 && this.CardsCount.Count(x => x.Value == 1) == 3) {
            this.TypeOfHand = (int)(HandType.OnePair);
        }
        else if (this.Cards.Distinct().Count() == 5) {
            this.TypeOfHand = (int)(HandType.HighCard);
        }
    }
    public void CalculateHandTypePart2() {

        int jackCount = 0;
        if (this.CardsCount.Where(x => x.Key == "J") is not null) {
            jackCount = this.CardsCount.FirstOrDefault(x => x.Key == "J").Value;
        }

        this.CardsCount.Remove("J");
        if (jackCount == 5) {
            this.TypeOfHand = (int)(HandType.FiveOfAKind);
            return;
        }

        if (this.CardsCount.Any(x => x.Value + jackCount == 5)) {
            this.TypeOfHand = (int)(HandType.FiveOfAKind);
        }
        else if (this.CardsCount.Any(x => x.Value + jackCount == 4)) {
            this.TypeOfHand = (int)(HandType.FourOfAKind);
        }
        else if (this.CardsCount.Any(x => x.Value + jackCount == 3) && this.CardsCount.Count == 2) {
            this.TypeOfHand = (int)(HandType.FullHouse);
        }
        else if (this.CardsCount.Any(x => x.Value + jackCount == 3) && this.CardsCount.Count == 3) {
            this.TypeOfHand = (int)(HandType.ThreeOfAKind);
        }
        else if (this.CardsCount.Count(x => x.Value + jackCount == 2) == 2) {
            this.TypeOfHand = (int)(HandType.TwoPair);
        }
        else if (this.CardsCount.Any(x => x.Value + jackCount == 2)) {
            this.TypeOfHand = (int)(HandType.OnePair);
        }
        else if (this.Cards.Distinct().Count() == 5) {
            this.TypeOfHand = (int)(HandType.HighCard);
        }
    }
}
