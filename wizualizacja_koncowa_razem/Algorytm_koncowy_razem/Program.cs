namespace ConsoleApp3;

internal class Program
{
    private static void Main(string[] args)
    {
        var grid = Helpers.ImportGrid(@"grid.txt");
        var algorithm = new AStarAlgorithm(grid);
        var path = algorithm.FindPath(new Node(0, 0), new Node(19, 19));
        algorithm.CalculatedPath = path;
        algorithm.PrintCalculatedPath();
        algorithm.SaveBoardToFile("output.txt");
        algorithm.SavePathToFile("path.txt");
    }
}