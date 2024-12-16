using System.ComponentModel;
using MoreLinq;
using static AdventOfCode2024.Helper.Helper;

namespace AdventOfCode2024.Day12;

public static class Day12
{
    const char Fill = '.';
    private static readonly (int, int)[] Directions = [(1, 0), (0, 1), (0, -1), (-1, 0)];

    public static long Exercise1(string filePath)
    {
        var data = ReadData(filePath);
        var visited = new HashSet<(int, int)>();

        return data.Coordinates()
            .Select(coords => GetRegion(data, coords.Item1, coords.Item2, visited))
            .Where(region => region != null)
            .Select(region => region!)
            .Select(region => region.Perimeter() * region.Area())
            .Sum();
    }

    public static long Exercise2(string filePath)
    {
        var data = ReadData(filePath);
        var visited = new HashSet<(int, int)>();

        return data.Coordinates()
            .Select(coords => GetRegion(data, coords.Item1, coords.Item2, visited))
            .Where(region => region != null)
            .Select(region => region!)
            .Select(region => region.Sides() * region.Area())
            .Sum();
    }

    private static char[,]? GetRegion(char[,] data, int x, int y, HashSet<(int, int)> visited)
    {
        if (visited.Contains((x, y)))
            return null;

        var region = EmptyRegion(data);
        var regionType = data[x, y];
        var queue = new Queue<(int, int)>();
        queue.Enqueue((x, y));

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            region.Set(currentPos, regionType);

            foreach (var dir in Directions)
            {
                var newCoords = currentPos.Add(dir);

                if (data.IsInBounds(newCoords) && data.Get(newCoords) == regionType && !visited.Contains(newCoords))
                {
                    queue.Enqueue(newCoords);
                    visited.Add(newCoords);
                }
            }
        }

        return region;
    }

    private static int Perimeter(this char[,] data)
        => data.Coordinates()
            .Where(coords => data.Get(coords) != Fill)
            .SelectMany(coords => Directions.Select(dir => coords.Add(dir)))
            .Count(coords => !data.IsInBounds(coords) || data.Get(coords) == Fill);

    private static int Sides(this char[,] data) 
    {
        var border = data.Coordinates()
            .Where(coords => data.Get(coords) != Fill)
            .Where(coords => Directions.Any(dir => !data.IsInBounds(coords.Add(dir)) || data.Get(coords.Add(dir)) == Fill))
            .ToHashSet();
        
        return Directions.Sum(dir => {
            var visited = new HashSet<(int, int)>();
            return border.Select(borderElement => GroupSides(data, border, borderElement, visited, dir)).Count(x => x.Count > 0);
        });
    }
    
    private static List<(int, int)> GroupSides(char[,] data, HashSet<(int, int)> border, (int, int) borderElement, HashSet<(int, int)> visited, (int, int) dir)
    {
        if (visited.Contains(borderElement) || !border.Contains(borderElement) || !data.IsInBounds(borderElement))
            return [];
        
        if (data.IsInBounds(borderElement.Add(dir)) && data.Get(borderElement.Add(dir)) != Fill)
            return [];
        
        visited.Add(borderElement);
        
        var dirForNextElement = (dir.Item1 == 0) ? (1, 0) : (0, 1);

        return [
            borderElement, 
            .. GroupSides(data, border, borderElement.Add(dirForNextElement), visited, dir),
            .. GroupSides(data, border, borderElement.Add(dirForNextElement.Multiply(-1)), visited, dir)
        ];
    }
    
    private static int Area(this char[,] data) => data.Coordinates().Count(coords => data.Get(coords) != Fill);

    private static char[,] EmptyRegion(char[,] data)
    {
        var region = new char[data.GetLength(0), data.GetLength(1)];

        for (var i = 0; i < data.GetLength(0); i++)
        {
            for (var j = 0; j < data.GetLength(1); j++)
            {
                region[i, j] = Fill;
            }
        }

        return region;
    }

    private static char[,] ReadData(string filePath)
    {
        return File.ReadAllLines(filePath).Select(x => x.ToCharArray()).ToArray().ToJagged();
    }
}