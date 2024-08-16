using System;
using System.IO;

public class SimplexSolver
{
    private double[,] tableau;
    private int[] basicVariables;
    private int[] nonBasicVariables;
    private int m; // Number of constraints
    private int n; // Number of variables

    string filepath = "Simplex.txt";
    public SimplexSolver(int constraints, int variables)
    {
        using (StreamWriter writer = new StreamWriter(filepath))
        {


            m = constraints;
            n = variables;
            tableau = new double[m + 1, n + m + 1];

            // Initialize basic and non-basic variable indices
            basicVariables = new int[m];
            for (int i = 0; i < m; i++)
            {
                basicVariables[i] = n + i;
            }

            nonBasicVariables = new int[n];
            for (int i = 0; i < n; i++)
            {
                nonBasicVariables[i] = i;
            }

            FillTableau();

            Console.WriteLine($"Initial Tableau Dimensions: {tableau.GetLength(0)}x{tableau.GetLength(1)}");
            writer.WriteLine($"Initial Tableau Dimensions: {tableau.GetLength(0)}x{tableau.GetLength(1)}");
        }
    }
    //Responsible for collecting the data for filling a table
    private void FillTableau()
    {
      //  using (StreamWriter writer = new StreamWriter(filepath))
       // {
            Console.WriteLine("Enter the coefficients for each constraint:");
        //    writer.WriteLine("Enter the coefficients for each constraint:");

            for (int i = 0; i < m; i++)
            {
                Console.WriteLine($"Constraint {i + 1}:");
         //       writer.WriteLine($"Constraint {i + 1}:");

                for (int j = 0; j < n; j++)
                {
                    Console.Write($"Coefficient for x{j + 1}: ");
          //          writer.Write($"Coefficient for x{j + 1}: ");
                    tableau[i, j] = double.Parse(Console.ReadLine());
                }

                Console.Write($"Right-hand side value for constraint {i + 1}: ");
          //      writer.Write($"Right-hand side value for constraint {i + 1}: ");
                tableau[i, tableau.GetLength(1) - 1] = double.Parse(Console.ReadLine());
            }

            Console.WriteLine("Enter the coefficients for the objective function:");
          //  writer.WriteLine("Enter the coefficients for the objective function:");

            for (int j = 0; j < n; j++)
            {
                Console.Write($"Coefficient for x{j + 1}: ");
         //       writer.Write($"Coefficient for x{j + 1}: ");
                tableau[m, j] = -double.Parse(Console.ReadLine()); // Minimize: negate coefficients
            }

            for (int i = 0; i < m; i++)
            {
                tableau[i, n + i] = 1; // Slack variables
            }
       // }
    }
    //Responsible for gathering all declared methods to solve the knapsack
    public void Solve()
    {
        using (StreamWriter writer = new StreamWriter(filepath))
        {
            Console.WriteLine("\nInitial Tableau:");
            writer.WriteLine("\nInitial Tableau:");
            DisplayTableau(tableau);

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

                int pivotRow = ChooseLeavingVariable(pivotCol);
                if (pivotRow == -1)
                {
                    throw new InvalidOperationException("Unbounded solution.");
                }

                Pivot(pivotRow, pivotCol);

                Console.WriteLine("\nTableau after pivoting:");
                writer.WriteLine("\nTableau after pivoting:");
                DisplayTableau(tableau);
            }
        }
    }
    //Chooses the row to pivot
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
    //Chooses the column to pivot on based on the ratio calculated
    private int ChooseLeavingVariable(int pivotCol)
    {
      //  using (StreamWriter writer = new StreamWriter(filepath))
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
        //    writer.WriteLine($"Chosen pivot row: {pivotRow}");
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

            // Update basic and non-basic variables
            int temp = basicVariables[pivotRow];
            basicVariables[pivotRow] = nonBasicVariables[pivotCol];
            nonBasicVariables[pivotCol] = temp;

            Console.WriteLine($"Pivoted at row {pivotRow}, column {pivotCol}");
      //      writer.WriteLine($"Pivoted at row {pivotRow}, column {pivotCol}");
        }
    }
    //Responsible for the table display of the values
    private void DisplayTableau(double[,] tableau)
    {
      //  using (StreamWriter writer = new StreamWriter(filepath))
        {
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
        }
    }
    //Displays the solutions objective value
    private void DisplaySolution()
    {
    //    using (StreamWriter writer = new StreamWriter(filepath))
        {


            Console.WriteLine("\nSolution:");
    //        writer.WriteLine("\nSolution:");
            double[] solution = new double[n];

            for (int i = 0; i < m; i++)
            {
                if (basicVariables[i] < n)
                {
                    solution[basicVariables[i]] = tableau[i, tableau.GetLength(1) - 1];
                }
            }

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"x{i + 1} = {solution[i]:F2}");
    //            writer.WriteLine($"x{i + 1} = {solution[i]:F2}");
            }

            Console.WriteLine($"Objective function value: {tableau[m, tableau.GetLength(1) - 1]:F2}");
     //       writer.WriteLine($"Objective function value: {tableau[m, tableau.GetLength(1) - 1]:F2}");
        }
    }
}
