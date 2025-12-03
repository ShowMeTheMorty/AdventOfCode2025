

public static class IDValidation
{
    static IEnumerbale<int> UnpackIDRange (string idRangeString)
    {
        var ints = idRangeString.Split('-').Select(int.Parse).ToArray();

        Range range = new Range();
    }
    
    static IEnumerable<int> FindInvalidIDs (IEnumerable<string> ids)
    {
        
    }
}