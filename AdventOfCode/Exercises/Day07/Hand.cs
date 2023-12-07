using System.Text.RegularExpressions;
using static AdventOfCode.Exercises.Day07.Constants;

namespace AdventOfCode.Exercises.Day07;

public class Hand {

    public Hand(string cards, int bid) {
        this.Cards = cards;
        this.Bid = bid;
        this.CalculateCardValues();
        this.CalculateCardsCount();
        this.CalculateHandType();
    }

    public Dictionary<string, int> CardsCount { get; set; } = new();
    public List<int> CardValues { get; set; } = new();
    public string Cards { get; init; }
    public int Bid { get; init; }
    public int TypeOfHand { get; set; } = default!;

    private void CalculateCardValues() {

        foreach (var character in this.Cards) {
            this.CardValues.Add(this.GetCardValue(character));
        }
    }
    private int GetCardValue(char character) {
        return character switch {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => int.Parse(character.ToString()),
        };
    }

    private void CalculateCardsCount() {

        var distinctCards = this.Cards.Distinct().ToList();
        foreach (var character in distinctCards) {

            var key = character.ToString();
            int count = Regex.Matches(this.Cards, key).Count;
            this.CardsCount[key] = count;
        }
    }

    private void CalculateHandType() {

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
}
