using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode2024.Day3;

public static partial class Day3
{
    public static int Exercise1(string filePath)
    {
        return ReadData(filePath)
            .Select(line => MulRegex1().Matches(line).Select(match => match.Value))
            .SelectMany(mulMatches => mulMatches)
            .Select(GetProduct)
            .Sum();
    }

    public static int Exercise2(string filePath)
    {
        var matches = ReadData(filePath)
            .Select(line => MulRegex2().Matches(line).Select(match => match.Value))
            .SelectMany(mulMatches => mulMatches);
        
        var sum = 0;
        bool isActive = true;

        foreach (var match in matches)
        {
            if (match == "do()")
                isActive = true;
            else if (match == "don't()")
                isActive = false;
            else if (isActive)
                sum += GetProduct(match);
        } 

        return sum;
    }
    
    private static int GetProduct(string mulMatch)
    {
        var mul = mulMatch.Replace("mul", "")[1..^1].Split(',').Select(int.Parse).ToList();
        return mul[0] * mul[1];
    }

    private static List<string> ReadData(string filePath)
    {
        try
        {
            return File.ReadAllLines(filePath).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return [];
        }
    }

    [GeneratedRegex(@"mul\(\d{1,3},\d{1,3}\)")]
    private static partial Regex MulRegex1();
    
    [GeneratedRegex(@"(mul\(\d{1,3},\d{1,3}\)|do\(\)|don't\(\))")]
    private static partial Regex MulRegex2();
}