public record Vertex
{
    public readonly int X;
    public readonly int Y;

    public Vertex(int x, int y)
    {
        X = x; Y = y;
    }

    public static Vertex Min(Vertex a, Vertex b)
    {
        return new Vertex(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
    }
    
    public static Vertex Max (Vertex a, Vertex b)
    {
        return new Vertex(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
    }
}