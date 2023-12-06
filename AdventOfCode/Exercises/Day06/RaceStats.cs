using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Exercises.Day06;
public class RaceStats {
    public RaceStats(int time, int distance) {
        this.Time = time;
        this.Distance = distance;
    }

    public int Time { get; set; } = 0;
    public int Distance { get; set; } = 0;
    public int NumberOfWaysToBeatRecord  { get; set; } = 0;
}
