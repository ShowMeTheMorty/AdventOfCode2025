
public class Pantry
{
    readonly IEnumerable<FreshRange> freshRanges;
    readonly IEnumerable<long> ingredientIds;

    public Pantry (string data)
    {
        IEnumerable<string> lines = data.Split('\n')
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrEmpty(str));

        IEnumerable<string> rangeData = lines.Where(line => line.Contains('-'));
        IEnumerable<string> ingredientIdData = lines.Where(line => !line.Contains('-'));

        freshRanges = rangeData.Select(FreshRange.FromString);
        ingredientIds = ingredientIdData.Select(long.Parse);
        Console.WriteLine();
    }

    public static Pantry FromFile(string path)
    {
        return new Pantry(File.ReadAllText(path));
    }

    public IEnumerable<long> GetFreshIngredientIds()
    {
        return ingredientIds.Where(id =>
        {
            foreach (FreshRange range in freshRanges)
            {
                if (range.IsIdInRange(id)) return true;
            }
            return false;
        });
    }
    
    public long GetAllPossibleFreshIngredientIdCount()
    {
        long possibleIngredentIdCount = 0;
        List<FreshRange> rangePool = freshRanges.ToList();
        List<FreshRange> unionedRanges = [];

        while (rangePool.Count > 0)
        {
            FreshRange baseRange = rangePool[0];

            // we will union and accumulate any intersecting ranges and remove them from the pool
            int i = 1;
            int unionCount = 0;
            while (i < rangePool.Count || unionCount > 0)
            {
                // start over unitl all possible unions are made
                if (i >= rangePool.Count) 
                {
                    i = 1;
                    unionCount = 0;
                }

                if (baseRange.TryUnionWithRange(rangePool[i]))
                {
                    unionCount++;
                    rangePool.RemoveAt(i);
                }
                // we can't union YET, so keep scanning pool
                else i++;
            }

            unionedRanges.Add(baseRange);
            rangePool.RemoveAt(0);
        }

        foreach (FreshRange range in unionedRanges)
        {
            possibleIngredentIdCount += range.IdMax - range.IdMin + 1;
        }

        return possibleIngredentIdCount;
    }
}