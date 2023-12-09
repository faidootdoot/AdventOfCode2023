using AdventOfCode.Exercises.Day07;
using FluentAssertions.Equivalency;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Exercises.Day08;
public class Day08 {
    private readonly string instructions = "LRRRLRRRLRRLRLRRLRLRRLRRLRLLRRRLRLRLRRRLRRRLRLRLRLLRRLLRRLRRRLLRLRRRLRLRLRRRLLRLRRLRRRLRLRRRLLRLRRLRRRLRRLRRLRLRRLRRRLRLRRRLRRLLRRLRRLRLRRRLRRLRRRLRRRLRLRRLRLRRRLRLRRLRRLRRRLRRRLRRRLLRRLRRRLRLRLRLRRRLRLRLRRLRRRLRRRLRRLRRLLRLRRLLRLRRLRRLLRLLRRRLLRRLLRRLRRLRLRLRRRLLRRLRRRR";

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day08\\Data.txt");
    private readonly Dictionary<string, LeftRightNode> Nodes = new();
    private readonly string StartNodeName = default!;
    private readonly string destinationNodeName = default!;

    public Day08() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        foreach (var line in lines) {

            var dataArray = line.Split("=");
            var nodeName = dataArray[0].Trim();
            var leftRightNode = dataArray[1].Replace("(", "").Replace(")", "").Split(",");
            this.Nodes[nodeName] = new LeftRightNode(leftRightNode[0].Trim(), leftRightNode[1].Trim());
            this.StartNodeName = "AAA";//this.Nodes.First().Key;
            this.destinationNodeName = "ZZZ";//this.Nodes.Last().Key;
        }
    }

    public void RunPart1() {

        var node = this.Nodes[this.StartNodeName];
        string nodeName = string.Empty;
        int steps = 0;
        bool nodeFound = false;

        while (!nodeFound) {
            foreach (var direction in this.instructions) {

                steps++;

                if (direction == 'L') {
                    nodeName = node.LeftNodeName;
                }
                else if (direction == 'R') {

                    nodeName = node.RightNodeName;
                }

                Console.WriteLine($"{steps.ToString().PadRight(5)}{direction.ToString().PadRight(3)}{nodeName}");
                node = this.Nodes[nodeName];

                if (nodeName == this.destinationNodeName) {
                    nodeFound = true;
                    break;
                }
            }
        }

        Console.WriteLine($"PART 1");
        Console.WriteLine($"Steps is {steps}");
    }

    public void RunPart2_1() {

        // nodes that end with a
        List<string> nodeList = this.Nodes.Where(node => node.Key.EndsWith("A")).Select(node => node.Key).ToList();

        bool destinationArrived = false;
        int steps = 0;

        while (!destinationArrived) {

            foreach (var direction in this.instructions) {

                steps++;
                Task[] taskArray = new Task[nodeList.Count - 1];
                for (int i = 0; i < taskArray.Length; i++) {
                    taskArray[i] = Task.Factory.StartNew(() => {
                        var node = this.Nodes[nodeList[i]];
                        string nodeName = string.Empty;

                        if (direction == 'L') {
                            nodeName = node.LeftNodeName;
                        }
                        else if (direction == 'R') {

                            nodeName = node.RightNodeName;
                        }
                        Console.WriteLine($"{steps.ToString().PadRight(5)}{direction.ToString().PadRight(3)}{nodeName}");
                        nodeList[i] = nodeName;
                    });
                }
                Task.WaitAll(taskArray);

                // Do all nodes end in Z
                if (nodeList.Count(nodeName => nodeName.EndsWith("Z")) == nodeList.Count) {
                    destinationArrived = true;
                    break;
                }
            }
        }

        // how to run method simultaneously
        Console.WriteLine($"PART 2");
        Console.WriteLine($"Steps is {steps}");
    }


    public void RunPart2() {

        // nodes that end with a
        List<string> nodeList = this.Nodes.Where(node => node.Key.EndsWith("A")).Select(node => node.Key).ToList();

        bool destinationArrived = false;
        int steps = 0;

        while (!destinationArrived) {

            foreach (var direction in this.instructions) {

                steps++;

                for (int i = 0; i < nodeList.Count; i++) {

                    if (nodeList[i].EndsWith("Z")) {
                        continue;
                    }

                    var node = this.Nodes[nodeList[i]];
                    string nodeName = string.Empty;

                    if (direction == 'L') {
                        nodeName = node.LeftNodeName;
                    }
                    else if (direction == 'R') {

                        nodeName = node.RightNodeName;
                    }

                    Console.WriteLine($"{steps.ToString().PadRight(5)}{direction.ToString().PadRight(3)}{nodeName}");
                    nodeList[i] = nodeName;
                }

                // Do all nodes end in Z
                if (nodeList.Count(nodeName => nodeName.EndsWith("Z")) == nodeList.Count) {
                    destinationArrived = true;
                    break;
                }
                //else {
                //    if (nodeList.Count(nodeName => nodeName.EndsWith("Z")) > 1) {
                //        Console.WriteLine("Check:");
                //        nodeList.ForEach(nodeName => Console.WriteLine(nodeName));
                //    }
                //}
                ////else {
                ////    nodeList.ForEach(nodeName => Console.WriteLine(nodeName));
                ////}
            }
        }

        // how to run method simultaneously
        Console.WriteLine($"PART 2");


        Console.WriteLine("Check:");
        nodeList.ForEach(nodeName => Console.WriteLine(nodeName));


        Console.WriteLine($"Steps is {steps}");
    }

}
