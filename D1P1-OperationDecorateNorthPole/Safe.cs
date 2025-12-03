

public class Safe (int Position=50, bool CountZeroPasses=false)
{
    private const int MinPosition = 0;
    private const int MaxPosition = 99;

    public int ZeroStateAccumulator { get; private set; } = 0;

    public void LogZeroStateAccumulator ()
    {
        Console.WriteLine($"The safe has been at position 0 a total of {ZeroStateAccumulator} times.");
    }

    public void Rotate (int clicks)
    {
        bool isStartingAtZero = Position == 0;
        int dialRange = MaxPosition - MinPosition + 1;
        int targetPosition = Position + clicks;

        int underClicks = -Math.Min(0, targetPosition - MinPosition); 
        int overClicks = Math.Max(0, targetPosition - MaxPosition);

        int underClickRanges = underClicks > 0 ? ((underClicks-1) / dialRange + 1) : 0;
        int overClickRanges = overClicks > 0 ? ((overClicks-1) / dialRange + 1) : 0;

        int underCorrectionClicks = underClickRanges * dialRange;
        int overCorrectionClicks = overClickRanges * dialRange;

        Position = targetPosition + underCorrectionClicks - overCorrectionClicks;

        Console.WriteLine($"The safe rotated {(clicks > 0 ? "right" : "left")} by {Math.Abs(clicks)} clicks to position {Position}");

        if (CountZeroPasses) // if we need to count passing zero
        {
            ZeroStateAccumulator += underClickRanges + overClickRanges;
            
            bool isCountingBackwardsFromZero = isStartingAtZero && clicks < 0;
            bool isCountingForwardsToZero = Position == 0 && clicks > 0;
            if (isCountingBackwardsFromZero || isCountingForwardsToZero)
            {
                ZeroStateAccumulator--;
            }
        }

        if (Position == 0)
        {
            ZeroStateAccumulator++;
        }
    }
}