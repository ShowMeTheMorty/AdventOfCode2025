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

    int RayTest(long X, long Y, RayDirection direction)
    {
        int hits = 0;
        foreach (Edge edge in AAEdges)
        {
            // bool flat = edge.A.Y == edge.B.Y;
            // if (flat) continue; // ignore aligned edges

            // long min = Math.Min(edge.A.Y, edge.B.Y);
            // long max = Math.Max(edge.A.Y, edge.B.Y);
            // if (Y >= min && Y <= max && X <= edge.A.X) hits++;
            
            if (direction == RayDirection.UP)
            {
                bool flat = edge.A.X == edge.B.X;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.X, edge.B.X);
                long max = Math.Max(edge.A.X, edge.B.X);
                if (X >= min && X <= max && Y <= edge.A.Y) hits++;
            }
            if (direction == RayDirection.DOWN)
            {
                bool flat = edge.A.X == edge.B.X;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.X, edge.B.X);
                long max = Math.Max(edge.A.X, edge.B.X);
                if (X >= min && X <= max && Y >= edge.A.Y) hits++;
            }
            if (direction == RayDirection.RIGHT)
            {
                bool flat = edge.A.Y == edge.B.Y;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.Y, edge.B.Y);
                long max = Math.Max(edge.A.Y, edge.B.Y);
                if (Y >= min && Y <= max && X <= edge.A.X) hits++;
            }
            if (direction == RayDirection.LEFT)
            {
                bool flat = edge.A.Y == edge.B.Y;
                if (flat) continue; // ignore aligned edges

                long min = Math.Min(edge.A.Y, edge.B.Y);
                long max = Math.Max(edge.A.Y, edge.B.Y);
                if (Y >= min && Y <= max && X >= edge.A.X) hits++;
            }
        }
        return hits;
    }

    bool IsVertexInside(Vertex vertex, RayDirection direction)
    {
        int hits = RayTest(vertex.X, vertex.Y, direction);
        if (hits != 1) return false;
        return true;
    }

    public bool CanContainAABB(AABB rectangle)
    {
        bool upNotRight = false;
        for (int i=0; i<4; i++)
        {
            upNotRight = !upNotRight;
            if (!IsVertexInside(rectangle.Vertices[i], RayDirection.RIGHT)) return false;
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