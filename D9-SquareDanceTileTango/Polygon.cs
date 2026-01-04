using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class Polygon
{
    readonly Edge[] AAEdges;
    
    readonly Vertex[] OuterCellsWithinBounds;
    readonly MinMax2D MinMaxVertex;

    public Polygon(Vertex[] vertices)
    {
        List<Edge> edges = [];

        for (int i = 0, j = 1; i < vertices.Length; i++, j++)
        {
            if (j == vertices.Length) j = 0; // wrap to start

            edges.Add(new Edge(vertices[i], vertices[j]));
        }

        AAEdges = edges.ToArray();
        MinMaxVertex = MinMax2D.FromParallelReduction(vertices);
        OuterCellsWithinBounds = FloodFillInaccesibleCells().ToArray();

        Console.WriteLine("polygon initialised");
    }

    bool IsCellInsidePolygon(Vertex cell)
    {
        if (!IsCellInLocalRange(cell)) return false;
        if (OuterCellsWithinBounds.Contains(cell)) return false;
        return true;
    }

    bool IsCellOnEdge(Vertex cell)
    {
        foreach (Edge edge in AAEdges)
        {
            bool inline = edge.IsFlat ? cell.Y == edge.A.Y : cell.X == edge.A.X;
            if (!inline) continue;

            long edgeMin = edge.IsFlat ? Math.Min(edge.A.X, edge.B.X) : Math.Min(edge.A.Y, edge.B.Y);
            long edgeMax = edge.IsFlat ? Math.Max(edge.A.X, edge.B.X) : Math.Max(edge.A.Y, edge.B.Y);
            long cellValue = edge.IsFlat ? cell.X : cell.Y;

            bool onEdge = cellValue >= edgeMin && cellValue <= edgeMax;
            if (onEdge) return true;
        }
        return false;
    }

    bool IsCellInLocalRange(Vertex cell)
    {
        // add a border to acceptable range
        return !(cell.X < MinMaxVertex.Min.X - 1
            || cell.Y < MinMaxVertex.Min.Y - 1
            || cell.X > MinMaxVertex.Max.X + 1
            || cell.Y > MinMaxVertex.Max.Y + 1);
    }

    IEnumerable<Vertex> FloodFillInaccesibleCells()
    {
        Console.WriteLine("flood filling polygon outer");

        Queue<Vertex> checkList = new Queue<Vertex>();
        List<Vertex> outerCells = [];
        // enqueue corners
        checkList.Enqueue(MinMaxVertex.Min);
        checkList.Enqueue(MinMaxVertex.Max);
        // push out to sneak around edge
        checkList.Enqueue(new Vertex(MinMaxVertex.Max.X + 1, MinMaxVertex.Min.Y - 1));
        checkList.Enqueue(new Vertex(MinMaxVertex.Min.X - 1, MinMaxVertex.Max.Y + 1));

        while (checkList.Count > 0)
        {
            Vertex cell = checkList.Dequeue();
            if (!IsCellInLocalRange(cell)) continue;
            if (outerCells.Contains(cell)) continue;
            if (IsCellOnEdge(cell)) continue;

            outerCells.Add(cell);

            checkList.Enqueue(new Vertex(cell.X + 1, cell.Y));
            checkList.Enqueue(new Vertex(cell.X - 1, cell.Y));
            checkList.Enqueue(new Vertex(cell.X, cell.Y + 1));
            checkList.Enqueue(new Vertex(cell.X, cell.Y - 1));
        }

        return outerCells;
    }

    public bool CanContainAABB(AABB rectangle)
    {
        Console.WriteLine($"testing rectangle of size {rectangle.Size}");

        foreach (Vertex vert in rectangle.GetAllEdgeCells())
        {
            if (!IsCellInsidePolygon(vert)) return false;
        }

        return true;
    }
}