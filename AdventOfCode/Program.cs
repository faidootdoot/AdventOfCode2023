// See https://aka.ms/new-console-template for more information
using AdventOfCode.Exercises.Day01;
using AdventOfCode.Exercises.Day02;

int day = 2;

string className = $"Day{day.ToString().PadLeft(2, '0')}";
Console.WriteLine(className);

//Type t = Type.GetType(className);
//var obj = Activator.CreateInstance(t);
//obj.Run
Day02 Day02 = new();
Day02.RunPart1();
Day02.RunPart2();


Day01 Day01 = new();
Day01.RunPart1();
Day01.RunPart2();
