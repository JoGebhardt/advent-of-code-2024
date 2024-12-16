namespace AdventOfCode2024.Day11;

public static class Day11
{
    public static long Exercise1(string filePath) => MainLogic(filePath, 25);
    public static long Exercise2(string filePath) => MainLogic(filePath, 75);

    private static long MainLogic(string filePath, int iterations)
    {
        var data = ReadData(filePath);
        var stones = data.GroupBy(x => x).ToDictionary(x => x.Key, x => (long)x.Count());

        for (var i = 0; i < iterations; i++)
        {
            var newStones = new Dictionary<long, long>();

            foreach (var (stone, count) in stones)
            {
                ApplyRule(stone).ForEach(newStone =>
                {
                    if (newStones.ContainsKey(newStone))
                        newStones[newStone] += count;
                    else
                        newStones[newStone] = count;
                });
            }
            
            stones = newStones;
        }

        return stones.Sum(x => x.Value);
    }

    private static List<long> ApplyRule(long val)
    {
        if (val == 0)
            return [1];

        if (val.ToString().Length % 2 == 0)
        {
            var firstHalf = val.ToString()[..(val.ToString().Length / 2)];
            var secondHalf = val.ToString()[(val.ToString().Length / 2)..];

            return [long.Parse(firstHalf), long.Parse(secondHalf)];
        }

        return [val * 2024];
    }

    private static List<long> ReadData(string filePath)
    {
        return File.ReadAllLines(filePath)[0].Split(' ').Select(long.Parse).ToList();
    }
}