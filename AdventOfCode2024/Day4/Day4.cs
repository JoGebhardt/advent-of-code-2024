using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode2024.Day4;

public static partial class Day4
{
    [GeneratedRegex( @"(?=(XMAS|SAMX))")]
    private static partial Regex XmasRegex();
    
    private static readonly List<string> Mas = ["MAS", "SAM"];

    public static int Exercise1(string filePath)
    {
        var data = ReadData(filePath);
        
        return HorizontalLines(data)
            .Concat(VerticalLines(data))
            .Concat(DiagonalLinesTopRightToBottomLeft(data))
            .Concat(DiagonalLinesTopRightToBottomLeft(FlitHorizontally(data)))
            .Select(static line => XmasRegex().Matches(line).Count)
            .Sum();
        
        static List<string> HorizontalLines(char[,] grid)
        {
            var lines = new List<string>();
            
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                var line = "";
                
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    line += grid[i, j];
                }
                
                lines.Add(line);
            }
            
            return lines;
        }
        
        static List<string> VerticalLines(char[,] grid)
        {
            var lines = new List<string>();
            
            for (var i = 0; i < grid.GetLength(1); i++)
            {
                var line = "";
                
                for (var j = 0; j < grid.GetLength(0); j++)
                {
                    line += grid[j, i];
                }
                
                lines.Add(line);
            }

            return lines;
        }
        
        static List<string> DiagonalLinesTopRightToBottomLeft(char[,] grid)
        {
            var lines = new List<string>();
            
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                var line = "";
                
                for (var j = 0; j < grid.GetLength(1); j++)
                {
                    if (i + j < grid.GetLength(0))
                        line += grid[i + j, j];
                }
                
                lines.Add(line);
            }
            
            for (var i = 1; i < grid.GetLength(1); i++)
            {
                var line = "";
                
                for (var j = 0; j < grid.GetLength(0); j++)
                {
                    if (i + j < grid.GetLength(1))
                        line += grid[j, i + j];
                }
                
                lines.Add(line);
            }

            return lines;
        }
        
        static char[,] FlitHorizontally(char[,] data)
        {
            var newData = new char[data.GetLength(0), data.GetLength(1)];
            
            for (var i = 0; i < data.GetLength(0); i++)
            {
                for (var j = 0; j < data.GetLength(1); j++)
                {
                    newData[i, j] = data[i, data.GetLength(1) - j - 1];
                }
            }

            return newData;
        }
    }
    
    public static int Exercise2(string filePath)
    {
        var data = ReadData(filePath);
        var counter = 0;

        for (var i = 0; i < data.GetLength(0) - 2; i++)
        {
            for (var j = 0; j < data.GetLength(1) - 2; j++)
            {
                if (ContainsXmasCross(Create3x3Square(data, i, j)))
                    counter++;
            } 
        }
        
        return counter;
        
        static char[,] Create3x3Square(char[,] data, int i, int j)
        {
            var square = new char[3, 3];
            
            for (var k = 0; k < 3; k++)
            {
                for (var l = 0; l < 3; l++)
                {
                    square[k, l] = data[i + k, j + l];
                }
            }

            return square;
        }
        
        static bool ContainsXmasCross(char[,] square)
        {
            var diagonal1 = $"{square[0, 0]}{square[1, 1]}{square[2, 2]}";
            var diagonal2 = $"{square[0, 2]}{square[1, 1]}{square[2, 0]}";
            
            return Mas.Contains(diagonal1) && Mas.Contains(diagonal2);
        }
    }
    
    private static char[,] ReadData(string filePath)
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            var grid = new char[lines.Length, lines[0].Length];
            
            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = 0; j < lines[0].Length; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }

            return grid;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new char[0, 0];
        }
    }
}