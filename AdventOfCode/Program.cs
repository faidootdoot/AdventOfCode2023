// See https://aka.ms/new-console-template for more information
using AdventOfCode.Exercises.Day01;
using AdventOfCode.Exercises.Day02;
using AdventOfCode.Exercises.Day03;
using AdventOfCode.Exercises.Day04;

int day = 4;

string className = $"Day{day.ToString().PadLeft(2, '0')}";
Console.WriteLine(className);

//Type t = Type.GetType(className);
//var obj = Activator.CreateInstance(t);
//obj.Run
switch (day) {
	case 1:
        Day01 Day01 = new();
        Day01.RunPart1();
        Day01.RunPart2();
        break;
    case 2:
        Day02 Day02 = new();
        Day02.RunPart1();
        Day02.RunPart2();
        break;
    case 3:
        Day03 Day03 = new();
        Day03.RunPart1();
        Day03.RunPart2();
        break;
    case 4:
        Day04 Day04 = new();
        Day04.RunPart1();
        Day04.RunPart2();
        break;
}


