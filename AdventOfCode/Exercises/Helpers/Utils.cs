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
}
