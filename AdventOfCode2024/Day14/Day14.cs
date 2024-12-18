using System.Security.Cryptography.X509Certificates;
using AdventOfCode2024.Helper;
using MoreLinq;
using Tuple = (long, long);

namespace AdventOfCode2024.Day14;

public static class Day14
{
    private const bool Test = false;
    private readonly static (long, long) Frame = Test ? (11, 7) : (101, 103);
    
    public static long Exercise1(string filePath)
    {
        var data = ReadData(filePath);

        return data
            .Select(t => CalculatePosition(t.Item1, t.Item2))
            .Where(t => t.Item1 != Frame.Item1 / 2 && t.Item2 != Frame.Item2 / 2)
            .GroupBy(x => (x.Item1 >= Frame.Item1 / 2, x.Item2 >= Frame.Item2 / 2))
            .Select(x => x.Count())
            .Aggregate((a, b) => a * b);
    }

    public static long Exercise2(string filePath)
    {
        var data = ReadData(filePath);
        
        var (Second, Tree) = Enumerable.Range(0, 10_000)
            .Select(x => (
                Second: x,
                Tree: data.Select(t => CalculatePosition(t.Item1, t.Item2, x)).Distinct().ToList())
            )
            .OrderBy(x => GroupsCount(x.Tree))
            .First();
        
        // PrintPositions(Tree);
        
        return Second;
    }
    
    private static long GroupsCount(List<Tuple> positions)
    {
        var visited = new HashSet<Tuple>(); 
        var groups = 0;

        for (var i = 0; i < Frame.Item2; i++)
        {
            for (var j = 0; j < Frame.Item1; j++)
            {
                var pos = (j, i);
                if (visited.Contains(pos) || !positions.Contains(pos))
                    continue;

                groups++;
                var queue = new Queue<Tuple>();
                queue.Enqueue(pos);

                while (queue.Any())
                {
                    var current = queue.Dequeue();
                    if (visited.Contains(current))
                        continue;

                    visited.Add(current);
                    var neighbours = new List<Tuple>
                    {
                        (current.Item1 + 1, current.Item2),
                        (current.Item1 - 1, current.Item2),
                        (current.Item1, current.Item2 + 1),
                        (current.Item1, current.Item2 - 1)
                    };

                    foreach (var neighbour in neighbours)
                    {
                        if (neighbour.Item1 < 0 || neighbour.Item1 >= Frame.Item1 || neighbour.Item2 < 0 || neighbour.Item2 >= Frame.Item2)
                            continue;

                        if (positions.Contains(neighbour))
                            queue.Enqueue(neighbour);
                    }
                }
            }
        }
        
        return groups;
    }

    private static void PrintPositions(List<(long, long)> positions)
    {
        for (var i = 0; i < Frame.Item2; i++)
        {
            for (var j = 0; j < Frame.Item1; j++)
            {
                if (positions.Any(x => x.Item1 == j && x.Item2 == i))
                    Console.Write("#");
                else
                    Console.Write(".");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    private static Tuple CalculatePosition(Tuple p, Tuple v, int seconds = 100)
    {
        var pos = p.Add(v.Multiply(seconds));
        pos = (pos.Item1 % Frame.Item1, pos.Item2 % Frame.Item2);
        pos = (pos.Item1 < 0 ? pos.Item1 + Frame.Item1 : pos.Item1, pos.Item2 < 0 ? pos.Item2 + Frame.Item2 : pos.Item2);
        return pos;
    }
    
    private static List<(Tuple, Tuple)> ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath).ToList();
        var data = new List<(Tuple, Tuple)>();

        foreach (var line in lines)
        {
            var nums = line.Replace("p=", "").Replace("v=", ",").Replace(" ", "").Split(",").Select(long.Parse).ToArray();
            
            var p = (nums[0], nums[1]);
            var v = (nums[2], nums[3]);
            
            data.Add((p, v));
        }
        
        return data;
    }
}
