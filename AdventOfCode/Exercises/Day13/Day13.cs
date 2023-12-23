
namespace AdventOfCode.Exercises.Day13;
public class Day13 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day13\\DataExample.txt");
    private List<string> lines = new();
    private readonly List<Frame> frames = new();

    private enum Mode {
        RowMode,
        ColumnMode
    }

    private void ReadData() {

        this.lines.Clear();
        this.frames.Clear();
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

    }

    private static List<string> TransposeLines(List<string> lines) {

        int maxColumn = lines.FirstOrDefault()!.Length;
        List<string> transposedLines = new();
        for (int i = 0; i < maxColumn; i++) {

            string columnText = string.Empty;
            foreach (var line in lines) {
                columnText += line[i].ToString();
            }
            transposedLines.Add(columnText);
        }
        return transposedLines;
    }

    private static List<string> TransposeLines(Frame frame) {

        int maxColumn = frame.lines.FirstOrDefault()!.Length;
        List<string> transposedLines = new();
        for (int i = 0; i < maxColumn; i++) {

            string columnText = string.Empty;
            foreach (var line in frame.lines) {
                columnText += line[i].ToString();
            }
            transposedLines.Add(columnText);
        }
        return transposedLines;
    }

    private static (int lower, int upper) IdentifyMatchingRows(Frame frame) {

        int upper = 0;
        int lower = 0;
        string line;
        bool hasMatchingRow = false;

        for (int row = 0; row < frame.lines.Count; row++) {

            lower = row;
            line = frame.lines[row];
            upper = frame.lines.FindIndex(row + 1, l => l.Contains(line));
            if (upper >= 0) {
                var reflectionRow = IdentifyReflectionRow(lower, upper);
                hasMatchingRow = IsHorizontalReflectionReal(frame, reflectionRow.lower, reflectionRow.upper);
                if (hasMatchingRow) {
                    break;
                }
            }
        }

        return hasMatchingRow ? (lower, upper) : (-1, -1);
    }

    private static (int lower, int upper) IdentifyMatchingRowsWithSmuding(Frame frame, (int lower, int upper) originalMatchingRows) {

        bool isMatch;
        (int lower, int upper) matchingRows = (-1, -1);
        for (int lineIndex = 0; lineIndex < frame.lines.Count; lineIndex++) {

            var line = frame.lines.ElementAt(lineIndex);

            isMatch = false;
            int charIndex = 0;
            do {
                var newLine = line.ToArray();
                var currentChar = line[charIndex];
                var alternativeCharacter = AlternativeCharacter(currentChar);
                newLine[charIndex] = alternativeCharacter;
                frame.lines[lineIndex] = new string(newLine);

                //Console.WriteLine("\r\n Updated frame");
                // PrintFrame(frame);

                matchingRows = IdentifyMatchingRows(frame);
                if ((matchingRows.lower < 0 || matchingRows.upper < 0) || (originalMatchingRows.lower == matchingRows.lower && originalMatchingRows.upper == matchingRows.upper)) {
                    frame.lines[lineIndex] = line;
                    charIndex++;
                }
                else {
                    isMatch = true;
                }

            } while (!isMatch
            && charIndex < line.Length
            && originalMatchingRows.lower == matchingRows.lower
            && originalMatchingRows.upper == matchingRows.upper);

            if (isMatch) {
                break;
            }
        }
        return (matchingRows.lower, matchingRows.upper);
    }
    private static (int lower, int upper) IdentifyReflectionRow(int lower, int upper) {

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

    private static void PrintFrame(Frame frame) {
        frame.lines.ForEach(line => Console.WriteLine(line));

    }

    private static void PrintUpperLower(int lower, int upper) {

        Console.WriteLine();
        Console.WriteLine($"{lower} : {upper}");
    }

    private static string AlternativeString(string line, int charIndex) {

        var character = line[charIndex];
        var altCharacter = AlternativeCharacter(character);
        var newLine = line.ToArray();
        newLine[charIndex] = altCharacter;
        return newLine.ToString()!;
    }

    private static char AlternativeCharacter(char character) => character.Equals('#') ? '.' : '#';

    private int IdentifyMirror(List<string> lines) {

        List<string> above;
        List<string> below;

        for (int row = 1; row < lines.Count; row++) {

            var length = row <= lines.Count - row ? row : lines.Count - row;

            above = lines.GetRange(0, row);
            above.Reverse();
            above = above.GetRange(0, length);
            below = lines.GetRange(row, length);

            if (above.SequenceEqual(below)) {
                return row;
            }
        }
        return 0;
    }

    public void RunPart1() {

        this.ReadData();

        long total = 0;
        foreach (var frame in this.frames) {

            int row = this.IdentifyMirror(frame.lines);
            total += row * 100;

            if (row == 0) {
                int column = this.IdentifyMirror(TransposeLines(frame.lines));
                total += column;
            }
        }

        Console.WriteLine("PART 1");
        Console.WriteLine($"Total is {total}");
    }

    public void RunPart11() {

        int columnsCount = 0;
        int rowsCount = 0;
        bool columnMode;

        this.ReadData();

        foreach (var frame in this.frames) {

            columnMode = false;
            var matchingRows = IdentifyMatchingRows(frame);

            // transpose the rows if no row matches and repeat - this is checking for matching columns
            if (matchingRows.lower < 0 || matchingRows.upper < 0) {
                frame.lines = TransposeLines(frame);
                matchingRows = IdentifyMatchingRows(frame);
                columnMode = true;
            }

            var reflectionRow = IdentifyReflectionRow(matchingRows.lower, matchingRows.upper);

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

    private static Mode IdentifyMode(int matchingRowLower, int matchingRowUpper) {

        if (matchingRowLower < 0 || matchingRowUpper < 0) {
            return Mode.ColumnMode;
        }
        else {
            return Mode.RowMode;
        }
    }

    public void RunPart21() {

        int columnsCount = 0;
        int rowsCount = 0;
        (int lower, int upper) matchingRows;
        (int lower, int upper) originalMatchingRows;
        (int lower, int upper) reflectionRow;

        this.ReadData();
        Console.WriteLine("PART 2");

        foreach (var frame in this.frames) {

            // Identify the original matching rows
            originalMatchingRows = IdentifyMatchingRows(frame);

            // Identify the matching rows with smudging
            matchingRows = IdentifyMatchingRowsWithSmuding(frame, originalMatchingRows);

            // Identify the mode
            var mode = IdentifyMode(matchingRows.lower, matchingRows.upper);

            // Identify the matching columns
            if (mode == Mode.ColumnMode) {
                frame.lines = TransposeLines(frame);
                originalMatchingRows = IdentifyMatchingRows(frame);
                matchingRows = IdentifyMatchingRowsWithSmuding(frame, originalMatchingRows);
            }

            // Identify the reflection row
            if (matchingRows.lower < 0) {
                reflectionRow = IdentifyReflectionRow(originalMatchingRows.lower, originalMatchingRows.upper);
            }
            else {
                reflectionRow = IdentifyReflectionRow(matchingRows.lower, matchingRows.upper);
            }

            var count = reflectionRow.lower + 1;
            switch (mode) {
                case Mode.RowMode:
                    rowsCount += count;
                    break;
                case Mode.ColumnMode:
                    columnsCount += count;
                    break;
                default:
                    break;
            }
            PrintUpperLower(reflectionRow.lower, reflectionRow.upper);

        }

        long total = columnsCount + 100 * rowsCount;
        Console.WriteLine($"Total is {total}");
    }


    public void RunPart2() {

        int columnsCount = 0;
        int rowsCount = 0;
        (int lower, int upper) matchingRows;
        (int lower, int upper) originalMatchingRows;
        (int lower, int upper) reflectionRow;

        this.ReadData();
        Console.WriteLine("PART 2");

        for (int i = 0; i < this.frames.Count; i++) {

            var frame = this.frames[i];

            // Identify the original matching rows
            originalMatchingRows = IdentifyMatchingRows(frame);

            // Identify the matching rows with smidging
            matchingRows = IdentifyMatchingRowsWithSmuding(frame, originalMatchingRows);

            // Identify the mode
            var mode = IdentifyMode(matchingRows.lower, matchingRows.upper);

            // Identify the matching columns
            if (mode == Mode.ColumnMode) {
                frame.lines = TransposeLines(frame);
                originalMatchingRows = IdentifyMatchingRows(frame);
                matchingRows = IdentifyMatchingRowsWithSmuding(frame, originalMatchingRows);
            }

            // Identify the reflection row         
            reflectionRow = IdentifyReflectionRow(matchingRows.lower, matchingRows.upper);

            //var reflectionRow = IdentifyReflectionRow(matchingRows.lower, matchingRows.upper);

            var count = reflectionRow.lower + 1;
            switch (mode) {
                case Mode.RowMode:
                    rowsCount += count;
                    break;
                case Mode.ColumnMode:
                    columnsCount += count;
                    break;
                default:
                    break;
            }
            PrintUpperLower(reflectionRow.lower, reflectionRow.upper);
        }

        long total = columnsCount + 100 * rowsCount;
        Console.WriteLine($"Total is {total}");
    }
}
