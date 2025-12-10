using System.Numerics;
using static CircuitLinker.Circuit;
using static KDTree3D;

public class CircuitLinker
{
    public class Circuit
    {
        public enum JoinResult { NotInCircuit, Joined, AlreadyConnected }

        public List<JunctionBoxPair> LinkedPairs;

        public Circuit (JunctionBoxPair initialPair)
        {
            LinkedPairs = [initialPair];
        }

        public JoinResult TryJoinToCircuit (JunctionBoxPair incoming)
        {
            IEnumerable<Vector3> allPoints = LinkedPairs.SelectMany(pair => new Vector3[]{pair.BoxA, pair.BoxB}).ToArray();
            bool containsA = allPoints.Contains(incoming.BoxA);
            bool containsB = allPoints.Contains(incoming.BoxB);

            bool containsAtLeastOne = containsA || containsB;
            bool containsBoth = containsA && containsB;

            if (containsBoth) return JoinResult.AlreadyConnected;
            if (containsAtLeastOne)
            {
                LinkedPairs.Add(incoming);
                return JoinResult.Joined;
            }
            return JoinResult.NotInCircuit;
        }

        public int GetCircuitSize ()
        {
            return LinkedPairs.Count + 1;
        }
    }

    public class JunctionBoxPair
    {
        public Vector3 BoxA;
        public Vector3 BoxB;
        public float Distance;

        public override bool Equals(object? obj)
        {
            if (obj is not JunctionBoxPair other)
                return false;

            // A–B == B–A
            return
                (Equals(BoxA, other.BoxA) && Equals(BoxB, other.BoxB)) ||
                (Equals(BoxA, other.BoxB) && Equals(BoxB, other.BoxA));
        }

        public override int GetHashCode()
        {
            int hashA = BoxA.GetHashCode();
            int hashB = BoxB.GetHashCode();

            return hashA ^ hashB;
        }
    }

    readonly KDTree3D Tree;
    readonly int TotalPoints;

    public CircuitLinker(string data)
    {
        Vector3[] points = data
            .Split('\n')
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrEmpty(str))
            .Select(str =>
            {
                var parts = str.Split(',');
                return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            })
            .ToArray();

        TotalPoints = points.Length;
        Tree = new(points);
    }

    public static CircuitLinker FromFile(string path)
    {
        return new CircuitLinker(File.ReadAllText(path));
    }

    bool InsertUniquePairByDistance (List<JunctionBoxPair> pairs, JunctionBoxPair incoming)
    {
        if (pairs.Contains(incoming)) return false;
        for (int i=0; i<pairs.Count; i++)
        {
            JunctionBoxPair test = pairs[i];
            if (incoming.Distance < test.Distance)
            {
                pairs.Insert(i, incoming);
                return true;
            }
        }

        pairs.Add(incoming);
        return true;
    }

    IEnumerable<JunctionBoxPair> GetClosestPairs ()
    {
        List<JunctionBoxPair> junctionBoxPairs = [];
        
        foreach (Node node in Tree.GetAllNodes())
        {
            Node nearest = Tree.FindNearestNeighbour(node)!;
            float distance = Vector3.Distance(node.Point, nearest.Point);

            JunctionBoxPair pair = new JunctionBoxPair()
            {
                BoxA = node.Point,
                BoxB = nearest.Point,
                Distance = distance
            };

            InsertUniquePairByDistance(junctionBoxPairs, pair);
        }

        return junctionBoxPairs;
    }

    public int LinkNodesAndCountCircuitsAndMultiplyLengths()
    {
        List<Circuit> circuits = [];
        IEnumerable<JunctionBoxPair> junctionBoxPairs = GetClosestPairs();

        int connectionsMade = 0;
        foreach (JunctionBoxPair pair in junctionBoxPairs)
        {
            if (connectionsMade == TotalPoints) break;
            
            List<Circuit> containingCircuits = new List<Circuit>();

            foreach (Circuit circuit in circuits)
            {
                var allPoints = circuit.LinkedPairs.SelectMany(p => new[] { p.BoxA, p.BoxB });
                if (allPoints.Contains(pair.BoxA) || allPoints.Contains(pair.BoxB))
                    containingCircuits.Add(circuit);
            }

            if (containingCircuits.Count == 0)
            {
                // neither endpoint in any circuit -> new circuit
                circuits.Add(new Circuit(pair));
                connectionsMade++;
            }
            else if (containingCircuits.Count == 1)
            {
                // only one circuit contains an endpoint -> add to it
                containingCircuits[0].LinkedPairs.Add(pair);
                connectionsMade++;
            }
            else
            {
                // both endpoints in different circuits -> merge them
                Circuit first = containingCircuits[0];
                for (int i = 1; i < containingCircuits.Count; i++)
                {
                    first.LinkedPairs.AddRange(containingCircuits[i].LinkedPairs);
                    circuits.Remove(containingCircuits[i]);
                }
                first.LinkedPairs.Add(pair);
                connectionsMade++;
            }
        }

        List<int> circuitLengths = circuits
            .Select(c => c.GetCircuitSize())
            .ToList();

        int nodesInCircuits = circuits.Sum(c => c.GetCircuitSize());
        int singletons = TotalPoints - nodesInCircuits;

        for (int i = 0; i < singletons; i++)
            circuitLengths.Add(1);

        circuitLengths.Sort();

        return circuitLengths[^1] * circuitLengths[^2] * circuitLengths[^3];
    }
}
