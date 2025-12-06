public class FreshRange
{   
    public long IdMin;
    public long IdMax;

    public FreshRange(long idMin, long idMax)
    {
        IdMin = idMin;
        IdMax = idMax;
    }

    public bool IsIdInRange(long id)
    {
        return id >= IdMin && id <= IdMax;
    }

    public bool TryUnionWithRange(FreshRange other)
    {
        if (IdMin <= other.IdMax && other.IdMin <= IdMax)
        {
            IdMin = Math.Min(IdMin, other.IdMin);
            IdMax = Math.Max(IdMax, other.IdMax);
            return true;
        }
        return false;
    }
    
    public static FreshRange FromString (string data)
    {
        var numericParts = data.Split('-').Select(long.Parse).ToArray();
        return new FreshRange(numericParts[0], numericParts[1]);
    }
}