﻿
using FluentAssertions;
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
    private List<long> seeds = new();
    private List<Map> seedToSoilMap = new();
    private List<Map> soilToFertilizerMap = new();
    private List<Map> fertilizerToWaterMap = new();
    private List<Map> waterToLightMap = new();
    private List<Map> lightToTemperatureMap = new();
    private List<Map> temperatureToHumidityMap = new();
    private List<Map> humidityToLocationMap = new();

    public Day05() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();

        this.ReadDataIntoMaps(lines);
        this.OrderAllMaps();
    }

    public void RunPart1() {

        List<long> locations = new();

        foreach (var seed in this.seeds) {

            var soil = this.GetDestinationFromMap(seed, this.seedToSoilMap);
            var fertilizer = this.GetDestinationFromMap(soil, this.soilToFertilizerMap);
            var water = this.GetDestinationFromMap(fertilizer, this.fertilizerToWaterMap);
            var light = this.GetDestinationFromMap(water, this.waterToLightMap);
            var temperature = this.GetDestinationFromMap(light, this.lightToTemperatureMap);
            var humidity = this.GetDestinationFromMap(temperature, this.temperatureToHumidityMap);
            var location = this.GetDestinationFromMap(humidity, this.humidityToLocationMap);

            locations.Add(location);
        }
        var minLocation = locations.Min();
        Console.WriteLine("PART 1");
        Console.WriteLine($"Lowest location is {minLocation}");
    }

    public void RunPart2() {

        
        Console.WriteLine("PART 2");
    }

    #region Helpers

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
    private void OrderAllMaps() {
        this.seedToSoilMap = this.seedToSoilMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.soilToFertilizerMap = this.soilToFertilizerMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.fertilizerToWaterMap = this.fertilizerToWaterMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.waterToLightMap = this.waterToLightMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.lightToTemperatureMap = this.lightToTemperatureMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.temperatureToHumidityMap = this.temperatureToHumidityMap.OrderBy(m => m.SourceRangeStart).ToList();
        this.humidityToLocationMap = this.humidityToLocationMap.OrderBy(m => m.SourceRangeStart).ToList();
    }

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
    #endregion
}