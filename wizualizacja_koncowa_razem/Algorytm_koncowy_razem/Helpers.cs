namespace ConsoleApp3;

public static class Helpers
{
    public static int[,]? ImportGrid(string filePath)
    {
        var lines = File.ReadAllLines(filePath);

        if (lines.Length == 0) return null;

        var rows = lines.Select(p => p.SanitizeAndSplit()).ToArray();

        if (rows.Any(p => p.Length != rows.First().Length)) return null;

        var numberRows = lines.Length;
        var numberCols = rows[0].Length;

        var parsedGrid = new int[numberRows, numberCols];


        for (var i = 0; i < rows.Length; i++)
        for (var j = 0; j < rows[i].Length; j++)
        {
            var parsed = int.Parse(rows[i][j]);
            parsedGrid[numberRows - 1 - i, j] = parsed;
        }

        return parsedGrid;
    }

    private static string[] SanitizeAndSplit(this string str, char separator = ' ')
    {
        return str.Trim().Split(separator);
    }
}