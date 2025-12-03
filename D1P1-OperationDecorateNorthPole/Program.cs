// https://adventofcode.com/2025/day/1

using System.IO;
using System.Linq;

public static class Program
{
    public static int DecodeInstruction (string instruction)
    {
        int sign = instruction[0] == 'L' ? -1 : 1;
        int clicks = int.Parse(instruction[1..]) * sign;

        return clicks;
    }

    static void PerformExamplarTest ()
    {
        Safe safe = new Safe(Position: 50, CountZeroPasses: true);
        
        string testInstructions = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";
        IEnumerable<int> decodedInstructions = testInstructions
            .Split('\n')
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(DecodeInstruction);
        

        foreach (int instruction in decodedInstructions)
        {
            safe.Rotate(instruction);
        }

        safe.LogZeroStateAccumulator();
    }

    static void PerformTests ()
    {
        Safe safe = new Safe(Position: 0);
        safe.Rotate(-100);
        safe.Rotate(-200);
        safe.Rotate(-240);
        safe.Rotate(40);
        safe.Rotate(-1);
        safe.Rotate(-99);
        safe.Rotate(-199);


        safe.LogZeroStateAccumulator();

    }

    static void PerformPuzzle ()
    {
        IEnumerable<int> decodedInstructions;
        IEnumerable<string> rawInstructions;

        using (FileStream fs = new FileStream(@".\input.txt", FileMode.Open))
        using (StreamReader sr = new StreamReader(fs))
        {
            rawInstructions = sr.ReadToEnd()
                .Split('\n')
                .Where(line => !string.IsNullOrWhiteSpace(line));
        }        

        decodedInstructions = rawInstructions.Select(DecodeInstruction);

        Safe safe = new Safe(Position: 50, CountZeroPasses: true);
        foreach (int instruction in decodedInstructions)
        {
            safe.Rotate(instruction);
        }
        safe.LogZeroStateAccumulator();
    }

    public static void Main()
    {
        // PerformTests();
        // PerformExamplarTest();
        PerformPuzzle();
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
