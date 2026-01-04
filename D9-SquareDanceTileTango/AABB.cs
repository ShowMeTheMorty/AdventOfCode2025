public record AABB
{
    public readonly Edge[] AAEdges;
    public readonly long Size;

    public AABB(Vertex cornerA, Vertex cornerB)
    {
        int minX = Math.Min(cornerA.X, cornerB.X);
        int maxX = Math.Max(cornerA.X, cornerB.X);
        int minY = Math.Min(cornerA.Y, cornerB.Y);
        int maxY = Math.Max(cornerA.Y, cornerB.Y);

        Vertex[] verts =
        [
            new Vertex(minX, maxY), // TL
            new Vertex(maxX, maxY), // TR
            new Vertex(maxX, minY), // BR
            new Vertex(minX, minY), // BL
        ];

        AAEdges =
        [
            new Edge(verts[0], verts[1]),
            new Edge(verts[1], verts[2]),
            new Edge(verts[2], verts[3]),
            new Edge(verts[3], verts[0])
        ];

        long X = maxX - minX + 1;
        long Y = maxY - minY + 1;

        Size = X * Y;
    }

    public IEnumerable<Vertex> GetAllEdgeCells()
    {
        HashSet<Vertex> cells = [];
        foreach (Edge edge in AAEdges)
        {
            int edgeMin = edge.IsFlat ? Math.Min(edge.A.X, edge.B.X) : Math.Min(edge.A.Y, edge.B.Y);
            int edgeMax = edge.IsFlat ? Math.Max(edge.A.X, edge.B.X) : Math.Max(edge.A.Y, edge.B.Y);
            int edgePos = edge.IsFlat ? edge.A.Y : edge.A.X;

            for (int n = edgeMin; n <= edgeMax; n++)
            {
                cells.Add(edge.IsFlat ? new Vertex(n, edgePos) : new Vertex(edgePos, n));
            }
        }
        return cells;
    }
}
