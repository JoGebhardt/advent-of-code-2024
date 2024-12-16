using System.Numerics;

namespace AdventOfCode2024.Helper;

public static class Helper
{
    public static T Get<T>(this T[,] arr, (int x, int y) pos) => arr[pos.x, pos.y];

    public static void Set<T>(this T[,] arr, (int x, int y) pos, T value) => arr[pos.x, pos.y] = value;

    public static bool IsInBounds<T>(this T[,] arr, (int x, int y) pos) => pos.x >= 0 && pos.x < arr.GetLength(0) && pos.y >= 0 && pos.y < arr.GetLength(1);

    public static T[,] ToJagged<T>(this T[][] arr)
    {
        var jagged = new T[arr.Length, arr[0].Length];

        for (var i = 0; i < arr.Length; i++)
        {
            if (arr[i].Length != arr[0].Length)
                throw new ArgumentException("All rows must have the same length");

            for (var j = 0; j < arr[0].Length; j++)
            {
                jagged[i, j] = arr[i][j];
            }
        }

        return jagged;
    }
    
    public static IEnumerable<(int, int)> Coordinates<T>(this T[,] arr)
    {
        for (var i = 0; i < arr.GetLength(0); i++)
        {
            for (var j = 0; j < arr.GetLength(1); j++)
            {
                yield return (i, j);
            }
        }
    }
    
    public static (T, T) Add<T>(this (T x, T y) a, (T x, T y) b) where T : INumber<T>
        => (a.x + b.x, a.y + b.y);
    
    public static (T, T) Subtract<T>(this (T x, T y) a, (T x, T y) b) where T : INumber<T>
        => (a.x - b.x, a.y - b.y);
    
    public static (T, T) Multiply<T>(this (T x, T y) a, T b) where T : INumber<T>
        => (a.x * b, a.y * b);
}