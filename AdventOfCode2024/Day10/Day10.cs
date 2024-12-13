using MoreLinq;

namespace AdventOfCode2024.Day10;

public static class Day10
{
    public static int Exercise1(string filePath)
    {
        var data = ReadData(filePath);
        return CoordsOfZero(data)
            .Select(coords => DFS1(data, coords, 0, []))
            .Sum();
    }
    
    public static int Exercise2(string filePath)
    {
        var data = ReadData(filePath);
        return CoordsOfZero(data)
            .Select(coords => DFS2(data, coords, 0))
            .Sum();
    }
    
    private static int DFS1(int[,] data, (int x, int y) pos, int stepHeight, HashSet<(int, int)> visitedNines)
    {
        if (!IsInBounds(data, pos))
            return 0;
        
        if (stepHeight == 9)
        {
            return visitedNines.Add(pos) ? 1 : 0;
        }
        
        (int x, int y)[] dirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];
        
        return dirs
            .Where(dir => IsInBounds(data, (pos.x + dir.x, pos.y + dir.y)))
            .Where(dir => data[pos.x + dir.x, pos.y + dir.y] == stepHeight + 1)
            .Select(dir => DFS1(data, (pos.x + dir.x, pos.y + dir.y), stepHeight + 1, visitedNines))
            .Sum();
    }

    private static int DFS2(int[,] data, (int x, int y) pos, int stepHeight)
    {
        if (!IsInBounds(data, pos))
            return 0;
        
        if (stepHeight == 9)
            return 1;
        
        (int x, int y)[] dirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];
        
        return dirs
            .Where(dir => IsInBounds(data, (pos.x + dir.x, pos.y + dir.y)))
            .Where(dir => data[pos.x + dir.x, pos.y + dir.y] == stepHeight + 1)
            .Select(dir => DFS2(data, (pos.x + dir.x, pos.y + dir.y), stepHeight + 1))
            .Sum();
    }
    
    private static List<(int, int)> CoordsOfZero(int[,] data)
    {
        var result = new List<(int, int)>();
        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                if (data[i, j] == 0)
                    result.Add((i, j));
            }
        }
        return result;
    }
    
    private static bool IsInBounds(int[,] data, (int x, int y) pos)
    {
        return pos.x >= 0 && pos.x < data.GetLength(0) && pos.y >= 0 && pos.y < data.GetLength(1);
    }
    
    private static int[,] ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var result = new int [lines.Length, lines[0].Length];
        
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                result[i, j] = int.Parse(lines[i][j].ToString());
            }
        }
        
        return result;
    } 
}