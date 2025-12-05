
public class Pantry
{
    public struct FreshRange (int IdMin, int IdMax)
    {
        public bool IsIdInRange (int id)
        {
            return id > IdMin && id < IdMax;
        }
    }

    readonly IEnumerable<FreshRange> freshRanges;

    public Pantry (string data)
    {
        

    }

    public static Pantry FromFile (string path)
    {
        File.ReadAllLines(path);
        
    }
}