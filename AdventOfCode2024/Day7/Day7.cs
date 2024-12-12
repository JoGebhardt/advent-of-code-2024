using MoreLinq;

namespace AdventOfCode2024.Day7;

public static class Day7
{
    public static ulong Exercise1(string filePath) => MainLogic(filePath, ["+", "*"]);
    public static ulong Exercise2(string filePath) => MainLogic(filePath, ["+", "*", "||"]);
    
    static ulong MainLogic(string filePath, string[] operations)
    {
        var data = ReadData(filePath);
        ulong sum = 0;

        foreach (var (result, values) in data)
        {
            var isValid = CreateAllPossibleOperations(operations, values.Count - 1).Select(
                operations => values
                    .Skip(1)
                    .Zip(operations, (v, o) => (v, o))
                    .Aggregate(values[0], (acc, tuple) => Calculate(acc, tuple.v, tuple.o))
            )                
            .Any(v => v == result);

            if (isValid)
                sum += result;
        }

        return sum;
    }

    static ulong Calculate(ulong x, ulong y, string operation) => operation switch
    {
        "+" => x + y,
        "*" => x * y,
        "||" => x * (ulong)Math.Pow(10, Math.Floor(Math.Log10(y)) + 1) + y,
        _ => throw new InvalidOperationException()
    };

    static IEnumerable<IEnumerable<string>> CreateAllPossibleOperations(string[] operations, int length)
    {
        int[] indexes = new int[length];
        
        for (ulong i = 0; i < Math.Pow(operations.Length, length); i++)
        {
            yield return indexes.Select(k => operations[k]);
            Increment();
        }
        
        void Increment(int pos = 0)
        {
            if (pos == length)
                return;
            
            if (indexes[pos] == operations.Length - 1)
            {
                indexes[pos] = 0;
                Increment(pos + 1);
            }
            else
            {
                indexes[pos]++;
            }
        }
    }

    public static List<(ulong, List<ulong>)> ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var data = new List<(ulong, List<ulong>)>();

        foreach (var line in lines)
        {
            var parts = line.Split(':');
            var result = ulong.Parse(parts[0]);
            var values = parts[1].Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .Select(ulong.Parse)
                .ToList();

            data.Add((result, values));
        }
        return data;
    }
}