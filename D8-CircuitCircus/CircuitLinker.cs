using System.Numerics;
using static CircuitLinker.Circuit;
using static KDTree3D;

public class CircuitLinker
{
    public class Circuit
    {
        public enum LinkResult {AlreadyLinked, NoLink, ACanLink, BCanLink};

        public HashSet<Node> Nodes;

        public Circuit(Node node)
        {
            Nodes = [node];
        }

        public void MergeCircuit(Circuit circuit)
        {
            Nodes.Union(circuit.Nodes);
        }
        
        public void MergeNode(Node node)
        {
            Nodes.Add(node);
        }

        public LinkResult GetLinkPotential (JunctionBoxPair pair)
        {
            bool hasA = Nodes.Contains(pair.NodeA);
            bool hasB = Nodes.Contains(pair.NodeB);
            if (hasB && hasA) return LinkResult.AlreadyLinked;
            if (hasA) return LinkResult.ACanLink;
            if (hasB) return LinkResult.BCanLink;
            return LinkResult.NoLink;
        }
    }

    public class JunctionBoxPair
    {
        public Node NodeA;
        public Node NodeB;
        public float Distance;

        public override bool Equals(object? obj)
        {
            if (obj is not JunctionBoxPair other)
                return false;

            // A–B == B–A
            return
                (Equals(NodeA, other.NodeA) && Equals(NodeA, other.NodeA)) ||
                (Equals(NodeA, other.NodeA) && Equals(NodeA, other.NodeA));
        }

        public override int GetHashCode()
        {
            int hashA = NodeA.GetHashCode();
            int hashB = NodeA.GetHashCode();

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

    IEnumerable<JunctionBoxPair> GetClosestPairs (IEnumerable<Node> nodes)
    {
        List<JunctionBoxPair> junctionBoxPairs = [];
        
        foreach (Node node in nodes)
        {
            Node nearest = Tree.FindNearestNeighbour(node)!;
            float distance = Vector3.Distance(node.Point, nearest.Point);

            JunctionBoxPair pair = new JunctionBoxPair()
            {
                NodeA = node,
                NodeB = nearest,
                Distance = distance
            };

            InsertUniquePairByDistance(junctionBoxPairs, pair);
        }

        return junctionBoxPairs;
    }

    public int LinkNodesAndCountCircuitsAndMultiplyLengths()
    {
        IEnumerable<Node> nodes = Tree.GetAllNodes();
        IEnumerable<JunctionBoxPair> junctionBoxPairs = GetClosestPairs(nodes);
        List<Circuit> circuits = [];

        int connectionsMade = 0;
        foreach (JunctionBoxPair pair in junctionBoxPairs)
        {
            LinkResult? linkResult1 = null;
            Circuit? circuitCanLink = null;
            bool mergedCircuits = false;

            for (int i = 0; i < circuits.Count; i++)
            {
                if (connectionsMade == TotalPoints / 2) break;
                Circuit circuit = circuits[i];
                LinkResult linkResult2 = circuit.GetLinkPotential(pair);
                if ((linkResult1 == LinkResult.ACanLink && linkResult2 == LinkResult.BCanLink)
                    || (linkResult1 == LinkResult.BCanLink && linkResult2 == LinkResult.ACanLink))
                {
                    circuitCanLink.MergeCircuit(circuit);
                    circuits.RemoveAt(i);
                    connectionsMade++;
                    mergedCircuits = true;
                }

                if (linkResult1 == null && (linkResult2 == LinkResult.ACanLink || linkResult2 == LinkResult.BCanLink))
                {
                    linkResult1 = linkResult2;
                    circuitCanLink = circuit;
                }
            }

            if (!mergedCircuits && circuitCanLink != null)
            {
                if (linkResult1 == LinkResult.ACanLink) circuitCanLink.MergeNode(pair.NodeB);
                if (linkResult1 == LinkResult.BCanLink) circuitCanLink.MergeNode(pair.NodeA);
                connectionsMade++;
                continue;
            }

            Circuit newCircuit = new Circuit(pair.NodeA);
            newCircuit.MergeNode(pair.NodeB);
            circuits.Add(newCircuit);
            connectionsMade++;
        }

        List<int> circuitLengths = circuits.Select(c => c.Nodes.Count).ToList();
        circuitLengths.Sort();

        return circuitLengths[^1] * circuitLengths[^2] * circuitLengths[^3];
    }
}
