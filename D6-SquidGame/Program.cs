// https://adventofcode.com/2025/day/6

public static class Program
{
    static void PerformTests()
    {
        string testData = @"123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  ";

        HomeworkOMatic homeworkOMatic = new(testData, true);
        long sumOfExpressions = homeworkOMatic.SolveAndSumAllExpressions();

        Console.WriteLine($"The solved expressions add to {sumOfExpressions}");
    }

    static void PerformPuzzleOne()
    {
        HomeworkOMatic homeworkOMatic = HomeworkOMatic.FromFile(@".\input.txt");
        long sumOfExpressions = homeworkOMatic.SolveAndSumAllExpressions();

        Console.WriteLine($"The solved expressions add to {sumOfExpressions}");
    }
    
    static void PerformPuzzleTwo()
    {
        HomeworkOMatic homeworkOMatic = HomeworkOMatic.FromFile(@".\input.txt", cephalaopdSyntax: true);
        long sumOfExpressions = homeworkOMatic.SolveAndSumAllExpressions();

        Console.WriteLine($"The solved expressions add to {sumOfExpressions}");
    }
    
    public static void Main (string[] args)
    {
        // PerformTests();
        // PerformPuzzleOne();
        PerformPuzzleTwo();
    }
}
