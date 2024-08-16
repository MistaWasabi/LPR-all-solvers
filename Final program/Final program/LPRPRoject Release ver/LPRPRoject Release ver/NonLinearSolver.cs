using System;
using System.IO;

public class NonLinearSolver
{
    private double learningRate;
    private double tolerance;
    private int maxIterations;

    public NonLinearSolver(double learningRate, double tolerance, int maxIterations)
    {
        this.learningRate = learningRate;
        this.tolerance = tolerance;
        this.maxIterations = maxIterations;
    }

    public void Solve(Func<double, double> function, Func<double, double> gradient, double initialGuess)
    {
        string filepath = "NonLinear.txt";
        using (StreamWriter writer = new StreamWriter(filepath))
        {
            double x = initialGuess;
            double fx = function(x);
            int iteration = 0;

            Console.WriteLine($"Starting gradient descent with initial guess: x = {x}");
            writer.WriteLine($"Starting gradient descent with initial guess: x = {x}");
            Console.WriteLine($"Function: f(x) = {fx}");
            writer.WriteLine($"Function: f(x) = {fx}");

            while (iteration < maxIterations)
            {
                double grad = gradient(x);
                double newX = x - learningRate * grad;

                if (Math.Abs(newX - x) < tolerance)
                {
                    Console.WriteLine($"Converged to x = {newX} with function value f(x) = {function(newX)} after {iteration} iterations.");
                    writer.WriteLine($"Converged to x = {newX} with function value f(x) = {function(newX)} after {iteration} iterations.");

                    return;
                }

                x = newX;
                fx = function(x);
                Console.WriteLine($"Iteration {iteration}: x = {x}, f(x) = {fx}");
                writer.WriteLine($"Iteration {iteration}: x = {x}, f(x) = {fx}");

                iteration++;
            }

            Console.WriteLine($"Max iterations reached. Approximate solution: x = {x}, f(x) = {function(x)}");
            writer.WriteLine($"Max iterations reached. Approximate solution: x = {x}, f(x) = {function(x)}");
        }
    }
}
