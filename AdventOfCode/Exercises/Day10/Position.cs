
using FluentAssertions.Equivalency;
using System.Data.Common;

namespace AdventOfCode.Exercises.Day10;
public class Position {

    public Position(int row, int column) {

        this.Row = row;
        this.Column = column;
    }

    public Position(Position position) {

        this.Row = position.Row;
        this.Column = position.Column;
    }

    public int Row { get; set; }
    public int Column { get; set; }

    public bool IsSame(Position position) => position.Row == this.Row && position.Column == this.Column;

    public void Update(int row, int column) {
        this.Row = row;
        this.Column = column;
    }
}