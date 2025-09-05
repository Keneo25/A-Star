namespace ConsoleApp3;

public class AStarAlgorithm
{
    private readonly int[,] _board;
    public List<Node> CalculatedPath;

    public AStarAlgorithm(int[,] board)
    {
        _board = board;
        CalculatedPath = new List<Node>();
    }

    public List<Node>? FindPath(Node startNode, Node endNode)
    {
        var openList = new List<Node> { startNode }; 
        var closedList = new List<Node>(); 

        startNode.G = 0;
        startNode.H = startNode.CalculateHeuristic(endNode);
        
        while (openList.Any())
        {
            var current = openList.Where(p => p.F == openList.Min(x => x.F)).LastOrDefault();
            if (current!.EqualPosition(endNode)) 
                return current.ReverseAndReconstructPath();
            openList.Remove(current); t 
            closedList.Add(current); 
            
            foreach (var neighbour in GetNeighbours(current))
            {
                if (closedList.Any(x => x.EqualPosition(neighbour))) 
                    continue;
                var tentativeG = current.G + 1; 
                if (!openList.Any(x => x.EqualPosition(neighbour))) 
                    openList.Add(neighbour);
                else if (tentativeG >= neighbour.G) 
                    continue;
                UpdateNeighbour(neighbour, current, endNode, tentativeG);
            }
        }

        return null;
    }

    private void UpdateNeighbour(Node neighbour, Node current, Node endNode, double tentativeG)
    {
        neighbour.Parent = current; 
        neighbour.G = tentativeG; 
        neighbour.H = neighbour.CalculateHeuristic(endNode); 
    }

    private List<Node> GetNeighbours(Node node)
    {
        var x = node.X;
        var y = node.Y;

        var numberCols = _board.GetLength(0);
        var numberRows = _board.GetLength(1);

        var neighbours = new List<Node>();
        //góra
        if (y > 0 && _board[y - 1, x] != 5)
            neighbours.Add(new Node(x, y - 1));
        //dół
        if (y < numberCols - 1 && _board[y + 1, x] != 5) 
            neighbours.Add(new Node(x, y + 1));
        //lewo
        if (x > 0 && _board[y, x - 1] != 5) 
            neighbours.Add(new Node(x - 1, y));
        //prawo
        if (x < numberRows - 1 && _board[y, x + 1] != 5)
            neighbours.Add(new Node(x + 1, y));

        return neighbours;
    }

    public void PrintCalculatedPath()
    {
        if (CalculatedPath == null || !CalculatedPath.Any())
        {
            Console.WriteLine("\n Nie znaleziono drogi!");
            return;
        }

        foreach (var node in CalculatedPath) _board[node.Y, node.X] = 3;
        Console.WriteLine("\nMapa z wyznaczoną trasą");

        for (var i = 19; i >= 0; i--)
        {
            Console.Write($"{i,2} ");
            for (var j = 0; j < 20; j++)
                if (_board[i, j] == 0) Console.Write("0 ");
                else if (_board[i, j] == 3) Console.Write("3 ");
                else if (_board[i, j] == 5) Console.Write("5 ");
            Console.WriteLine();
        }
    }

    public void SavePathToFile(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

        if (CalculatedPath == null || !CalculatedPath.Any())
        {
            File.WriteAllText(filePath, "");
            Console.WriteLine("Brak obliczonej ścieżki do zapisania.");
            return;
        }

        Console.WriteLine("Zapisuję ścieżkę do pliku...");
        
        var lines = CalculatedPath.Select(node => $"{node.X},{node.Y}").ToArray();
        File.WriteAllLines(filePath, lines);

        Console.WriteLine($"Ścieżka została zapisana do pliku: {filePath}");
        Console.WriteLine($"Zapisano {CalculatedPath.Count} punktów ścieżki.");
    }

    public void SaveBoardToFile(string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        {
            for (var i = 19; i >= 0; i--)
            {
                for (var j = 0; j < 20; j++)
                {
                    writer.Write(_board[i, j]);
                    if (j < 19) writer.Write(" ");
                }

                writer.WriteLine();
            }
        }

        Console.WriteLine($"\nPlansza została zapisana do pliku: {filePath}");
    }
}