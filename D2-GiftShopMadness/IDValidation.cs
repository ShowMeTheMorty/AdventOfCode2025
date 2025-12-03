public static class IDValidation
{
    public static Func<int, bool> IsRepeatsInvalidFunction { private get; set; } = (int repeatCount) =>
    {
        return repeatCount == 2; // two repeats only
    };


    static Dictionary<int, int[]> factorsByLength = new Dictionary<int, int[]>
    {
        { 2, new int[] { 2 }},
        { 3, new int[] { 3 }},
        { 4, new int[] { 2, 4 }},
        { 5, new int[] { 5 }},
        { 6, new int[] { 2, 3, 6 }},
        { 7, new int[] { 7 }},
        { 8, new int[] { 2, 4, 8 }},
        { 9, new int[] { 3, 9 }},
        { 10, new int[] { 2, 5, 10 }},
        { 11, new int[] { 11 }},
        { 12, new int[] { 2, 3, 4, 6, 12 }},
        { 13, new int[] { 13 }},
        { 14, new int[] { 2, 7, 14 }},
        { 15, new int[] { 3, 5, 15 }},
        { 16, new int[] { 2, 4, 8, 16 }},
        { 17, new int[] { 17 }},
        { 18, new int[] { 2, 3, 6, 9, 18 }},
        { 19, new int[] { 19 }},
        { 20, new int[] { 2, 4, 5, 10, 20 }}
    };

    static bool IsIDValid (long id)
    {
        string strID = id.ToString();
        int length = strID.Length;

        if (length == 1) return true;
        else if (!factorsByLength.ContainsKey(length))
        {
            throw new Exception($"No factors defined for ID length {length}");
            // could use this to build out the factors table
        }

        foreach (int factor in factorsByLength[length])
        {
            int subLength = length / factor;
            string testString = strID.Substring(0, subLength);

            bool repeatsHarmonic = true;
            int repeatsCount = 1;

            for (int i = 1; i < factor; i++)
            {
                string nextTest = strID.Substring(i * subLength, subLength);
                if (nextTest != testString)
                {
                    repeatsHarmonic = false;
                    break;
                }
                else
                {
                    repeatsCount++;
                }
            }

            if (repeatsHarmonic && IsRepeatsInvalidFunction(repeatsCount)) // we only invalidate if it repeats twice
            {
                return false;
            }
        }

        return true;
    }

    static IEnumerable<long> FindInvalidIDs(IEnumerable<long> ids)
    {
        foreach (long id in ids)
        {
            if (!IsIDValid(id))
            {
                Console.WriteLine(id);
                yield return id;
            }
        }
    }

    static IEnumerable<long> UnpackIDRange(string idRangeString)
    {
        long[] ints = idRangeString.Split('-').Select(long.Parse).ToArray();
        long start = ints[0];
        long end = ints[1];

        for (long i = start; i <= end; i++)
            yield return i;
    }
    
    public static IEnumerable<long> FindInvalidIDsInIDRangeSet (string idRangeSet)
    {
        return idRangeSet.Split(',')
            .SelectMany(idRangeString =>
                FindInvalidIDs(UnpackIDRange(idRangeString)));
    }
}