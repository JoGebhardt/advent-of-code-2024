namespace AdventOfCode2024.Day8;

public static class Day8
{
    public static int Exercise1(string filePath) => MainLogic(filePath);

    public static int Exercise2(string filePath) => MainLogic(filePath, true);
    
    static int MainLogic(string filePath, bool respectHarmonics = false)
    {
        var data = ReadData(filePath);

        return AllCharsExceptPoints(data)
            .Select(x => FindCoordinatesOf(data, x))
            .Select(x => CalculateAllAntiNodes(data, x, respectHarmonics))
            .Aggregate(new HashSet<(int X, int Y)>(), (acc, x) => { acc.UnionWith(x); return acc; })
            .Count;
    }

    static HashSet<char> AllCharsExceptPoints(char[,] data)
    {
        var set = new HashSet<char>();

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (data[i, j] != '.')
                    set.Add(data[i, j]);
            }
        }

        return set;
    }

    static HashSet<(int X, int Y)> FindCoordinatesOf(char[,] data, char c)
    {
        var coordinates = new HashSet<(int X, int Y)>();

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (data[i, j] == c)
                    coordinates.Add((i, j));
            }
        }

        return coordinates;
    }

    static HashSet<(int X, int Y)> CalculateAllAntiNodes(char[,] data, HashSet<(int X, int Y)> coords, bool respectHarmonics = false)
    {
        var antiNodes = new HashSet<(int X, int Y)>();

        foreach (var c1 in coords)
        {
            foreach (var c2 in coords)
            {
                if (respectHarmonics)
                {
                    antiNodes.Add(c1);
                    antiNodes.Add(c2);
                }

                if (c1 == c2)
                    continue;

                var (X, Y) = (c2.X - c1.X, c2.Y - c1.Y);
                var (X2, Y2) = (X, Y);
                
                while (true)
                {
                    var antinode1 = (c1.X - X2, c1.Y - Y2);
                    var antinode2 = (c2.X + X2, c2.Y + Y2);

                    if (IsInBounds(data, antinode1))
                        antiNodes.Add(antinode1);

                    if (IsInBounds(data, antinode2))
                        antiNodes.Add(antinode2);
                    
                    if (!IsInBounds(data, antinode1) && !IsInBounds(data, antinode2))
                        break;
                    
                    if (!respectHarmonics)
                        break;

                    (X2, Y2) = (X2 + X, Y2 + Y);
                }
            }
        }

        return antiNodes;
    }

    static bool IsInBounds(char[,] data, (int X, int Y) coords)
        => coords.X >= 0 && coords.X < data.GetLength(0) && coords.Y >= 0 && coords.Y < data.GetLength(1);

    private static char[,] ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var chars = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                chars[i, j] = lines[i][j];
            }
        }

        return chars;
    }
}