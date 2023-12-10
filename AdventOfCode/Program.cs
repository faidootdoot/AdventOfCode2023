// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Reflection;

int day = 9;

string className = $"Day{day.ToString().PadLeft(2, '0')}";
Console.WriteLine(className);

Stopwatch stopwatch = Stopwatch.StartNew();

Type? classType = Assembly.GetExecutingAssembly().GetType($"AdventOfCode.Exercises.{className}.{className}");
if (classType != null) {

    object? instance = Activator.CreateInstance(classType);
    if (instance != null) {
        classType.GetMethod("RunPart1")?.Invoke(instance, null);
        classType.GetMethod("RunPart2")?.Invoke(instance, null);
    }
}

stopwatch.Stop();
TimeSpan timespan = stopwatch.Elapsed;

Console.WriteLine($"{String.Format("{0:00}:{1:00}:{2:00}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10)}");