public class MinMax2D
{
    public Vertex Min;
    public Vertex Max;

    public MinMax2D (Vertex min, Vertex max)
    {
        Min = min; Max = max;
    }

    public static MinMax2D FromParallelReduction(Vertex[] input)
    {
        int n = input.Length;
        int pow2 = GetNextPowerOf2(n);

        Vertex[] mins = new Vertex[pow2];
        Vertex[] maxs = new Vertex[pow2];

        for (int i = 0; i < n; i++)
        {
            mins[i] = input[i];
            maxs[i] = input[i];
        }

        for (int i = n; i < pow2; i++)
        {
            mins[i] = new Vertex(int.MaxValue, int.MaxValue);
            maxs[i] = new Vertex(int.MaxValue, int.MaxValue);
        }

        for (int step = 1; step < pow2; step *= 2)
        {
            int stride = step * 2;

            Parallel.For(0, pow2 / stride, i =>
            {
                int a = i * stride;
                int b = a + step;

                mins[a] = Vertex.Min(mins[a], mins[b]);
                maxs[a] = Vertex.Max(maxs[a], maxs[b]);
            });
        }

        return new MinMax2D(mins[0], maxs[0]);
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