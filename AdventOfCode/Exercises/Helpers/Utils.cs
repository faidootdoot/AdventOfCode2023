namespace AdventOfCode.Exercises.Helpers;
public static class Utils {

    /// <summary>
    /// Get Id based on the format "sometext {Id}:"
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static int GetId(string line, string lineTitle) {
        return int.Parse(line.ToLower()[..line.IndexOf(":")].Replace(lineTitle.ToLower(), "").Replace(":", "").Trim());
    }

    /// <summary>
    /// Get the line of text after the line prefix e.g. Games 1: a b c will return a b c 
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static string RemoveLinePrefix(string line) {
        return line.ToLower()[(line.IndexOf(":") + 1)..].Trim();

    }
}
