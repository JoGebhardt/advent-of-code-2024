using MoreLinq;

namespace AdventOfCode2024.Day2;

public static class Day2
{
    public static int Exercise1(string filePath)
    {
        return ReadData(filePath)
            .Count(ReportIsSafe);
    }
    
    public static int Exercise2(string filePath)
    {
        return ReadData(filePath)
            .Count(ReportIsSafeWithProblemDampener);

        static bool ReportIsSafeWithProblemDampener(List<int> report)
        {
            for (var i = 0; i < report.Count - 1; i++)
            {
                report.RemoveAt(i);
                if (ReportIsSafe(report))
                {
                    return true;
                }
            }
            return false;
        }
    }

    private static bool ReportIsSafe(IEnumerable<int> report)
    {
        var pairwise = report.Pairwise((a, b) => (a, b)).ToList();

        if (pairwise.Any(pair => Math.Abs(pair.a - pair.b) < 1 || Math.Abs(pair.a - pair.b) > 3))
        {
            return false;
        }
        
        return pairwise.All(pair => pair.a < pair.b) || pairwise.All(pair => pair.a > pair.b);
    }
    
    private static List<List<int>> ReadData(string filePath)
    {
        try
        {
            return File.ReadAllLines(filePath)
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(nums => nums.Select(int.Parse).ToList())
                .ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return [];
        } 
    }
}