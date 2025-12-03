public class BatteryArray (byte[] Joltages)
{
    // what is this function name up to
    byte[] IncreaseArrayByInsert (byte[] currentBest, byte newValue)
    {
        byte[] newBest = new byte[currentBest.Length];
        Array.Copy(currentBest, newBest, currentBest.Length);

        // walk forward, move higher numbers down until we hit a higher number than what we have to offer
        for (int i=0; i<currentBest.Length-1; i++)
        {
            if (currentBest[i] >= currentBest[i + 1])
            {
                newBest[i + 1] = currentBest[i];
            }
            else break;
        }

        // new value is the best
        newBest[0] = newValue;

        return newBest;
    }

    public long GetBestJoltage (int count = 2)
    {
        // initialise with the end items
        byte[] currentBest = Joltages[^count..];

        // walk backwards
        for (int i = Joltages.Length - count - 1; i >= 0; i--)
        {
            // shuffle down, big numbers coming in
            if (Joltages[i] >= currentBest[0])
            {
                currentBest = IncreaseArrayByInsert(currentBest, Joltages[i]);
            }
        }

        return long.Parse(string.Join("", currentBest));
    }

    public static BatteryArray FromString(string batteryArrayString)
    {
        byte[] joltages = batteryArrayString
            .Trim()
            .Where(char.IsDigit)
            .Select(c => (byte)(c - '0'))
            .ToArray();

        return new BatteryArray(joltages);
    }

    public override string ToString()
    {
        return string.Join("", Joltages);
    }
}