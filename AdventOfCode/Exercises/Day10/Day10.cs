
using static AdventOfCode.Exercises.Day10.Day10;
using System.Data.Common;
using FluentAssertions.Data;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Exercises.Day10;
public class Day10 {


    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day10\\Data.txt");
    private readonly char[,] tileMap;
    private readonly List<string> lines;
    private readonly List<Position> path = new();
    private readonly Position startPosition;
    private readonly int maxColum;
    private readonly int maxRow;
    public enum Direction {
        North,
        East,
        South,
        West
    }

    public const char startTile = 'S';
    public const string CompatibleNorthTiles = "|7F";
    public const string CompatibleSouthTiles = "|LJ";
    public const string CompatibleEastTiles = "-7J";
    public const string CompatibleWestTiles = "-LF";

    public class CompatibleTile : Dictionary<Direction, string> {
        public CompatibleTile() {
            this.Add(Direction.North, string.Empty);
            this.Add(Direction.East, string.Empty);
            this.Add(Direction.South, string.Empty);
            this.Add(Direction.West, string.Empty);
        }
    }


    private const string NorthSouth = "|";

    /*| is a vertical pipe connecting north and south.
- is a horizontal pipe connecting east and west.
L is a 90-degree bend connecting north and east.
J is a 90-degree bend connecting north and west.
7 is a 90-degree bend connecting south and west.
F is a 90-degree bend connecting south and east.
. is ground; there is no pipe in this tile.
S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
    */


    public Day10() {

        this.lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        this.tileMap = new char[this.lines.Count, this.lines.FirstOrDefault()!.Length];

        for (int row = 0; row < this.lines.Count; row++) {

            var line = this.lines[row];
            for (int column = 0; column < line.Count(); column++) {
                this.tileMap[row, column] = line[column];
            }
        }

        this.startPosition = this.FindStartPositionTileMap();
        this.maxRow = this.tileMap.GetLength(0);
        this.maxColum = this.tileMap.GetLength(1);

    }

    public void RunPart1() {
        this.FindPath();

        var steps = this.path.Count / 2;

        Console.WriteLine($"PART 1");
        Console.WriteLine($"Number of steps is {steps}");
    }

    public void RunPart2() {
        Console.WriteLine("RunPart1 to do");
    }


    #region Helpers
    /// <summary>
    /// Find the start position in the map
    /// </summary>
    /// <returns></returns>
    public Position FindStartPositionTileMap() {

        // ln: 59, ch: 52
        for (int row = 0; row < this.tileMap.GetLength(0); row++) {
            for (int column = 0; column < this.tileMap.GetLength(1); column++) {
                if (this.tileMap[row, column] == startTile) {
                    return new Position(row, column);
                }
            }
        }

        return new Position(-1, -1);
    }

    /// <summary>
    /// Find the path
    /// </summary>
    public void FindPath() {

        Position previousPosition = new(this.startPosition);
        Position currentPosition = new(this.startPosition);
        Position? nextPosition;

        this.path.Add(this.startPosition);

        do {
            nextPosition = this.GetNextPosition(currentPosition);
            if (nextPosition != null) {
                this.path.Add(nextPosition);
                previousPosition.Update(currentPosition.Row, currentPosition.Column);
                currentPosition.Update(nextPosition.Row, nextPosition.Column);
            }
        } while (nextPosition != null);

        this.path.ForEach(position => Console.WriteLine($"Position: [{position.Row}, {position.Column}]"));
        this.PrintRoutedMap();

    }

    /// <summary>
    /// Get the next position 
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    private Position? GetNextPosition(Position currentPosition) {

        var tile = this.tileMap[currentPosition.Row, currentPosition.Column];
        Position? nextPosition = null;
        if (tile == 'S') {
            nextPosition = this.CheckNorthPosition(currentPosition);
            nextPosition ??= this.CheckSouthPosition(currentPosition);
            nextPosition ??= this.CheckEastPosition(currentPosition);
            nextPosition ??= this.CheckWestPosition(currentPosition);
        }
        if (tile == '|') {
            nextPosition = this.CheckNorthPosition(currentPosition);
            nextPosition ??= this.CheckSouthPosition(currentPosition);
        }
        if (tile == '-') {
            nextPosition = this.CheckEastPosition(currentPosition);
            nextPosition ??= this.CheckWestPosition(currentPosition);
        }
        if (tile == 'L') {
            nextPosition = this.CheckNorthPosition(currentPosition);
            nextPosition ??= this.CheckEastPosition(currentPosition);
        }
        if (tile == 'J') {
            nextPosition = this.CheckNorthPosition(currentPosition);
            nextPosition ??= this.CheckWestPosition(currentPosition);
        }
        if (tile == 'F') {
            nextPosition = this.CheckEastPosition(currentPosition);
            nextPosition ??= this.CheckSouthPosition(currentPosition);
        }
        if (tile == '7') {
            nextPosition = this.CheckWestPosition(currentPosition);
            nextPosition ??= this.CheckSouthPosition(currentPosition);
        }
        return nextPosition;
    }

    /// <summary>
    /// Check north tile is compatible
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    private Position? CheckNorthPosition(Position currentPosition) {

        // Check North
        if (currentPosition.Row is not 0) {

            var northPosition = new Position(currentPosition.Row - 1, currentPosition.Column);
            if (!this.path.Any(position => position.IsSame(northPosition))) {

                var northTile = this.tileMap[currentPosition.Row - 1, currentPosition.Column];
                if (CompatibleNorthTiles.Contains(northTile)) {
                    return northPosition;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Check south tile is compatible
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    private Position? CheckSouthPosition(Position currentPosition) {

        // Check South
        if (currentPosition.Row != this.maxRow) {

            var southPosition = new Position(currentPosition.Row + 1, currentPosition.Column);
            if (!this.path.Any(position => position.IsSame(southPosition))) {
                var southTile = this.tileMap[currentPosition.Row + 1, currentPosition.Column];
                if (CompatibleSouthTiles.Contains(southTile)) {
                    return southPosition;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Check east tile is compatible
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    private Position? CheckEastPosition(Position currentPosition) {
        if (currentPosition.Column != this.maxColum) {

            var eastPosition = new Position(currentPosition.Row, currentPosition.Column + 1);
            if (!this.path.Any(position => position.IsSame(eastPosition))) {
                var eastTile = this.tileMap[currentPosition.Row, currentPosition.Column + 1];
                if (CompatibleEastTiles.Contains(eastTile)) {
                    return eastPosition;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Check west tile is compatible
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    private Position? CheckWestPosition(Position currentPosition) {
        // Check West
        if (currentPosition.Column is not 0) {

            var westPosition = new Position(currentPosition.Row, currentPosition.Column - 1);
            if (!this.path.Any(position => position.IsSame(westPosition))) {
                var westTile = this.tileMap[currentPosition.Row, currentPosition.Column - 1];
                if (CompatibleWestTiles.Contains(westTile)) {
                    return westPosition;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Print out the whole map
    /// </summary>
    public void PrintMap() {

        this.lines.ForEach(line => Console.WriteLine(line));
    }

    public void PrintRoutedMap() {

        List<string> routedMap = new();
        this.lines.ForEach(line => routedMap.Add(new string(' ', line.Length)));


        foreach (var position in this.path) {
            routedMap[position.Row] = routedMap[position.Row].Insert(position.Column, this.lines.ElementAt(position.Row).ElementAt(position.Column).ToString());
            routedMap.ElementAt(position.Row).Remove(position.Column);
        }

        routedMap.ForEach(line => Console.WriteLine(line));
    }
    #endregion
}
