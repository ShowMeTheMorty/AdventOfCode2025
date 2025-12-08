
using System.Numerics;

public class MinMax3D
{
    public Vector3 Min;
    public Vector3 Max;

    public MinMax3D (Vector3 min, Vector3 max)
    {
        Min = min; Max = max;
    }

    public static MinMax3D FromParallelReduction(Vector3[] input)
    {
        int n = input.Length;
        int pow2 = GetNextPowerOf2(n);

        Vector3[] mins = new Vector3[pow2];
        Vector3[] maxs = new Vector3[pow2];

        for (int i = 0; i < n; i++)
        {
            mins[i] = input[i];
            maxs[i] = input[i];
        }

        for (int i = n; i < pow2; i++)
        {
            mins[i] = new Vector3(float.MaxValue);
            maxs[i] = new Vector3(float.MinValue);
        }

        for (int step = 1; step < pow2; step *= 2)
        {
            int stride = step * 2;

            Parallel.For(0, pow2 / stride, i =>
            {
                int a = i * stride;
                int b = a + step;

                mins[a] = Vector3.Min(mins[a], mins[b]);
                maxs[a] = Vector3.Max(maxs[a], maxs[b]);
            });
        }

        return new MinMax3D(mins[0], maxs[0]);
    }

    private static int GetNextPowerOf2(int v)
    {
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;
        return v;
    }
}