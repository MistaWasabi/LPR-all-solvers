//programmers
//Willem Paton
//Joshua Meyer
//Aiden Bekley

//Nhlanhla Mbatha was a non participant




using System;
using System.IO;

class Program
{
    
    static void Main()
    {

        string filepath = "main.txt";
        using (StreamWriter writer = new StreamWriter(filepath))
        {


            Console.WriteLine("Choose an algorithm:");
            writer.WriteLine("Choose an algorithm:");
            Console.WriteLine("1. Simplex Solver");
            writer.WriteLine("1. Simplex Solver");
            Console.WriteLine("2. Knapsack Solver");
            writer.WriteLine("2. Knapsack Solver");
            Console.WriteLine("3. Cutting Plane Solver");
            writer.WriteLine("3. Cutting Plane Solver");
            Console.WriteLine("4. Branch and Bound Solver");
            writer.WriteLine("4. Branch and Bound Solver");
            Console.WriteLine("5. Non-Linear Solver");
            writer.WriteLine("5. Non-Linear Solver");
            Console.Write("Enter choice (1, 2, 3, 4 or 5 ): ");
            writer.Write("Enter choice (1, 2, 3, 4 or 5 ): ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    RunSimplexSolver();
                    break;

                case 2:
                    RunKnapsackSolver();
                    break;

                case 3:
                    RunCuttingPlaneSolver();
                    break;

                case 4:
                    RunBranchAndBoundSolver();
                    break;
                case 5:
                    RunNonLinearSolver();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            // Pause the console so the user can view the results
            Console.WriteLine("Press Enter to exit...");
            writer.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
    //Responsible for running the Simplex through the main method
    private static void RunSimplexSolver()
    {

        
            Console.Write("Enter the number of constraints: ");
           // writer.Write("Enter the number of constraints: ");
            int constraints = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of variables: ");
            //writer.Write("Enter the number of variables: ");
            int variables = int.Parse(Console.ReadLine());

            SimplexSolver simplexSolver = new SimplexSolver(constraints, variables);
            simplexSolver.Solve();
        
    }
    //Responsible for running the Knapsack through the main method
    private static void RunKnapsackSolver()
    {

       // string filepath = "main.txt";
       // using (StreamWriter writer = new StreamWriter(filepath))
       // {


            Console.Write("Enter the number of items: ");
         //   writer.Write("Enter the number of items: ");
            int itemCount = int.Parse(Console.ReadLine());

            int[] weights = new int[itemCount];
            int[] values = new int[itemCount];

            Console.WriteLine("Enter the weights of the items:");
           // writer.WriteLine("Enter the weights of the items:");
            for (int i = 0; i < itemCount; i++)
            {
                Console.Write($"Weight of item {i + 1}: ");
        //        writer.Write($"Weight of item {i + 1}: ");
                weights[i] = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Enter the values of the items:");
        //    writer.WriteLine("Enter the values of the items:");
            for (int i = 0; i < itemCount; i++)
            {
                Console.Write($"Value of item {i + 1}: ");
         //       writer.Write($"Value of item {i + 1}: ");
                values[i] = int.Parse(Console.ReadLine());
            }

            Console.Write("Enter the capacity of the knapsack: ");
         //   writer.Write("Enter the capacity of the knapsack: ");
            int capacity = int.Parse(Console.ReadLine());

            KnapsackSolver knapsackSolver = new KnapsackSolver(weights, values, capacity);
            knapsackSolver.Solve();
       // }
    }
    //Responsible for running the CuttingPlane through the main method
    private static void RunCuttingPlaneSolver()
    {
 //       string filepath = "main.txt";
   //     using (StreamWriter writer = new StreamWriter(filepath))
     //   {
            Console.Write("Enter the number of constraints: ");
      //      writer.Write("Enter the number of constraints: ");
            int constraints = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of variables: ");
       //     writer.Write("Enter the number of variables: ");
            int variables = int.Parse(Console.ReadLine());

            CuttingPlaneSolver cuttingPlaneSolver = new CuttingPlaneSolver(constraints, variables);
            cuttingPlaneSolver.Solve();
       // }
    }
    //Responsible for running the BranchAndBound through the main method
    private static void RunBranchAndBoundSolver()
    {
     //   string filepath = "main.txt";
       // using (StreamWriter writer = new StreamWriter(filepath))
      //  {
            Console.Write("Enter the number of items: ");
      //      writer.Write("Enter the number of items: ");
            int itemCount = int.Parse(Console.ReadLine());

            int[] weights = new int[itemCount];
            int[] values = new int[itemCount];

            Console.WriteLine("Enter the weights of the items:");
      //      writer.WriteLine("Enter the weights of the items:");
            for (int i = 0; i < itemCount; i++)
            {
                Console.Write($"Weight of item {i + 1}: ");
      //          writer.Write($"Weight of item {i + 1}: ");
                weights[i] = int.Parse(Console.ReadLine());
            }

            Console.WriteLine("Enter the values of the items:");
      //      writer.WriteLine("Enter the values of the items:");
            for (int i = 0; i < itemCount; i++)
            {
                Console.Write($"Value of item {i + 1}: ");
      //          writer.Write($"Value of item {i + 1}: ");
                values[i] = int.Parse(Console.ReadLine());
            }

            Console.Write("Enter the capacity of the knapsack: ");
      //      writer.Write("Enter the capacity of the knapsack: ");
            int capacity = int.Parse(Console.ReadLine());

            BranchAndBoundSolver branchAndBoundSolver = new BranchAndBoundSolver(weights, values, capacity);
            branchAndBoundSolver.Solve();
      //  }
    }
    //Responsible for running the Non Linear through the main method
    private static void RunNonLinearSolver()
    {
        //string filepath = "main.txt";
        //using (StreamWriter writer = new StreamWriter(filepath))
        //{
            Console.Write("Enter the learning rate: ");
        //    writer.Write("Enter the learning rate: ");
            double learningRate = double.Parse(Console.ReadLine());

            Console.Write("Enter the tolerance: ");
        //    writer.Write("Enter the tolerance: ");
            double tolerance = double.Parse(Console.ReadLine());

            Console.Write("Enter the maximum number of iterations: ");
        //    writer.Write("Enter the maximum number of iterations: ");
            int maxIterations = int.Parse(Console.ReadLine());

            NonLinearSolver nonLinearSolver = new NonLinearSolver(learningRate, tolerance, maxIterations);

            // Function: f(x) = x^2
            Func<double, double> function = x => x * x;
            // Gradient (derivative): f'(x) = 2*x
            Func<double, double> gradient = x => 2 * x;

            Console.Write("Enter the initial guess: ");
         //   writer.Write("Enter the initial guess: ");
            double initialGuess = double.Parse(Console.ReadLine());

            nonLinearSolver.Solve(function, gradient, initialGuess);
       // }
    }
}


