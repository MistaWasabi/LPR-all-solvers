using System;
using System.Collections.Generic;
using System.IO;

public class KnapsackSolver
{
    private int[] weights;
    private int[] values;
    private int capacity;
    private int itemCount;
    //Specifies the file where the knapsack is read from
    string filepath = "Knapsack.txt";
    public KnapsackSolver(int[] weights, int[] values, int capacity)
    {
        this.weights = weights;
        this.values = values;
        this.capacity = capacity;
        this.itemCount = weights.Length;
    }
    //Responsible for the main knapsack calculations
    public void Solve()
    {   //Used to write to file
        using (StreamWriter writer = new StreamWriter(filepath))
        {   //Displays the first knapsack table after calculating the values
            int[,] dp = new int[itemCount + 1, capacity + 1];
            Console.WriteLine("Initial DP Table:");
            writer.WriteLine("Initial DP Table:");
            DisplayDPTable(dp);

            for (int i = 1; i <= itemCount; i++)
            {
                for (int w = 1; w <= capacity; w++)
                {
                    if (weights[i - 1] <= w)
                    {
                        dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - weights[i - 1]] + values[i - 1]);
                    }
                    else
                    {
                        dp[i, w] = dp[i - 1, w];
                    }
                }
                Console.WriteLine($"\nDP Table after processing item {i}:");
                writer.WriteLine($"\nDP Table after processing item {i}:");
                DisplayDPTable(dp);
            }
            //The Best ZValue
            Console.WriteLine($"\nMaximum value that can be obtained: {dp[itemCount, capacity]}");
            writer.WriteLine($"\nMaximum value that can be obtained: {dp[itemCount, capacity]}");
            PrintSelectedItems(dp);
        }
    }
    //Writes the chosen items to the file
    private void PrintSelectedItems(int[,] dp)
    {
     //   using (StreamWriter writer = new StreamWriter(filepath))
        {
            int w = capacity;
            List<int> selectedItems = new List<int>();
            Console.WriteLine("Items selected:");
     //       writer.WriteLine("Items selected:");

            for (int i = itemCount; i > 0 && w > 0; i--)
            {
                if (dp[i, w] != dp[i - 1, w])
                {
                    selectedItems.Add(i - 1);
                    Console.WriteLine($"Item {i}: Weight = {weights[i - 1]}, Value = {values[i - 1]}");
      //              writer.WriteLine($"Item {i}: Weight = {weights[i - 1]}, Value = {values[i - 1]}");
                    w -= weights[i - 1];
                }
            }

            Console.WriteLine("Selected Items in reverse order:");
       //     writer.WriteLine("Selected Items in reverse order:");
            selectedItems.Reverse();
            foreach (var item in selectedItems)
            {
                Console.WriteLine($"Item {item + 1}: Weight = {weights[item]}, Value = {values[item]}");
        //        writer.WriteLine($"Item {item + 1}: Weight = {weights[item]}, Value = {values[item]}");
            }
        }
    }
    //Responsible for how the knapsack is displayed in a table
    private void DisplayDPTable(int[,] dp)
    {
    //    using (StreamWriter writer = new StreamWriter(filepath))
        {
            Console.Write("  ");
     //       writer.Write("  ");
            for (int w = 0; w <= capacity; w++)
            {
                Console.Write($"{w,4}");
     //           writer.Write($"{w,4}");
            }
            Console.WriteLine();
    //        writer.WriteLine();

            for (int i = 0; i <= itemCount; i++)
            {
                Console.Write($"{i,2}:");
      //          writer.Write($"{i,2}:");
                for (int w = 0; w <= capacity; w++)
                {
                    Console.Write($"{dp[i, w],4}");
      //              writer.Write($"{dp[i, w],4}");
                }
                Console.WriteLine();
      //          writer.WriteLine();
            }
        }
    }
}
