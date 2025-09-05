namespace ConsoleApp3;

public class Node
{
    public Node(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public double G { get; set; } // z bierzącego do nstepnego
    public double H { get; set; } // od bierzącego do końcowego
    public double F => G + H;
    public Node? Parent { get; set; }

    public double CalculateHeuristic(Node secondNode)
    {
        var diffPow = Math.Pow(X - secondNode.X, 2);
        var diffPow2 = Math.Pow(Y - secondNode.Y, 2);
        return Math.Sqrt(diffPow + diffPow2);
    }

    public List<Node> ReverseAndReconstructPath()
    {
        var reconstructedPath = new List<Node>();
        var previous = this;
        while (previous != null)
        {
            reconstructedPath.Add(previous);
            previous = previous.Parent;
        }

        reconstructedPath.Reverse();
        return reconstructedPath;
    }

    public bool EqualPosition(Node node)
    {
        return X == node.X && Y == node.Y; 
    }
}