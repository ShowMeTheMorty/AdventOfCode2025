using System.Drawing;
using System.Formats.Asn1;
using System.Numerics;
using static KDTree3D;

public class CircuitLinker
{
    public class JunctionBoxPair
    {
        public Vector3 PointA;
        public Vector3 PointB;
        public float DistanceSquared;

        public override bool Equals(object? obj)
        {
            if (obj is not JunctionBoxPair other)
                return false;

            // A–B == B–A
            return
                (Equals(PointA, other.PointA) && Equals(PointB, other.PointB)) ||
                (Equals(PointA, other.PointB) && Equals(PointB, other.PointA));
        }

        public override int GetHashCode()
        {
            int hashA = PointA.GetHashCode();
            int hashB = PointB.GetHashCode();

            return hashA ^ hashB;
        }
    }

    readonly Vector3[] Points;

    public CircuitLinker(string data)
    {
        Points = data
            .Split('\n')
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrEmpty(str))
            .Select(str =>
            {
                var parts = str.Split(',');
                return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            })
            .ToArray();
    }

    public static CircuitLinker FromFile(string path)
    {
        return new CircuitLinker(File.ReadAllText(path));
    }

    IEnumerable<JunctionBoxPair> GetAllDistancePairsOrdered ()
    {
        List<JunctionBoxPair> allPairs = new List<JunctionBoxPair>();
        for (int i = 0; i < Points.Length; i++)
        {
            for (int j = i + 1; j < Points.Length; j++)
            {
                allPairs.Add(new JunctionBoxPair {
                    PointA = Points[i],
                    PointB = Points[j],
                    DistanceSquared = Vector3.DistanceSquared(Points[i], Points[j])
                });
            }
        }
        allPairs.Sort((a,b) => a.DistanceSquared.CompareTo(b.DistanceSquared));
        return allPairs;

    }

    public int GetMagicNumber(bool keepGoing=false)
    {
        IEnumerable<JunctionBoxPair> distancePairsOrdered = GetAllDistancePairsOrdered();
        List<HashSet<Vector3>> circuits = new List<HashSet<Vector3>>();

        int linked = 0;
        JunctionBoxPair? lastConnectedPair = null;
        foreach (JunctionBoxPair pair in distancePairsOrdered)
        {
            if (!keepGoing && linked == Points.Length) break;

            HashSet<Vector3>? matchedA = null;
            HashSet<Vector3>? matchedB = null;

            bool alreadyUsed = false;
            foreach (HashSet<Vector3> circuit in circuits)
            {
                if (matchedA != null && matchedB != null) break;

                bool hasA = circuit.Contains(pair.PointA);
                bool hasB = circuit.Contains(pair.PointB);

                if (hasA && hasB)
                {
                    alreadyUsed = true;
                    break;
                }

                if (hasA) matchedA = circuit;
                else if (hasB) matchedB = circuit;
            }

            if (alreadyUsed)
            {
                // do nothing
            }
            else if (matchedA == null && matchedB == null)
            {
                circuits.Add(new HashSet<Vector3>() { pair.PointA, pair.PointB });
            }
            else if (matchedA != null && matchedB != null)
            {
                matchedA.UnionWith(matchedB);
                circuits.Remove(matchedB);
            }
            else if (matchedA != null)
            {
                matchedA.Add(pair.PointB);
            }
            else if (matchedB != null)
            {
                matchedB.Add(pair.PointA);
            }

            if (keepGoing && circuits[0].Count == Points.Length)
            {
                lastConnectedPair = pair;
                break;
            }

            linked++;
        }

        List<int> circuitLengths = circuits.Select(set => set.Count).ToList();
        circuitLengths.Sort();

        if (keepGoing) return (int)(lastConnectedPair!.PointA.X * lastConnectedPair!.PointB.X);
        return circuitLengths[^1] * circuitLengths[^2] * circuitLengths[^3];
    }
}
