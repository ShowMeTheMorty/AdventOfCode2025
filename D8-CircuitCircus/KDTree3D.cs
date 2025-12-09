using System.Numerics;

public class KDTree3D
{
    public class Node
    {
        public Vector3 Point;
        public int Axis;
        public Node? Left;
        public Node? Right;

        public Node(Vector3 point, int axis)
        {
            Point = point;
            Axis = axis;
            Left = null;
            Right = null;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Node other)
                return false;

            return Point.Equals(other.Point) && Axis == other.Axis;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Point, Axis);
        }
    }
    
    readonly Node RootNode;

    public KDTree3D (Vector3[] points)
    {
        RootNode = BuildKDTree(points);
    }

    public static Node BuildKDTree(Vector3[] points, int depth = 0)
    {
        if (points.Length == 0)
            return null;

        int axis = depth % 3;

        int medianIndex = QuickSelect3D.GetMedianIndex(points, axis);
        Vector3 medianPoint = points[medianIndex];

        List<Vector3> leftPoints = [];
        List<Vector3> rightPoints = [];
        for (int i = 0; i < points.Length; i++)
        {
            if (i == medianIndex) continue;
            if (points[i][axis] < medianPoint[axis])
                leftPoints.Add(points[i]);
            else
                rightPoints.Add(points[i]);
        }

        Node node = new(medianPoint, axis)
        {
            Left = BuildKDTree(leftPoints.ToArray(), depth + 1),
            Right = BuildKDTree(rightPoints.ToArray(), depth + 1)
        };

        return node;
    }
    
    public Node? FindNearestNeighbour(Node targetNode)
    {
        return FindNearestNeighbor(RootNode, targetNode, 0, null, float.MaxValue);
    }

    private Node? FindNearestNeighbor(Node? node, Node targetNode, int depth, Node? best, float bestDist)
    {
        if (node == null)
            return best;

        if (node != targetNode)
        {
            float dist = Vector3.DistanceSquared(targetNode.Point, node.Point);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = node;
            }
        }

        int axis = depth % 3;

        Node? first, second;
        if (targetNode.Point[axis] < node.Point[axis])
        {
            first = node.Left;
            second = node.Right;
        }
        else
        {
            first = node.Right;
            second = node.Left;
        }

        best = FindNearestNeighbor(first, targetNode, depth + 1, best, bestDist);
        if (best != null) bestDist = Vector3.DistanceSquared(targetNode.Point, best.Point);

        float axisDist = (targetNode.Point[axis] - node.Point[axis]) * (targetNode.Point[axis] - node.Point[axis]);
        if (axisDist < bestDist)
        {
            best = FindNearestNeighbor(second, targetNode, depth + 1, best, bestDist);
        }

        return best;
    }

    public IEnumerable<Node> GetAllNodes()
{
    return Traverse(RootNode);
}

    IEnumerable<Node> Traverse(Node? node)
    {
        if (node == null) yield break;
        yield return node;
        foreach (Node left in Traverse(node.Left)) yield return left;
        foreach (Node right in Traverse(node.Right)) yield return right;
    }
}