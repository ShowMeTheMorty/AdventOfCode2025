public class AABB
{
    public readonly Vertex[] Vertices;
    public readonly long Size;

    public AABB(Vertex cornerA, Vertex cornerB)
    {
        long minX = Math.Min(cornerA.X, cornerB.X);
        long maxX = Math.Max(cornerA.X, cornerB.X);
        long minY = Math.Min(cornerA.Y, cornerB.Y);
        long maxY = Math.Max(cornerA.Y, cornerB.Y);

        Vertices =
        [
            new Vertex(minX, maxY), // TL
            new Vertex(maxX, maxY), // TR
            new Vertex(minX, minY), // BL
            new Vertex(maxX, minY)  // BR
        ];

        long X = maxX - minX + 1;
        long Y = maxY - minY + 1;

        Size = X * Y;
    }
}
