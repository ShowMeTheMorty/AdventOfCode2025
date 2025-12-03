public static class Program
{
    static string ImportData ()
    {
        using (FileStream fileStream = new FileStream(@".\input.txt", FileMode.Open))
        using (StreamReader reader = new StreamReader(fileStream))
        {
            return reader.ReadToEnd();
        }
    }

    static void PerformTests ()
    {
        string rangeSet = @"11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";
        IEnumerable<long> invalidIds = IDValidation.FindInvalidIDsInIDRangeSet(rangeSet);

        Console.WriteLine("Invalid IDs:");
        foreach (int ID in invalidIds)
        {
            Console.WriteLine(ID);
        }

        Console.WriteLine($"Cumulative value is {invalidIds.Sum()}");
    }

    static void PerformPuzzleOne()
    {
        string idRangeSet = ImportData();
        IEnumerable<long> invalidIds = IDValidation.FindInvalidIDsInIDRangeSet(idRangeSet);
        Console.WriteLine($"Cumulative value is {invalidIds.Sum()}");
    }
    
    static void PerformPuzzleTwo ()
    {
        string idRangeSet = ImportData();

        // Change the repeat invalidation logic to at least 2 repeats
        IDValidation.IsRepeatsInvalidFunction = (int repeats) => repeats >= 2;

        IEnumerable<long> invalidIds = IDValidation.FindInvalidIDsInIDRangeSet(idRangeSet);
        Console.WriteLine($"Cumulative value is {invalidIds.Sum()}");
    }


    public static void Main (string[] args)
    {
        // PerformTests();
        // PerformPuzzleOne();
        PerformPuzzleTwo();
    }
}