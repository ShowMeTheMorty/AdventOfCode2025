

using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

public class SquareSeeker
{

    readonly Vertex[] Coords;

    public SquareSeeker(string data)
    {
        Coords = data
            .Split('\n')
            .Select(str => str.Trim())
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .Select(str =>
            {
                string[] parts = str.Split(',');
                return new Vertex(long.Parse(parts[0]), long.Parse(parts[1]));
            }).ToArray();
    }

    public static SquareSeeker FromFile(string path)
    {
        return new SquareSeeker(File.ReadAllText(path));
    }

    AABB[] GetAllSquares()
    {
        List<AABB> squares = [];
        foreach (Vertex coord in Coords)
        {
            foreach (Vertex other in Coords)
            {
                if (coord == other) continue;
                squares.Add(new AABB(coord, other));
            }
        }

        squares.Sort((a, b) => a.Size.CompareTo(b.Size));

        return squares.ToArray();
    }

    public long FindLargestSquarea(bool anySquarea = true)
    {
        AABB[] squares = GetAllSquares();
        if (anySquarea) return squares[^1].Size;

        // test polygon
        ConvexHullPolygon polygon = new(Coords);

        for (int i = squares.Length - 1; i >= 0; i--)
        {
            if (polygon.CanContainAABB(squares[i]))
            {
                return squares[i].Size;
            }
        }

        throw new Exception("Poodoo");
    }
    
}