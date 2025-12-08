using System.Net;

public class TachyonManifold
{
    public struct TachyonSplittingResult
    {
        public int splitterActivationCount;
        public long tachyonPathCount;
    }
    
    struct Splitter
    {
        public int location;
    }

    class Beam
    {
        public long possibleAncestralPaths = 1;
    }
    
    Dictionary<int, Beam> PositionedBeams = [];
    Dictionary<int, IEnumerable<Splitter>> SplitterBanks = [];
    readonly int BeamEntryPoint;
    readonly int TotalRanks;

    public TachyonManifold(string data)
    {
        string[] lines = data
            .Split('\n')
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .ToArray();
        
        // interate through ranks
        for (int i=0; i<lines.Count(); i++)
        {
            string line = lines[i];
            // beam emission is on the first rank
            if (i == 0) 
            {
                BeamEntryPoint = line.IndexOf('S');
                continue;
            }

            List<Splitter> splitters = [];
            // find splitters
            for (int c=0; c<lines[0].Length; c++)
            {
                if (line[c] == '^') 
                    splitters.Add(new Splitter {location=c});
            }

            if (splitters.Count > 0) SplitterBanks.Add(i, splitters);
            TotalRanks = i;
        }
    }

    void SpawnBeam (int position, long currentAncestralPaths)
    {
        if (!PositionedBeams.TryAdd(position, new Beam() {possibleAncestralPaths = currentAncestralPaths}))
        {
            // Beam exists
            PositionedBeams[position].possibleAncestralPaths += currentAncestralPaths;
        }
    }

    public TachyonSplittingResult RunTachyonSplittingManifold ()
    {
        PositionedBeams.Add(BeamEntryPoint, new Beam ());

        int splitterActivationCount = 0;
        // iterate through splitter banks adding and removing beams on encountering splitter
        for (int rank=1; rank<TotalRanks; rank++)
        {
            // no splitters mave on
            if (!SplitterBanks.ContainsKey(rank)) continue;
            foreach (Splitter splitter in SplitterBanks[rank])
            {
                if (PositionedBeams.ContainsKey(splitter.location))
                {
                    // split beam
                    long currentAncestralPaths = PositionedBeams[splitter.location].possibleAncestralPaths;
                    
                    PositionedBeams.Remove(splitter.location);
                    
                    SpawnBeam(splitter.location+1, currentAncestralPaths);
                    SpawnBeam(splitter.location-1, currentAncestralPaths);

                    splitterActivationCount++;
                }
            }
        }

        long tachyonPathCount = PositionedBeams.Select(kvp => kvp.Value.possibleAncestralPaths).Sum();
        
        // beams dissipate
        PositionedBeams = [];

        return new TachyonSplittingResult
        {
            splitterActivationCount = splitterActivationCount,
            tachyonPathCount = tachyonPathCount
        };
    }
    
    public static TachyonManifold FromFile (string path)
    {
        return new TachyonManifold(File.ReadAllText(path));
    }   
}