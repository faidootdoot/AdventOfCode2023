namespace AdventOfCode.Exercises.Day06;
public class Day06 {


    public void RunPart1() {

        List<RaceStats> raceStatsList = new() { new(51, 222), new(92, 2031), new(68, 1126), new(90, 1225) };

        foreach (RaceStats race in raceStatsList) {

            int ways = 0;
            for (int holdButtonTime = 0; holdButtonTime <= race.Time; holdButtonTime++) {
                var timeRemaining = race.Time - holdButtonTime;
                var distanceTravelled = timeRemaining * holdButtonTime;
                if (distanceTravelled > race.Distance) {
                    ways++;
                }
            }
            race.NumberOfWaysToBeatRecord = ways;
        }

        var mutipliedOutput = 1;
        foreach (RaceStats race in raceStatsList) {
            mutipliedOutput *= race.NumberOfWaysToBeatRecord;
        }
        Console.WriteLine("PART 1");
        Console.WriteLine($"Mulitplied output is {mutipliedOutput}");

    }
    public void RunPart2() {

        long time = 51926890;
        long distance = 222203111261225;
        long ways = 0; 

        for (int holdButtonTime = 0; holdButtonTime <= time; holdButtonTime++) {
            var timeRemaining = time - holdButtonTime;
            var distanceTravelled = timeRemaining * holdButtonTime;
            if (distanceTravelled > distance) {
                ways++;
            }
        }

        Console.WriteLine("PART 2");
        Console.WriteLine($"Ways is {ways}");
    }
}
