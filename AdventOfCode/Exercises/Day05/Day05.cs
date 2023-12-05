
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode.Exercises.Day01.Day01;

namespace AdventOfCode.Exercises.Day05;
public class Day05 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day05\\Data.txt");
    private readonly List<long> seeds = new();
    private readonly List<Map> seedToSoilMap = new();
    private readonly List<Map> soilToFertilizerMap = new();
    private readonly List<Map> fertilizerToWaterMap = new();
    private readonly List<Map> waterToLightMap = new();
    private readonly List<Map> lightToTemperatureMap = new();
    private readonly List<Map> temperatureToHumidityMap = new();
    private readonly List<Map> humidityToLocationMap = new();


    public Day05() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        List<Map>? map = null;

        foreach (var line in lines) {
            if (line.StartsWith("seeds:")) {
                var seedText = Helpers.Utils.RemoveLinePrefix(line);
                this.seeds = seedText.Split(" ").Select(text => long.Parse(text)).ToList();
            }

            if (line.StartsWith("seed-to-soil map:")) {
                map = this.seedToSoilMap;
            }
            else if (line.StartsWith("soil-to-fertilizer map:")) {
                map = this.soilToFertilizerMap;
            }
            else if (line.StartsWith("fertilizer-to-water map:")) {
                map = this.fertilizerToWaterMap;
            }
            else if (line.StartsWith("water-to-light map:")) {
                map = this.waterToLightMap;
            }
            else if (line.StartsWith("light-to-temperature map:")) {
                map = this.lightToTemperatureMap;
            }
            else if (line.StartsWith("temperature-to-humidity map:")) {
                map = this.temperatureToHumidityMap;
            }
            else if (line.StartsWith("humidity-to-location map:")) {
                map = this.humidityToLocationMap;
            }

            if (map != null) {
                var data = line.Split(" ");
                if (data != null && data.Length == 3) {
                    var convertedData = data.Select(text => long.Parse(text)).ToList();
                    map.Add(new Map(convertedData[0], convertedData[1], convertedData[2]));
                }
            }
        }
    }

    public void RunPart1() {
        Console.WriteLine("PART 1");
    }

    public void RunPart2() {
        Console.WriteLine("PART 2");
    }
}
