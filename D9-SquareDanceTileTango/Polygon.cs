using System.Collections;
using System.Security.Cryptography.X509Certificates;

public class Polygon
{
    public interface RayHit
    {
        Edge HitEdge { get; set; }
        long Distance { get; set; }
    }

    public struct AlignedRayHit : RayHit
    {
        public Edge HitEdge { get; set; }
        public long Distance { get; set; }
    }

    public struct BroadsideRayHit : RayHit
    {
        public Edge HitEdge { get; set; }
        public long Distance { get; set; }
    }


    public enum RayDirection
    {
        RIGHT, DOWN, UP, LEFT
    };

    
    readonly Edge[] AAEdges;

    public Polygon(Vertex[] vertices)
    {
        List<Edge> edges = [];

        for (int i = 0, j = 1; i < vertices.Length; i++, j++)
        {
            if (j == vertices.Length) j = 0; // wrap to start

            edges.Add(new Edge(vertices[i], vertices[j]));
        }

        AAEdges = edges.ToArray();
    }

    IEnumerable<RayHit> RayTest(long X, long Y, long distanceMax, RayDirection direction)
    {
        foreach (Edge edge in AAEdges)
        {
            bool flatRay = direction == RayDirection.RIGHT || direction == RayDirection.LEFT;
            bool positiveRayDir = direction == RayDirection.UP || direction == RayDirection.RIGHT;
            bool flatEdge = edge.A.Y == edge.B.Y;
            bool aligned = (flatRay && flatEdge) || (!flatRay && !flatEdge);

            long edgeMin = flatEdge ? Math.Min(edge.A.X, edge.B.X) : Math.Min(edge.A.Y, edge.B.Y);
            long edgeMax = flatEdge ? Math.Max(edge.A.X, edge.B.X) : Math.Max(edge.A.Y, edge.B.Y);
            
            long perpEdgePosition = flatEdge ? edge.A.Y : edge.A.X;
            long perpRayPosition = flatRay ? Y : X;
            long rayStartPosition = flatRay ? X : Y;

            bool inline = aligned && perpEdgePosition == perpRayPosition;

            if (aligned)
            {
                if (!inline) continue; // miss

                long distance = positiveRayDir ?
                    edgeMin - rayStartPosition : rayStartPosition - edgeMax;

                if (distance <= 0 || distance >= distanceMax) continue;

                yield return new AlignedRayHit
                {
                    Distance = distance,
                    HitEdge = edge
                };
            }
            else
            {
                long distance = positiveRayDir ?
                    perpEdgePosition - rayStartPosition : rayStartPosition - perpEdgePosition;

                if (distance <= 0 || distance >= distanceMax) continue;

                bool rayOnTarget = perpRayPosition >= edgeMin && perpRayPosition <= edgeMax;

                if (!rayOnTarget) continue;
                
                yield return new BroadsideRayHit
                {
                    Distance = distance,
                    HitEdge = edge
                };
            }
        }
    }

    bool IsEdgeWithinBounds(Vertex vertexFrom, Vertex vertexTo)
    {
        bool flat = vertexFrom.Y == vertexTo.Y;
        bool dirIsPositive = flat ? vertexFrom.X <= vertexTo.X : vertexFrom.Y <= vertexTo.Y;
        RayDirection direction = flat ? (
            dirIsPositive ? RayDirection.RIGHT : RayDirection.LEFT
        ) : (
            dirIsPositive ? RayDirection.UP : RayDirection.DOWN
        );
        long distance = flat ? Math.Abs(vertexFrom.X - vertexTo.X) : Math.Abs(vertexFrom.Y - vertexTo.Y);

        IEnumerable<RayHit> hits = RayTest(vertexFrom.X, vertexFrom.Y, distance, direction);

        List<BroadsideRayHit> broadsideRayHits = hits
            .Where(hit => hit is BroadsideRayHit)
            .Select(hit => (BroadsideRayHit)hit)
            .ToList();

        List<AlignedRayHit> alignedRayHits = hits
            .Where(hit => hit is AlignedRayHit)
            .Select(hit => (AlignedRayHit)hit)
            .ToList();

        int i = 0;
        while (i < broadsideRayHits.Count)
        {
            foreach (var alignedHit in alignedRayHits)
            {
                // collision should be ignored if the ray rides the edge
                if (alignedHit.Distance == broadsideRayHits[i].Distance) 
                {
                    broadsideRayHits.RemoveAt(i);
                    i--;
                    break;
                }
            }
            i++;
        }

        return broadsideRayHits.Count == 0;
    }

    public bool CanContainAABB(AABB rectangle)
    {
        for (int i=0, j=1; i<4; i++, j++)
        {
            if (j == 4) j = 0;
            if (!IsEdgeWithinBounds(
                rectangle.Vertices[i],
                rectangle.Vertices[j])
                ) return false;
        }

        return true;
    }
}