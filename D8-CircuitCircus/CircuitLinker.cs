using System.Drawing;
using System.Formats.Asn1;
using System.Numerics;
using static KDTree3D;

public class CircuitLinker
{
    public class JunctionBoxPair
    {
        public Vec3 PointA;
        public Vec3 PointB;
        public float DistanceSquared;

        public override string ToString()
        {
            return $"{{({PointA.X}, {PointA.Y}, {PointA.Z}) <-> ({PointB.X}, {PointB.Y}, {PointB.Z}): {Math.Sqrt(DistanceSquared)}}}";
        }

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

    readonly Vec3[] Points;

    public CircuitLinker(string data)
    {
        Points = data
            .Split('\n')
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrEmpty(str))
            .Select(str =>
            {
                var parts = str.Split(',');
                return new Vec3(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2]));
            })
            .ToArray();
    }

    public static CircuitLinker FromFile(string path)
    {
        return new CircuitLinker(File.ReadAllText(path));
    }

    JunctionBoxPair[] GetAllDistancePairsOrdered ()
    {
        List<JunctionBoxPair> allPairs = new List<JunctionBoxPair>();
        for (int i = 0; i < Points.Length; i++)
        {
            for (int j = i + 1; j < Points.Length; j++)
            {
                allPairs.Add(new JunctionBoxPair {
                    PointA = Points[i],
                    PointB = Points[j],
                    DistanceSquared = Vec3.DistanceSquared(Points[i], Points[j])
                });
            }
        }
        allPairs.Sort((a,b) => a.DistanceSquared.CompareTo(b.DistanceSquared));
        return allPairs.ToArray();

    }

    public int GetMagicNumber(bool keepGoing=false)
    {
        JunctionBoxPair[] distancePairsOrdered = GetAllDistancePairsOrdered();
        List<HashSet<Vec3>> circuits = new List<HashSet<Vec3>>();

        Console.WriteLine(distancePairsOrdered[0]);
        Console.WriteLine(distancePairsOrdered[1000]);
        Console.WriteLine(distancePairsOrdered[5000]);

        int linked = 0;
        JunctionBoxPair? lastConnectedPair = null;
        foreach (JunctionBoxPair pair in distancePairsOrdered)
        {
            if (!keepGoing && linked == Points.Length) break;

            HashSet<Vec3>? matchedA = null;
            HashSet<Vec3>? matchedB = null;

            bool alreadyUsed = false;
            foreach (HashSet<Vec3> circuit in circuits)
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
                circuits.Add(new HashSet<Vec3>() { pair.PointA, pair.PointB });
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

        Console.WriteLine(lastConnectedPair);

        List<int> circuitLengths = circuits.Select(set => set.Count).ToList();
        circuitLengths.Sort();

        if (keepGoing) return (int)(lastConnectedPair!.PointA.X * lastConnectedPair!.PointB.X);
        return circuitLengths[^1] * circuitLengths[^2] * circuitLengths[^3];
    }
}
