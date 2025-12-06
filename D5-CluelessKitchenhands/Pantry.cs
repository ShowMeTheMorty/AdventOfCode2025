
public class Pantry
{
    public struct FreshRange (long IdMin, long IdMax)
    {
        public bool IsIdInRange(long id)
        {
            return id >= IdMin && id <= IdMax;
        }
        
        public static FreshRange FromString (string data)
        {
            var numericParts = data.Split('-').Select(long.Parse).ToArray();
            return new FreshRange(numericParts[0], numericParts[1]);
        }
    }

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
}