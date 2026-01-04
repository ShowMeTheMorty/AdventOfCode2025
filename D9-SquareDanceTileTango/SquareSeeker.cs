

using System.Collections.Concurrent;
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
                return new Vertex(int.Parse(parts[0]), int.Parse(parts[1]));
            }).ToArray();
    }

    public static SquareSeeker FromFile(string path)
    {
        return new SquareSeeker(File.ReadAllText(path));
    }

    AABB[] GetAllSquares()
    {
        HashSet<AABB> squares = [];
        foreach (Vertex coord in Coords)
        {
            foreach (Vertex other in Coords)
            {
                if (coord == other) continue;
                squares.Add(new AABB(coord, other));
            }
        }

        List<AABB> squareList = squares.ToList();
        squareList.Sort((a, b) => a.Size.CompareTo(b.Size));

        return squareList.ToArray();
    }

    public long FindLargestSquarea(bool anySquarea = true)
    {
        AABB[] squares = GetAllSquares();
        if (anySquarea) return squares[^1].Size;

        // test polygon
        Polygon polygon = new(Coords);

        for (int i = squares.Length - 1; i >= 0; i--)
        {
            Console.Write($"testing {i} of {squares.Length}\t");
            if (polygon.CanContainAABB(squares[i]))
            {
                return squares[i].Size;
            }
        }

        throw new Exception("dum");
    }
    
}