public class Vec3
{
    public long X;
    public long Y;
    public long Z;

    public Vec3(long x, long y, long z)
    {
        X = x; Y = y; Z = z;
    }
    
    public long DistanceSquared(Vec3 other)
    {
        long xDiff = X - other.X;
        long yDiff = Y - other.Y;
        long zDiff = Z - other.Z;

        return xDiff * xDiff + yDiff * yDiff + zDiff * zDiff;
    }
    
    public static long DistanceSquared (Vec3 A, Vec3 B)
    {
        return A.DistanceSquared(B);
    }
}
