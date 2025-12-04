// https://adventofcode.com/2025/day/4

using System.Net.Security;

public static class Program
{
    static void PerformTests()
    {
        string exampleData = @"..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.";

        StorageGrid grid = new StorageGrid(exampleData);


        IEnumerable<(int, int)> accessibleItemCoords = grid.GetAccessibleStoredItemCoords(
            new StorageGrid.Kernel(3, 3),
            (int filledNeighbours) => filledNeighbours < 4
        );
        
        grid.DrawStorageGridWithTargetedItems(accessibleItemCoords);

        Console.WriteLine($"There are {accessibleItemCoords.Count()} accessible items");
    }

    static void PerformPuzzleOne()
    {
        StorageGrid grid = StorageGrid.FromFile(@".\input.txt");

        IEnumerable<(int, int)> accessibleItemCoords = grid.GetAccessibleStoredItemCoords(
            new StorageGrid.Kernel(3, 3),
            (int filledNeighbours) => filledNeighbours < 4
        );
        
        grid.DrawStorageGridWithTargetedItems(accessibleItemCoords);

        Console.WriteLine($"There are {accessibleItemCoords.Count()} accessible items");
    }
    
    static void PerformPuzzleTwo()
    {
        StorageGrid grid = StorageGrid.FromFile(@".\input.txt");

        int initialItemCount = grid.CountStoredItems();
        int currentlyAccessibleItems = 1; // has to be something

        IEnumerable<(int, int)> accessibleItemCoords = grid.GetAccessibleStoredItemCoords(
            new StorageGrid.Kernel(3, 3),
            (int filledNeighbours) => filledNeighbours < 4
        );

        int removalPasses = 0;
        while (currentlyAccessibleItems > 0)
        {
            grid.RemoveItemsAtCoords(accessibleItemCoords);

            accessibleItemCoords = grid.GetAccessibleStoredItemCoords(
                new StorageGrid.Kernel(3, 3),
                (int filledNeighbours) => filledNeighbours < 4
            );

            currentlyAccessibleItems = accessibleItemCoords.Count();
            removalPasses++;
        }

        int remainingStoredItemCount = grid.CountStoredItems();
        int totalItemsRemoved = initialItemCount - remainingStoredItemCount;

        Console.WriteLine($"{totalItemsRemoved} were removed in {removalPasses} passes from an inital count of {initialItemCount}, leaving {remainingStoredItemCount} items remaining");
    }

    public static void Main(string[] args)
    {
        // PerformTests();
        // PerformPuzzleOne();
        PerformPuzzleTwo();
    }
}