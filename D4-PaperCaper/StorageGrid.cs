
using System.Collections.Concurrent;

public class StorageGrid
{
    public struct Kernel(int Ranks, int Files)
    {
        bool IsCoordInRange(int rank, int file, int maxRanks, int maxFiles)
        {
            if (rank < 0 || file < 0) return false;
            if (rank >= maxRanks || file >= maxFiles) return false;
            return true;
        }
        
        public IEnumerable<(int, int)> GetValidNeighbourCoords (int fromRank, int fromFile, int maxRanks, int maxFiles)
        {
            List<(int, int)> neighbourCoords = new List<(int, int)>();

            int midRank = Ranks / 2;
            int midFile = Files / 2;

            for (int r = -midRank; r < Ranks - midRank; r++)
            {
                for (int f = -midFile; f < Files - midFile; f++)
                {
                    // not a neighbour
                    if (f == 0 && r == 0) continue;

                    (int, int) coord = (r + fromRank, f + fromFile);

                    // invalid coord
                    if (!IsCoordInRange(coord.Item1, coord.Item2, maxRanks, maxFiles)) continue;

                    neighbourCoords.Add(coord);
                }
            }

            return neighbourCoords;
        }
    }
    
    readonly bool[,] FillData;

    public StorageGrid(string data)
    {
        string[] rows = data
            .Split('\n')
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .Select(str => str.Trim())
            .ToArray();

        FillData = new bool[rows.Length, rows[0].Length];
        
        for (int rank=0; rank<FillData.GetLength(0); rank++)
        {
            for (int file=0; file<FillData.GetLength(1); file++)
            {
                bool isFilled = rows[rank][file] == '@' ? true : false;
                FillData[rank, file] = isFilled;
            }
        }
    }

    public static StorageGrid FromFile(string path)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        using (StreamReader streamReader = new StreamReader(fileStream))
        {
            return new StorageGrid(streamReader.ReadToEnd());
        }
    }

    public int CountStoredItems ()
    {
        int storedItemCount = 0;

        for (int rank = 0; rank < FillData.GetLength(0); rank++)
        {
            for (int file = 0; file < FillData.GetLength(0); file++)
            {
                if (FillData[rank, file]) storedItemCount++;
            }
        }

        return storedItemCount;
    }

    public IEnumerable<(int, int)> GetAccessibleStoredItemCoords(
        Kernel kernel,
        Func<int, bool> isAccessibleFromFilledNeighbours
    )
    {
        int ranks = FillData.GetLength(0);
        int files = FillData.GetLength(1);
        int totalCells = ranks * files;

        ConcurrentBag<(int, int)> accessibleItems = [];

        Parallel.For(0, totalCells, cellIndex =>
        {
            int rank = cellIndex / files;
            int file = cellIndex % files;

            if (!FillData[rank, file]) return; // no item to access

            IEnumerable<(int, int)> neighbourCoords = kernel.GetValidNeighbourCoords(rank, file, ranks, files);

            int filledNeighbourCount = 0;
            foreach (var coord in neighbourCoords)
            {
                if (FillData[coord.Item1, coord.Item2]) filledNeighbourCount++;
            }

            if (isAccessibleFromFilledNeighbours(filledNeighbourCount)) accessibleItems.Add((rank, file));
        });

        return accessibleItems;
    }
    
    // inefficient but oh well
    public void DrawStorageGridWithTargetedItems (IEnumerable<(int, int)> targetCoords)
    {
        for (int rank=0; rank<FillData.GetLength(0); rank++)
        {
            for (int file=0; file<FillData.GetLength(1); file++)
            {
                char charmander = '.';
                if (FillData[rank, file]) charmander = '@';

                foreach (var coord in targetCoords)
                {
                    if (coord.Item1 == rank && coord.Item2 == file) charmander = 'x';
                }

                Console.Write(charmander);
            }
            Console.Write('\n');
        }
    }

    internal void RemoveItemsAtCoords(IEnumerable<(int, int)> accessibleItemCoords)
    {
        int removeCount = 0;
        foreach (var coord in accessibleItemCoords)
        {
            if (FillData[coord.Item1, coord.Item2]) removeCount++;
            FillData[coord.Item1, coord.Item2] = false;
        }

        Console.WriteLine($"Removed {removeCount} stored items");
    }
}