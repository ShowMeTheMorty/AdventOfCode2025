using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class ConvexHullPolygon
{
    public enum RayDirection
    {
        RIGHT, DOWN, UP, LEFT
    };
    
    readonly Edge[] AAEdges;

    public ConvexHullPolygon(Vertex[] vertices)
    {
        List<Edge> edges = [];

        for (int i = 0, j = 1; i < vertices.Length; i++, j++)
        {
            if (j == vertices.Length) j = 0; // wrap to start

            edges.Add(new Edge(vertices[i], vertices[j]));
        }

        AAEdges = edges.ToArray();
    }

    int RayTest(long X, long Y, long distance, RayDirection direction)
    {
        int hits = 0;
        foreach (Edge edge in AAEdges)
        {
            bool hitsAreEven = hits % 2 == 0;

            if (direction == RayDirection.UP)
            {
                bool flat = edge.A.X == edge.B.X;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.X, edge.B.X);
                long max = Math.Max(edge.A.X, edge.B.X);
                if (hitsAreEven && X > min && X < max && Y <= edge.A.Y && edge.A.Y - Y < distance) hits++;
                if (!hitsAreEven && X >= min && X <= max && Y <= edge.A.Y && edge.A.Y - Y < distance) hits++;
            }
            if (direction == RayDirection.DOWN)
            {
                bool flat = edge.A.X == edge.B.X;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.X, edge.B.X);
                long max = Math.Max(edge.A.X, edge.B.X);
                if (hitsAreEven && X > min && X < max && Y >= edge.A.Y && Y - edge.A.Y < distance) hits++;
                if (!hitsAreEven && X >= min && X <= max && Y >= edge.A.Y && Y - edge.A.Y < distance) hits++;
            }
            if (direction == RayDirection.RIGHT)
            {
                bool flat = edge.A.Y == edge.B.Y;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.Y, edge.B.Y);
                long max = Math.Max(edge.A.Y, edge.B.Y);
                if (hitsAreEven && Y > min && Y < max && X <= edge.A.X && edge.A.X - X < distance) hits++;
                if (!hitsAreEven && Y >= min && Y <= max && X <= edge.A.X && edge.A.X - X < distance) hits++;
            }
            if (direction == RayDirection.LEFT)
            {
                bool flat = edge.A.Y == edge.B.Y;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.Y, edge.B.Y);
                long max = Math.Max(edge.A.Y, edge.B.Y);
                if (hitsAreEven && Y > min && Y < max && X >= edge.A.X && X - edge.A.X < distance) hits++;
                if (!hitsAreEven && Y >= min && Y <= max && X >= edge.A.X && X - edge.A.X < distance) hits++;
            }
        }
        return hits;
    }

    bool IsVertexInside(Vertex vertexFrom, Vertex vertexTo)
    {
        bool flat = vertexFrom.Y == vertexTo.Y;
        bool dirIsPositive = flat ? vertexFrom.X <= vertexTo.X : vertexFrom.Y <= vertexTo.Y;
        RayDirection direction = flat ? (
            dirIsPositive ? RayDirection.RIGHT : RayDirection.LEFT
        ) : (
            dirIsPositive ? RayDirection.UP : RayDirection.DOWN
        );
        long distance = flat ? Math.Abs(vertexFrom.X - vertexTo.X) : Math.Abs(vertexFrom.Y - vertexTo.Y);

        int hits = RayTest(vertexFrom.X, vertexFrom.Y, distance, direction);
        if (hits != 1) return false;
        return true;
    }

    public bool CanContainAABB(AABB rectangle)
    {
        for (int i=0, j=1; i<4; i++, j++)
        {
            if (j == 4) j = 0;
            if (!IsVertexInside(rectangle.Vertices[i], rectangle.Vertices[j])) return false;
        }

        return true;
    }

    // public DrawPolygonWithSquare (AABB square)
    // {
    //     Vertex[] allCoords = AAEdges.SelectMany(edge => new Vertex[] { edge.A, edge.B }).ToArray();
    //     long maxX = allCoords.Max(c => c.X);
    //     long maxY = allCoords.Max(c => c.Y);

    //     string[] output = new string[maxY];
    //     for (int y = 0; y <= maxY; y++) "".PadRight((int)maxX, '.');
        
    //     foreach (Edge edge in AAEdges)
    //     {
    //         bool flat = edge.A.Y == edge.B.Y;
    //         if (flat)
    //         {
    //             int start = (int)Math.Min(edge.A.X, edge.B.X);
    //             int end = (int)Math.Max(edge.A.X, edge.B.X);
    //             for (int c=start; c<=end; c++)
    //             {
    //                 output[edge.A.Y] = "#";
    //             }
    //         }
    //     }
    // }
}