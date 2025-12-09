using System.Numerics;

public static class QuickSelect3D
{
    static readonly Random random = new();

    public static int GetMedianIndex(Vector3[] data, int axis)
    {
        int k = data.Length / 2;
        return QuickSelect(data, axis, 0, data.Length - 1, k);
    }

    private static int QuickSelect(Vector3[] data, int axis, int left, int right, int k)
    {
        if (left == right)
            return left;

        int pivotIndex = random.Next(left, right + 1);
        pivotIndex = Partition(data, axis, left, right, pivotIndex);

        if (k == pivotIndex)
            return k;
        else if (k < pivotIndex)
            return QuickSelect(data, axis, left, pivotIndex - 1, k);
        else
            return QuickSelect(data, axis, pivotIndex + 1, right, k);
    }

    private static int Partition(Vector3[] data, int axis, int left, int right, int pivotIndex)
    {
        float pivotValue = data[pivotIndex][axis];
        Swap(data, pivotIndex, right); 
        int storeIndex = left;

        for (int i = left; i < right; i++)
        {
            if (data[i][axis] < pivotValue)
            {
                Swap(data, storeIndex, i);
                storeIndex++;
            }
        }

        Swap(data, right, storeIndex);
        return storeIndex;
    }

    private static void Swap(Vector3[] data, int i, int j)
    {
        Vector3 temp = data[i];
        data[i] = data[j];
        data[j] = temp;
    }
}