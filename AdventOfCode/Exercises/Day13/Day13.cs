
namespace AdventOfCode.Exercises.Day13;
public class Day13 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day13\\Data.txt");
    private readonly List<List<string>> frames = new();
    
    private void ReadData() {

        this.frames.Clear();
        var listOfBlocks = File.ReadAllText(this.filePath.AbsolutePath).Split("\r\n\r\n").ToList();

        foreach (var block in listOfBlocks) {
            var lines = block.Split("\r\n").ToList();
            this.frames.Add(lines);
        }
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


    private static int IdentifyMirror(List<string> lines) {

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

    /// <summary>
    /// Smudge is when the mirrors reflect and all match except one character
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    private static int IdentifyMirrorWithSmudge(List<string> lines) {

        List<string> above;
        List<string> below;
        int sumOfMismatches = 0;
        for (int row = 1; row < lines.Count; row++) {

            var length = row <= lines.Count - row ? row : lines.Count - row;

            above = lines.GetRange(0, row);
            above.Reverse();
            above = above.GetRange(0, length);
            below = lines.GetRange(row, length);

            // Iterate through the list line by line
            for (int listIndex = 0; listIndex < length; listIndex++) {

                var aboveLine = above[listIndex].ToArray();
                var belowLine = below[listIndex].ToArray();

                // Compare character by character 
                for (int charIndex = 0; charIndex < aboveLine.Length; charIndex++) {

                    // Keep a sum of mismatches
                    if (aboveLine[charIndex] != belowLine[charIndex]) {
                        sumOfMismatches++;
                    }
                }
            }

            if (sumOfMismatches == 1) {
                return row;
            }
            else {
                sumOfMismatches = 0;
            }
        }
        return 0;
    }

    public void RunPart1() {

        this.ReadData();

        long total = 0;
        foreach (var frame in this.frames) {

            int row = IdentifyMirror(frame);
            total += row * 100;

            if (row == 0) {
                int column = IdentifyMirror(TransposeLines(frame));
                total += column;
            }
        }

        Console.WriteLine("PART 1");
        Console.WriteLine($"Total is {total}");
    }

    public void RunPart2() {

        this.ReadData();

        long total = 0;
        foreach (var frame in this.frames) {

            int row = IdentifyMirrorWithSmudge(frame);
            total += row * 100;

            if (row == 0) {
                int column = IdentifyMirrorWithSmudge(TransposeLines(frame));
                total += column;
            }
        }

        Console.WriteLine("PART 2");
        Console.WriteLine($"Total is {total}");
    }
}
