using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class BranchAndBoundSolver
{
    private int[] weights;
    private int[] values;
    private int capacity;
    private int itemCount;

    private List<int> bestSolutionItems = new List<int>(); // To track the best solution

    public BranchAndBoundSolver(int[] weights, int[] values, int capacity)
    {
        this.weights = weights;
        this.values = values;
        this.capacity = capacity;
        this.itemCount = weights.Length;
    }

    public void Solve()
    {
        Node root = new Node();
        root.Level = -1;
        root.Bound = CalculateBound(root);
        root.Weight = 0;
        root.Value = 0;
        root.Items = new List<int>(); // To track selected items

        Queue<Node> queue = new Queue<Node>();
        queue.Enqueue(root);

        Node bestNode = root;
        string filepath = "branch and bound.txt";
        using (StreamWriter writer = new StreamWriter(filepath))
        {
            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();

                // Display the node details
                Console.WriteLine($"Processing Node: Level = {node.Level}, Weight = {node.Weight}, Value = {node.Value}, Bound = {node.Bound}");
                writer.WriteLine($"Processing Node: Level = {node.Level}, Weight = {node.Weight}, Value = {node.Value}, Bound = {node.Bound}");

                Console.WriteLine($"Items in Node: {string.Join(", ", node.Items.Select(i => i + 1))}");
                writer.WriteLine($"Items in Node: {string.Join(", ", node.Items.Select(i => i + 1))}");

                if (node.Level == itemCount - 1)
                    continue;

                // Node including the current item
                Node nextNode = new Node();
                nextNode.Level = node.Level + 1;
                nextNode.Weight = node.Weight + weights[nextNode.Level];
                nextNode.Value = node.Value + values[nextNode.Level];
                nextNode.Items = new List<int>(node.Items) { nextNode.Level };

                if (nextNode.Weight <= capacity)
                {
                    Console.WriteLine($"Including Item {nextNode.Level + 1}: Weight = {nextNode.Weight}, Value = {nextNode.Value}");
                    writer.WriteLine($"Including Item {nextNode.Level + 1}: Weight = {nextNode.Weight}, Value = {nextNode.Value}");
                    if (nextNode.Value > bestNode.Value)
                    {
                        bestNode = nextNode;
                        bestSolutionItems = new List<int>(bestNode.Items);
                    }
                }

                nextNode.Bound = CalculateBound(nextNode);

                Console.WriteLine($"Bound for including item {nextNode.Level + 1}: {nextNode.Bound}");
                writer.WriteLine($"Bound for including item {nextNode.Level + 1}: {nextNode.Bound}");

                if (nextNode.Bound > bestNode.Value)
                {
                    queue.Enqueue(nextNode);
                }

                // Node excluding the current item
                Node nextNodeWithout = new Node();
                nextNodeWithout.Level = node.Level + 1;
                nextNodeWithout.Weight = node.Weight;
                nextNodeWithout.Value = node.Value;
                nextNodeWithout.Bound = CalculateBound(nextNodeWithout);
                nextNodeWithout.Items = new List<int>(node.Items);

                Console.WriteLine($"Excluding Item {nextNodeWithout.Level + 1}: Weight = {nextNodeWithout.Weight}, Value = {nextNodeWithout.Value}");
                writer.WriteLine($"Excluding Item {nextNodeWithout.Level + 1}: Weight = {nextNodeWithout.Weight}, Value = {nextNodeWithout.Value}");

                Console.WriteLine($"Bound for excluding item {nextNodeWithout.Level + 1}: {nextNodeWithout.Bound}");
                writer.WriteLine($"Bound for excluding item {nextNodeWithout.Level + 1}: {nextNodeWithout.Bound}");

                if (nextNodeWithout.Bound > bestNode.Value)
                {
                    queue.Enqueue(nextNodeWithout);
                }
            }
            //Best Z Value
            Console.WriteLine($"\nMaximum value that can be obtained: {bestNode.Value}");
            writer.WriteLine($"\nMaximum value that can be obtained: {bestNode.Value}");

            Console.WriteLine("Items selected:");
            writer.WriteLine("Items selected:");

            foreach (var item in bestSolutionItems)
            {
                Console.WriteLine($"Item {item + 1}: Weight = {weights[item]}, Value = {values[item]}");
                writer.WriteLine($"Item {item + 1}: Weight = {weights[item]}, Value = {values[item]}");
            }
        }
    }
    //The algorithim for calculating the Branch and Bound
    private double CalculateBound(Node node)
    {
        int j;
        double bound;

        if (node.Weight >= capacity)
            return 0;

        bound = node.Value;
        j = node.Level + 1;
        double totalWeight = node.Weight;

        while (j < itemCount && totalWeight + weights[j] <= capacity)
        {
            totalWeight += weights[j];
            bound += values[j];
            j++;
        }

        if (j < itemCount)
            bound += (capacity - totalWeight) * values[j] / (double)weights[j];

        return bound;
    }
    //Defining the constraints and objective function
    private class Node
    {
        public int Level { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
        public double Bound { get; set; }
        public List<int> Items { get; set; } // To track the items in the solution
    }
}
