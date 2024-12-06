namespace AdventOfCode2024.Day1;

public static class Day1
{
    public static int Exercise1(string filePath)
    {
        var data = ReadDataExercise(filePath);
        
        var firstList = data.Select(x => x.Item1).OrderBy(_ => _);
        var secondList = data.Select(x => x.Item2).OrderBy(_ => _);

        return firstList.Zip(secondList, (x, y) => Math.Abs(x - y)).Sum();
    }
    
    public static int Exercise2(string filePath)
    {
        var data = ReadDataExercise(filePath);

        var firstList = data.Select(x => x.Item1);
        var occurrencesInSecondList = data.Select(x => x.Item2).GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        
        return firstList.Where(occurrencesInSecondList.ContainsKey).Sum(x => x * occurrencesInSecondList[x]);
    }
    
    private static List<(int, int)> ReadDataExercise(string filePath)
    {
        List<(int, int)> data = [];

        try
        {
            foreach (string line in File.ReadLines(filePath))
            {
                string[] parts = line.Split([' ', '\t'], StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int first) &&
                    int.TryParse(parts[1], out int second))
                {
                    data.Add((first, second));
                }
                else
                {
                    Console.WriteLine($"Skipping invalid line: {line}");
                }
            }

            // Console.WriteLine("Read data:");
            // foreach (var (first, second) in data)
            // {
            //     Console.WriteLine($"{first}, {second}");
            // }
            
            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return [];
        } 
    }
}
    