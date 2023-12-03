using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Exercises.Day02;
public class Day02 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day02\\Data.txt");

    private readonly List<Game> games = new();

    public Day02() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        foreach (var line in lines) {

            this.games.Add(new Game() {
                Id = GetGameId(line),
                Sets = GetSets(line)
            });
        }
    }

    /// <summary>
    /// Determine which games would have been possible <br/>
    /// if the bag had been loaded with only 12 red cubes, 13 green cubes, and 14 blue cubes. <br/>
    /// What is the sum of the IDs of those games?
    /// </summary>
    public void RunPart1() {

        var total = this.games.Where(game => game.Sets.Where(set => set.RedCount <= 12 && set.GreenCount <= 13 && set.BlueCount <= 14)?.Count() == game.Sets.Count)
                              .Select(game => game.Id)
                              .Sum();

        Console.WriteLine("Part 1");
        Console.WriteLine($"Sum is {total}");
    }

    public void RunPart2() {

        var total = this.games.Select(game => game.Sets.Max(set => set.RedCount) * game.Sets.Max(set => set.GreenCount) * game.Sets.Max(set => set.BlueCount)).Sum();

        Console.WriteLine("Part 2");
        Console.WriteLine($"Sum is {total}");
    }

    #region Helpers

    /// <summary>
    /// Gets the game Id from a line of text
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static int GetGameId(string line) {
        return int.Parse(line.ToLower()[..line.IndexOf(":")].Replace("game", "").Replace(":", "").Trim());
    }

    /// <summary>
    /// Get all the sets for a game that is defined by a line of text<br/>
    /// Not particularly safe code with assumptions
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private static List<Set> GetSets(string line) {

        var sets = new List<Set>();
        var startIndex = line.IndexOf(":") + 1;
        var setsArray = line[startIndex..].Split(";");

        foreach (var setText in setsArray) {
            var ballsArray = setText.Split(",");

            var set = new Set();
            foreach (var ball in ballsArray) {

                if (ball.Contains("red", StringComparison.CurrentCultureIgnoreCase)) {
                    set.RedCount = int.Parse(ball[..ball.IndexOf("red", StringComparison.CurrentCultureIgnoreCase)].Trim());
                }
                else if (ball.Contains("green", StringComparison.CurrentCultureIgnoreCase)) {
                    set.GreenCount = int.Parse(ball[..ball.IndexOf("green", StringComparison.CurrentCultureIgnoreCase)].Trim());
                }
                else if (ball.Contains("blue", StringComparison.CurrentCultureIgnoreCase)) {
                    set.BlueCount = int.Parse(ball[..ball.IndexOf("blue", StringComparison.CurrentCultureIgnoreCase)].Trim());
                }
            }

            sets.Add(set);
        }

        return sets;
    }

    #endregion
}



