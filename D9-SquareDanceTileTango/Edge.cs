public record Edge
{
    public readonly Vertex A;
    public readonly Vertex B;
    public bool IsFlat;

    public Edge(Vertex a, Vertex b)
    {
        A = a; B = b;
        IsFlat = a.Y == b.Y;
    }
}