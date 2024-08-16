using System;
using System.Collections.Generic;
using System.IO;

public class CuttingPlaneSolver
{
    private int m; // Number of constraints
    private int n; // Number of variables
    private double[,] tableau;
    private List<double[]> cuttingPlanes = new List<double[]>();

    //This method is used for determining the coefficient values
    public CuttingPlaneSolver(int constraints, int variables)
    {
        m = constraints;
        n = variables;
        tableau = new double[m + 1, n + m + 1];
        InitializeTableau();
    }
    string filepath = "cutting plane.txt";
    private void InitializeTableau()
    {

        using (StreamWriter writer = new StreamWriter(filepath))
        {


            Console.WriteLine("Enter the coefficients for each constraint:");
            writer.WriteLine("Enter the coefficients for each constraint:");

            for (int i = 0; i < m; i++)
            {
                Console.WriteLine($"Constraint {i + 1}:");
                writer.WriteLine($"Constraint {i + 1}:");

                for (int j = 0; j < n; j++)
                {
                    Console.Write($"Coefficient for x{j + 1}: ");
                    writer.WriteLine($"Coefficient for x{j + 1}: ");
                    tableau[i, j] = double.Parse(Console.ReadLine());
                }

                Console.Write($"Right-hand side value for constraint {i + 1}: ");
                writer.WriteLine($"Right-hand side value for constraint {i + 1}: ");
                tableau[i, tableau.GetLength(1) - 1] = double.Parse(Console.ReadLine());
            }

            Console.WriteLine("Enter the coefficients for the objective function:");
            writer.WriteLine("Enter the coefficients for the objective function:");

            for (int j = 0; j < n; j++)
            {
                Console.Write($"Coefficient for x{j + 1}: ");
                writer.WriteLine($"Coefficient for x{j + 1}: ");
                tableau[m, j] = -double.Parse(Console.ReadLine()); // Minimize: negate coefficients
            }

            for (int i = 0; i < m; i++)
            {
                tableau[i, n + i] = 1; // Slack variables
            }
        }
    }

    public void Solve()
    {
        using (StreamWriter writer = new StreamWriter(filepath))
        {


            Console.WriteLine("Initial Tableau:");
            writer.WriteLine("Initial Tableau:");
            DisplayTableau();

            int iteration = 1;
            while (true)
            {
                int pivotCol = ChooseEnteringVariable();
                if (pivotCol == -1)
                {
                    Console.WriteLine("Optimal solution found.");
                    writer.WriteLine("Optimal solution found.");
                    DisplaySolution();
                    return;
                }
                //Will run if the answer is found to be infeasible
                int pivotRow = ChooseLeavingVariable(pivotCol);
                if (pivotRow == -1)
                {
                    Console.WriteLine("Infeasible solution: no valid pivot row.");
                    writer.WriteLine("Infeasible solution: no valid pivot row.");
                    return;
                }

                Pivot(pivotRow, pivotCol);

                Console.WriteLine($"\nTableau after iteration {iteration}:");
                writer.WriteLine($"\nTableau after iteration {iteration}:");
                DisplayTableau();

                // Generate cutting plane and add to tableau
                double[] cuttingPlane = GenerateCuttingPlane();
                cuttingPlanes.Add(cuttingPlane);
                Console.WriteLine($"Cutting Plane {iteration}: {string.Join(", ", cuttingPlane)}");
                writer.WriteLine($"Cutting Plane {iteration}: {string.Join(", ", cuttingPlane)}");

                iteration++;
            }
        }
    }
    //A variable is chosen for the ratio
    private int ChooseEnteringVariable()
    {
        int lastRow = tableau.GetLength(0) - 1;
        for (int j = 0; j < tableau.GetLength(1) - 1; j++)
        {
            if (tableau[lastRow, j] < 0)
                return j;
        }
        return -1;
    }
    //Chooses the intial row for pivoting 
    private int ChooseLeavingVariable(int pivotCol)
    {
    //    using (StreamWriter writer = new StreamWriter(filepath))
        {
            int pivotRow = -1;
            double minRatio = double.PositiveInfinity;

            for (int i = 0; i < tableau.GetLength(0) - 1; i++)
            {
                if (tableau[i, pivotCol] > 0)
                {
                    double ratio = tableau[i, tableau.GetLength(1) - 1] / tableau[i, pivotCol];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            Console.WriteLine($"Chosen pivot row: {pivotRow}");
    //        writer.WriteLine($"Chosen pivot row: {pivotRow}");
            return pivotRow;
        }
    }

    private void Pivot(int pivotRow, int pivotCol)
    {
    //    using (StreamWriter writer = new StreamWriter(filepath))
        {


            int rowCount = tableau.GetLength(0);
            int colCount = tableau.GetLength(1);

            double pivotElement = tableau[pivotRow, pivotCol];
            if (pivotElement == 0)
                throw new InvalidOperationException("Pivot element cannot be zero.");

            // Divide the pivot row by the pivot element
            for (int j = 0; j < colCount; j++)
            {
                tableau[pivotRow, j] /= pivotElement;
            }

            // Subtract the pivot row from other rows
            for (int i = 0; i < rowCount; i++)
            {
                if (i != pivotRow)
                {
                    double factor = tableau[i, pivotCol];
                    for (int j = 0; j < colCount; j++)
                    {
                        tableau[i, j] -= factor * tableau[pivotRow, j];
                    }
                }
            }

            Console.WriteLine($"Pivoted at row {pivotRow}, column {pivotCol}");
      //      writer.WriteLine($"Pivoted at row {pivotRow}, column {pivotCol}");
        }
    }

    private double[] GenerateCuttingPlane()
    {
        // Example cutting plane generation; customize as needed
        double[] cuttingPlane = new double[n + m];
        for (int i = 0; i < n + m; i++)
        {
            cuttingPlane[i] = 0; // Default values
        }

        // Set values for the cutting plane
        // This example sets a simple plane; you might need to adjust based on your requirements
        cuttingPlane[n] = -1; // Example coefficient
        cuttingPlane[n + 1] = 1; // Example coefficient

        return cuttingPlane;
    }
    //Defines how the values are displayed
    private void DisplayTableau()
    {
     //   using (StreamWriter writer = new StreamWriter(filepath))
     //   {


            for (int i = 0; i < tableau.GetLength(0); i++)
            {
                for (int j = 0; j < tableau.GetLength(1); j++)
                {
                    Console.Write($"{tableau[i, j]:F2}\t");
        //            writer.Write($"{tableau[i, j]:F2}\t");
                }
                Console.WriteLine();
        //        writer.WriteLine();
            }
       // }
    }
    //Responsible for displaying the solutions found during the cutting plane 
    private void DisplaySolution()
    {
    //    using (StreamWriter writer = new StreamWriter(filepath))
        {
            Console.WriteLine("\nSolution:");
    //        writer.WriteLine("\nSolution:");
            double[] solution = new double[n];

            for (int i = 0; i < m; i++)
            {
                if (tableau[i, n + i] == 1)
                {
                    solution[i] = tableau[i, tableau.GetLength(1) - 1];
                }
            }

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"x{i + 1} = {solution[i]:F2}");
    //            writer.WriteLine($"x{i + 1} = {solution[i]:F2}");
            }

            Console.WriteLine($"Objective function value: {tableau[m, tableau.GetLength(1) - 1]:F2}");
    //        writer.WriteLine($"Objective function value: {tableau[m, tableau.GetLength(1) - 1]:F2}");
        }
    }
}