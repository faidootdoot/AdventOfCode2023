namespace AdventOfCode.Exercises.Day05;
public class Map {

    public Map(long destinationRangeStart, long sourceRangeStart, long rangeLength) {

        this.DestinationRangeStart = destinationRangeStart;
        this.SourceRangeStart = sourceRangeStart;
        this.RangeLength = rangeLength;
    }

    public long DestinationRangeStart { get; set; }
    public long SourceRangeStart { get; set; }
    public long RangeLength { get; set; }


}
