public class CircuitLinker
{
    readonly JunctionBox[] JunctionBoxes;

    public CircuitLinker(string data)
    {
        
    }
    
    public static CircuitLinker FromFile(string path)
    {
        return new CircuitLinker(File.ReadAllText(path));
    }
}