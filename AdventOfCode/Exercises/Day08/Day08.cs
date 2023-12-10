namespace AdventOfCode.Exercises.Day08;
public class Day08 {
    private readonly string instructions = "LRRRLRRRLRRLRLRRLRLRRLRRLRLLRRRLRLRLRRRLRRRLRLRLRLLRRLLRRLRRRLLRLRRRLRLRLRRRLLRLRRLRRRLRLRRRLLRLRRLRRRLRRLRRLRLRRLRRRLRLRRRLRRLLRRLRRLRLRRRLRRLRRRLRRRLRLRRLRLRRRLRLRRLRRLRRRLRRRLRRRLLRRLRRRLRLRLRLRRRLRLRLRRLRRRLRRRLRRLRRLLRLRRLLRLRRLRRLLRLLRRRLLRRLLRRLRRLRLRLRRRLLRRLRRRR";

    private readonly Uri filePath = new(new Uri(AppDomain.CurrentDomain.BaseDirectory), "Exercises\\Day08\\Data.txt");
    private readonly Dictionary<string, LeftRightNode> Nodes = new();
    private readonly string StartNodeName = "AAA";
    private readonly string destinationNodeName = "ZZZ";
    private readonly List<string> nodeNameList = default!;

    public Day08() {

        var lines = File.ReadLines(this.filePath.AbsolutePath).ToList();
        foreach (var line in lines) {

            var dataArray = line.Split("=");
            var nodeName = dataArray[0].Trim();
            var leftRightNode = dataArray[1].Replace("(", "").Replace(")", "").Split(",");
            this.Nodes[nodeName] = new LeftRightNode(leftRightNode[0].Trim(), leftRightNode[1].Trim());
            this.nodeNameList = this.Nodes.Where(node => node.Key.EndsWith("A")).Select(node => node.Key).ToList();
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

    public void RunPart2() {

        // nodes that end with a

        bool destinationArrived = false;
        long steps = 0;

        while (!destinationArrived) {

            foreach (var direction in this.instructions) {

                steps++;

                //this.nodeNameList.AsParallel().ForAll(nodeName => {

                //    var index = this.GetListPosition(nodeName);
                //    var node = this.Nodes[nodeName];
                //    this.nodeNameList[index] = (direction == 'L') ? node.LeftNodeName : node.RightNodeName;
                //});


                Console.WriteLine("Check:");
                this.nodeNameList.ForEach(nodeName => Console.WriteLine(nodeName));

                Parallel.ForEach(this.nodeNameList, nodeName => {

                    var index = this.GetListPosition(nodeName);
                    var node = this.Nodes[nodeName];

                    this.nodeNameList[index] = (direction == 'L') ? node.LeftNodeName : node.RightNodeName;
                });

                // Do all nodes end in Z
                if (this.nodeNameList.Count(nodeName => nodeName.EndsWith("Z")) == this.nodeNameList.Count) {
                    destinationArrived = true;
                    break;
                }
            }
        }

        // how to run method simultaneously
        Console.WriteLine($"PART 2");
        Console.WriteLine($"Steps is {steps}");
    }


    private int GetListPosition(string nodeName) {

        int index = 0;
        for (int i = 0; i < this.nodeNameList.Count; i++) {
            if (this.nodeNameList[i] == nodeName) {
                index = i;
            }
        }
        return index;
    }

    private void MoveLeftRight(int index, char direction) {

        var node = this.Nodes[this.nodeNameList[index]];
        string nodeName = string.Empty;

        if (direction == 'L') {
            nodeName = node.LeftNodeName;
        }
        else if (direction == 'R') {

            nodeName = node.RightNodeName;
        }
        this.nodeNameList[index] = nodeName;
    }


    public void RunPart2_1() {

        // nodes that end with a
        List<string> nodeList = this.Nodes.Where(node => node.Key.EndsWith("A")).Select(node => node.Key).ToList();

        bool destinationArrived = false;
        int steps = 0;

        while (!destinationArrived) {

            foreach (var direction in this.instructions) {

                steps++;

                for (int i = 0; i < nodeList.Count; i++) {

                    var node = this.Nodes[nodeList[i]];
                    string nodeName = string.Empty;

                    if (direction == 'L') {
                        nodeName = node.LeftNodeName;
                    }
                    else if (direction == 'R') {

                        nodeName = node.RightNodeName;
                    }

                    //Console.WriteLine($"{steps.ToString().PadRight(5)}{direction.ToString().PadRight(3)}{nodeName}");
                    nodeList[i] = nodeName;
                }

                // Do all nodes end in Z
                if (nodeList.Count(nodeName => nodeName.EndsWith("Z")) == nodeList.Count) {
                    destinationArrived = true;
                    break;
                }
                else {
                    if (nodeList.Count(nodeName => nodeName.EndsWith("Z")) > 1) {
                        Console.WriteLine("Check:");
                        nodeList.ForEach(nodeName => Console.WriteLine(nodeName));
                    }
                }
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
