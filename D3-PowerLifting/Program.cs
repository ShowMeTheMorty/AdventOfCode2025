// https://adventofcode.com/2025/day/3

public static class Program
{
    static IEnumerable<BatteryArray> GetExampleBatteryArrays ()
    {
        const string exampleData = @"987654321111111
811111111111119
234234234234278
818181911112111";

        return exampleData
            .Split('\n')
            .Select(BatteryArray.FromString);
    }

    static IEnumerable<BatteryArray> ImportBatteryArrays()
    {
        string inputData;

        using (FileStream fileStream = new FileStream(@".\input.txt", FileMode.Open))
        using (StreamReader reader = new StreamReader(fileStream))
        {
            inputData = reader.ReadToEnd();
        }

        return inputData
            .Split('\n')
            .Where(str => !string.IsNullOrEmpty(str))
            .Select(BatteryArray.FromString);
    }

    static void AnalyzeOptimalJoltageFromBatteryArrays (IEnumerable<BatteryArray> batteryArrays, int sequenceLength=2)
    {
        long totalJoltage = 0;
        foreach (BatteryArray batteryArray in batteryArrays)
        {
            long joltage = batteryArray.GetBestJoltage(count: sequenceLength);
            Console.WriteLine($"Best joltage sequence ({sequenceLength}) for {batteryArray} is {joltage}");

            totalJoltage += joltage;
        }

        Console.WriteLine($"Total optimal joltage from battery arrays is {totalJoltage}");        
    }

    static void PerformTests()
    {
        IEnumerable<BatteryArray> batteryArrays = GetExampleBatteryArrays();
        AnalyzeOptimalJoltageFromBatteryArrays(batteryArrays, sequenceLength: 7);
    }

    static void PerformPuzzleOne()
    {
        IEnumerable<BatteryArray> batteryArrays = ImportBatteryArrays();
        AnalyzeOptimalJoltageFromBatteryArrays(batteryArrays, sequenceLength: 2);
    }
    
    static void PerformPuzzleTwo ()
    {
        IEnumerable<BatteryArray> batteryArrays = ImportBatteryArrays();
        AnalyzeOptimalJoltageFromBatteryArrays(batteryArrays, sequenceLength: 12);
    }

    public static void Main(string[] args)
    {
        // PerformTests();
        // PerformPuzzleOne();
        PerformPuzzleTwo();
    }
}
