using System.Numerics;
using static KDTree3D;

public class CircuitLinker
{
    public class Circuit
    {
        public List<Node> Nodes = [];
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

    public int LinkNodesAndCountCircuitsAndMultiplyLengths()
    {
        List<Circuit> circuits = [];

        foreach (Node node in Tree.GetAllNodes())
        {
            Node nearest = Tree.FindNearestNeighbour(node)!;
            bool foundExistingCircuit = false;
            int circuitsFound = 0;
            foreach (Circuit circuit in circuits)
            {
                bool containsNode = circuit.Nodes.Contains(node);
                bool containsNearest = circuit.Nodes.Contains(nearest);
                if (containsNearest && !containsNode) circuit.Nodes.Add(node);
                if (containsNode && !containsNearest) circuit.Nodes.Add(nearest);
                if (containsNearest || containsNode)
                {
                    foundExistingCircuit = true;
                    if (!containsNearest && containsNode) circuitsFound++;
                    break;
                }
            }
            if (!foundExistingCircuit)
            {
                circuits.Add(new Circuit() { Nodes = [node, nearest] });
                circuitsFound++;
            }
            if (circuitsFound == TotalPoints) break;
        }

        List<int> circuitLengths = circuits.Select(c => c.Nodes.Count).ToList();
        circuitLengths.Sort();

        return circuitLengths[^1] * circuitLengths[^2] * circuitLengths[^3];
    }
}
