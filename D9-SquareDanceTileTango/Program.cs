public static class Program
{
    static void PerformTest()
    {
        string testData = @"7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3";

        SquareSeeker seeker = new(testData);
        Console.WriteLine($"The largest possible square has an area of {seeker.FindLargestSquarea()}");
    }

    static void PerformPuzzleOne()
    {
        SquareSeeker seeker = SquareSeeker.FromFile(@".\input.txt");
        Console.WriteLine($"The largest possible square has an area of {seeker.FindLargestSquarea()}");
    }

    static void PerformPuzzleTwo()
    {
        SquareSeeker seeker = SquareSeeker.FromFile(@".\input.txt");
        Console.WriteLine($"The largest possible square has an area of {seeker.FindLargestSquarea(anySquarea: false)}");
    }
    
    public static void Main ()
    {
        // PerformTest();
        PerformPuzzleTwo(); 
    }
}