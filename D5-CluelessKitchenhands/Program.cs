// https://adventofcode.com/2025/day/5

public static class Program
{
    public static void PerformTests()
    {
        string testData = @"3-5
10-14
16-20
12-18

1
5
8
11
17
32";

        Pantry pantry = new Pantry(testData);
        IEnumerable<long> freshIngredientIds = pantry.GetFreshIngredientIds();

        Console.WriteLine($"There are {freshIngredientIds.Count()} fresh ingredients in the pantry");
    }

    public static void PerformPuzzleOne()
    {

        Pantry pantry = Pantry.FromFile(@".\input.txt");
        IEnumerable<long> freshIngredientIds = pantry.GetFreshIngredientIds();

        Console.WriteLine($"There are {freshIngredientIds.Count()} fresh ingredients in the pantry");
    }
    
    public static void PerformPuzzleTwo()
    {
        
    }

    public static void Main (string[] args)
    {
        PerformPuzzleOne();
    }
}