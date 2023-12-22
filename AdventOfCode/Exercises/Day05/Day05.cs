
using System.Data;

namespace AdventOfCode.Exercises.Day05;
public class Day05 {

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day05\\Data.txt");
    private List<long> seeds = new();
    private List<Map> seedToSoilMap = new();
    private List<Map> soilToFertilizerMap = new();
    private List<Map> fertilizerToWaterMap = new();
    private List<Map> waterToLightMap = new();
    private List<Map> lightToTemperatureMap = new();
    private List<Map> temperatureToHumidityMap = new();
    private List<Map> humidityToLocationMap = new();
    private readonly List<FullMap> fullMapList = new();

    public Day05() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();

        this.ReadDataIntoMaps(lines);
        //this.PopulateFullMap();
        this.OrderAllMaps();
    }

    public void RunPart1() {

        List<long> locations = new();

        long minimumLocation = 0;
        foreach (var seed in this.seeds) {

            var location = this.GetLocationFromSeed(seed);
            if (seed == this.seeds.First()) {
                minimumLocation = location;
            }
            minimumLocation = location < minimumLocation ? location : minimumLocation;
        }
        Console.WriteLine("PART 1");
        Console.WriteLine($"Lowest location is {minimumLocation}");
    }

    public void RunPart2() {

        int index = 0;
        long minimumLocation = this.GetLocationFromSeed(this.seeds[0]);
        while (index < this.seeds.Count) {

            var seedRangeStart = this.seeds[index];
            var seedRangeLength = this.seeds[++index];
            index++;

            for (int i = 0; i < seedRangeLength; i++) {

                var seed = seedRangeStart + i;
                var location = this.GetLocationFromSeed(seed);
                minimumLocation = location < minimumLocation ? location : minimumLocation;
                Console.WriteLine($"{location}\t{minimumLocation}");
            }
        }

        Console.WriteLine("PART 2");
        Console.WriteLine($"Lowest location is {minimumLocation}");
    }

    public void RunPart2_1() {

        int index = 0;
        long minimumLocation = this.GetLocationFromSeed(this.seeds[0]);
        while (index < this.seeds.Count) {

            var seedRangeStart = this.seeds[index];
            var seedRangeLength = this.seeds[++index];
            index++;

            for (int i = 0; i < seedRangeLength; i++) {

                var seed = seedRangeStart + i;
                var location = this.GetLocationFromSeed(seed);
                minimumLocation = location < minimumLocation ? location : minimumLocation;
                Console.WriteLine($"{location}\t{minimumLocation}");
            }
        }

        Console.WriteLine("PART 2");
        Console.WriteLine($"Lowest location is {minimumLocation}");
    }

    #region Helpers

    private void PopulateFullMap() {

        // Seed to soil
        foreach (var mapData in this.seedToSoilMap) {

            for (int i = 0; i < mapData.RangeLength; i++) {

                this.fullMapList.Add(new FullMap() {
                    SeedNumber = mapData.SourceRangeStart + i,
                    SoilNumber = mapData.DestinationRangeStart + i,
                });
            }
        }
    }

    /// <summary>
    /// Read all the input data to lists
    /// </summary>
    /// <param name="lines"></param>
    private void ReadDataIntoMaps(IEnumerable<string> lines) {
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

    /// <summary>
    /// Order all teh maps to be in ascending order so that it is easier to read
    /// </summary>
    private void OrderAllMaps() {
        this.seedToSoilMap = this.seedToSoilMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.soilToFertilizerMap = this.soilToFertilizerMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.fertilizerToWaterMap = this.fertilizerToWaterMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.waterToLightMap = this.waterToLightMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.lightToTemperatureMap = this.lightToTemperatureMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.temperatureToHumidityMap = this.temperatureToHumidityMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.humidityToLocationMap = this.humidityToLocationMap.OrderBy(m => m.SourceRangeStart).ToList();
    }

    /// <summary>
    /// Get the destination from the map
    /// </summary>
    /// <param name="sourceValue"></param>
    /// <param name="dataList"></param>
    /// <returns></returns>
    private long GetDestinationFromMap(long sourceValue, List<Map> dataList) {

        var match = dataList.FirstOrDefault(map => sourceValue >= map.SourceRangeStart && sourceValue < map.SourceRangeStart + map.RangeLength - 1);

        long destination;
        if (match == null) {
            destination = sourceValue;
        }
        else {
            destination = sourceValue - match!.SourceRangeStart + match.DestinationRangeStart;
        }
        return destination;
    }

    /// <summary>
    /// Get the Location from the seed
    /// </summary>
    /// <param name="seed"></param>
    /// <returns></returns>
    private long GetLocationFromSeed(long seed) {

        var soil = this.GetDestinationFromMap(seed, this.seedToSoilMap);
        var fertilizer = this.GetDestinationFromMap(soil, this.soilToFertilizerMap);
        var water = this.GetDestinationFromMap(fertilizer, this.fertilizerToWaterMap);
        var light = this.GetDestinationFromMap(water, this.waterToLightMap);
        var temperature = this.GetDestinationFromMap(light, this.lightToTemperatureMap);
        var humidity = this.GetDestinationFromMap(temperature, this.temperatureToHumidityMap);
        var location = this.GetDestinationFromMap(humidity, this.humidityToLocationMap);

        return location;
    }

    private long GetLocationFromSeed_Opt1(long seed) {

        var soil = this.GetDestinationFromMap(seed, this.seedToSoilMap);
        var fertilizer = this.GetDestinationFromMap(soil, this.soilToFertilizerMap);
        var water = this.GetDestinationFromMap(fertilizer, this.fertilizerToWaterMap);
        var light = this.GetDestinationFromMap(water, this.waterToLightMap);
        var temperature = this.GetDestinationFromMap(light, this.lightToTemperatureMap);
        var humidity = this.GetDestinationFromMap(temperature, this.temperatureToHumidityMap);
        var location = this.GetDestinationFromMap(humidity, this.humidityToLocationMap);

        return location;
    }
    #endregion
}
