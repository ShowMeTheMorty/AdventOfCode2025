// https://adventofcode.com/2025/day/7

public static class Program
{
    static void PerformTests()
    {
        string testData = @".......S.......
...............
.......^.......
...............
......^.^......
...............
.....^.^.^.....
...............
....^.^...^....
...............
...^.^...^.^...
...............
..^...^.....^..
...............
.^.^.^.^.^...^.
...............";

        TachyonManifold manifold = new(testData);
        TachyonManifold.TachyonSplittingResult splittingResult = manifold.RunTachyonSplittingManifold();

        Console.WriteLine($"The tachyon manifold split a beam {splittingResult.tachyonPathCount} times");
    }

    static void PerformPuzzleOne()
    {
        TachyonManifold manifold = TachyonManifold.FromFile(@".\input.txt");
        TachyonManifold.TachyonSplittingResult splittingResult = manifold.RunTachyonSplittingManifold();

        Console.WriteLine($"The tachyon manifold split a beam {splittingResult.splitterActivationCount} times");
    }
    
    static void PerformPuzzleTwo()
    {
        TachyonManifold manifold = TachyonManifold.FromFile(@".\input.txt");
        TachyonManifold.TachyonSplittingResult splittingResult = manifold.RunTachyonSplittingManifold();

        Console.WriteLine($"The tachyon manifold split a beam into {splittingResult.tachyonPathCount} probable paths");
    }

    public static void Main(string[] args)
    {
        // PerformTests();
        // PerformPuzzleOne();
        PerformPuzzleTwo();
    }
}