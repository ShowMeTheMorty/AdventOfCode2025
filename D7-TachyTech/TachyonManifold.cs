public class TachyonManifold
{
    public struct Splitter
    {
        public int location;
    }
    
    HashSet<int> Beams = [];
    Dictionary<int, Splitter> SplitterBanks = [];

    public TachyonManifold(string data)
    {
        IEnumerable<string> lines = data
            .Split('\n')
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrWhiteSpace(str));
        
        foreach (string line in lines)
        {
            
        }
    }
    
    public static TachyonManifold FromFile (string path)
    {
        return new TachyonManifold(File.ReadAllText(path));
    }   
}