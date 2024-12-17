using System.Collections.Concurrent;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using AdventOfCode2024.Helper;
using Point = (long, long);

namespace AdventOfCode2024.Day13;

public static partial class Day13
{
    public static long Exercise1(string filePath)
    {
        var data = ReadData(filePath);
        return data.Select(CalculateMinimalTokensBruteForce).Sum();
    }

    public static long Exercise2(string filePath)
    {
        var data = ReadData(filePath).Select(t => (t.A, t.B, t.Prize.Add((10000000000000, 10000000000000))));
        return data.Select(CalculateMinimalTokensSmart).Sum();
    }

    private static long CalculateMinimalTokensBruteForce((Point A, Point B, Point Prize) triple)
    {
        var maxB = Math.Min(100, Math.Max(triple.Prize.Item1 / triple.B.Item1, triple.Prize.Item2 / triple.B.Item2));

        var candidates = Enumerable.Range(0, (int)maxB + 1)
            .Select(i => (Index: i, Value: triple.B.Multiply(maxB - i).Add(triple.A.Multiply(CalcA(i)))))
            .Where(t => t.Value == triple.Prize)
            .ToArray();

        return candidates.Length != 0 ? candidates.Min(t => maxB - t.Index + CalcA(t.Index) * 3) : 0;

        long CalcA(long i) => (triple.Prize.Item1 - triple.B.Item1 * (maxB - i)) / triple.A.Item1;
    }

    private static long CalculateMinimalTokensSmart((Point A, Point B, Point Prize) triple)
    {
        var matrix = new long[,] { { triple.A.Item1, triple.B.Item1 }, { triple.A.Item2, triple.B.Item2 } };
        
        if (!TryGetInvertedMatrixTimesDet(out var inverse, out var det, matrix))
            return 0;
        
        var (x, y) = Multiply(inverse, triple.Prize);

        if (x * det < 0 || y * det < 0)
            return 0;
        
        if (x % det != 0 || y % det != 0)
            return 0;
        
        return x / det * 3 + y / det;
    }
    
    private static Point Multiply(long[,] matrix, Point point)
        => (matrix[0, 0] * point.Item1 + matrix[0, 1] * point.Item2, matrix[1, 0] * point.Item1 + matrix[1, 1] * point.Item2);
    
    private static bool TryGetInvertedMatrixTimesDet(out long[,] inverse, out long det, long[,] matrix)
    {
        inverse = new long[2, 2];

        det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
        if (det == 0)
        {
            Console.WriteLine("Determinant is 0");            
            return false;
        }

        inverse[0, 0] = matrix[1, 1];
        inverse[1, 1] = matrix[0, 0];
        inverse[0, 1] = -matrix[0, 1];
        inverse[1, 0] = -matrix[1, 0];
        return true;
    }


    private static List<(Point A, Point B, Point Prize)> ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var data = new List<(Point A, Point B, Point Prize)>();

        Point a = (0, 0);
        Point b = (0, 0);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("Button A:"))
            {
                var nums = NumberRegex().Matches(line).Select(m => long.Parse(m.Value)).ToArray();
                a = (nums[0], nums[1]);
            }

            if (line.StartsWith("Button B:"))
            {
                var nums = NumberRegex().Matches(line).Select(m => long.Parse(m.Value)).ToArray();
                b = (nums[0], nums[1]);
            }

            if (line.StartsWith("Prize:"))
            {
                var nums = NumberRegex().Matches(line).Select(m => long.Parse(m.Value)).ToArray();
                var prize = (nums[0], nums[1]);
                data.Add((a, b, prize));
            }
        }

        return data;
    }


    [GeneratedRegex(@"\d+")]
    private static partial Regex NumberRegex();
}