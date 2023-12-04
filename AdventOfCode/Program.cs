// See https://aka.ms/new-console-template for more information
using System.Reflection;

int day = 4;

string className = $"Day{day.ToString().PadLeft(2, '0')}";
Console.WriteLine(className);

Type? classType = Assembly.GetExecutingAssembly().GetType($"AdventOfCode.Exercises.{className}.{className}");
if (classType != null) {

    object? instance = Activator.CreateInstance(classType);
    if (instance != null) {
        classType.GetMethod("RunPart1")?.Invoke(instance, null);
        classType.GetMethod("RunPart2")?.Invoke(instance, null);
    }
}