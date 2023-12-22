
namespace AdventOfCode.Exercises.Day13;
public class Day13 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day13\\Data.txt");
    private readonly List<string> lines;
    private readonly List<Frame> frames = new();


    public Day13() {

        this.lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        int startRow = 0;
        int row = 0;
        do {

            // Check is the next line is empty
            if (row == this.lines.Count - 1 || string.IsNullOrEmpty(this.lines[row + 1])) {
                this.frames.Add(new Frame() { lines = this.lines.GetRange(startRow, row - startRow + 1) });
                startRow = row + 2;
            }
            row++;

        } while (row < this.lines.Count);

        this.PrintFrames();
    }

    private List<string> TransposeLines(Frame frame) {

        int maxColumn = frame.lines.FirstOrDefault()!.Length;
        List<string> transposedLines = new();
        for (int i = 0; i < maxColumn; i++) {

            string columnText = string.Empty;
            foreach (var line in frame.lines) {
                columnText+=line[i].ToString();
            }
            transposedLines.Add(columnText);
        }
        return transposedLines;
    }

    private (int lower, int upper) IdentifyMatchingRows(Frame frame) {

        int upper = 0;
        int lower = 0;
        string line;
        bool hasMatchingRow = false;

        for (int row = 0; row < frame.lines.Count; row++) {

            lower = row;
            line = frame.lines[row];
            upper = frame.lines.FindIndex(row + 1, l => l.Contains(line));
            if (upper >= 0) {
                var reflectionRow = this.IdentifyReflectionRow(lower, upper);
                hasMatchingRow = IsHorizontalReflectionReal(frame, reflectionRow.lower, reflectionRow.upper);
                if (hasMatchingRow) {
                    break;
                }
            }
        }

        return hasMatchingRow ? (lower, upper) : (-1, -1);
    }

    private (int lower, int upper) IdentifyReflectionRow(int lower, int upper) {

        int half = 0;
        if (lower >= 0 && upper >= 0) {
            half = (upper - lower) / 2;
        }
        int reflectionRowLower = lower + half;
        int reflectionRowUpper = upper - half;
        return (reflectionRowLower, reflectionRowUpper);
    }

    private static bool IsHorizontalReflectionReal(Frame frame, int reflectionRowLower, int reflectionRowUpper) {

        if (reflectionRowLower < 0 || reflectionRowUpper < 0) {
            return false;
        }

        var lowerList = frame.lines.GetRange(0, reflectionRowLower + 1);
        var upperList = frame.lines.GetRange(reflectionRowUpper, frame.lines.Count - reflectionRowUpper);

        var length = lowerList.Count <= upperList.Count ? lowerList.Count : upperList.Count;
        lowerList = lowerList.GetRange(lowerList.Count - length, length);
        upperList = upperList.GetRange(0, length);
        upperList.Reverse();
        var isMatch = upperList.SequenceEqual(lowerList);
        return isMatch;
    }

    private void PrintFrames() {
        foreach (var frame in this.frames) {
            Console.WriteLine();
            frame.lines.ForEach(line => Console.WriteLine(line));
        }
    }

    private static void PrintUpperLower(int lower, int upper) {

        Console.WriteLine();
        Console.WriteLine($"{lower} : {upper}");
    }


    public void RunPart1() {

        int columnsCount = 0;
        int rowsCount = 0;
        bool columnMode;
        foreach (var frame in this.frames) {

            columnMode = false;
            var matchingRows = this.IdentifyMatchingRows(frame);

            // transpose the rows if no row matches and repeat - this is checking for matching columns
            if(matchingRows.lower < 0 || matchingRows.upper < 0) {
                frame.lines = this.TransposeLines(frame);
                matchingRows = this.IdentifyMatchingRows(frame);
                columnMode = true;
            }

            var reflectionRow = this.IdentifyReflectionRow(matchingRows.lower, matchingRows.upper);

            var count = reflectionRow.lower + 1;
            if (columnMode) {
                columnsCount += count;
            }
            else {
                rowsCount += count;
            }

            PrintUpperLower(reflectionRow.lower, reflectionRow.upper);
        }

        long total = columnsCount + 100 * rowsCount;
        Console.WriteLine("PART 1");
        Console.WriteLine($"Total is {total}");
    }

    public void RunPart2() {
        Console.WriteLine("PART 2 - WIP");
    }
}
