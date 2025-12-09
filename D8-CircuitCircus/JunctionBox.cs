
public class JunctionBox
{
    public KDTree3D.Node Node;
    public List<JunctionBox> ConnectedBoxes;

    public JunctionBox(KDTree3D.Node node)
    {
        Node = node;
    }
    
    public void ConnectJunctionBox(JunctionBox otherJunctionBox)
    {
        ConnectedBoxes.Add(otherJunctionBox);
        otherJunctionBox.ConnectedBoxes.Add(this);
    }
}