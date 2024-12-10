using MoreLinq;

namespace AdventOfCode2024.Day5;

public static class Day5
{
    public static int Exercise1(string filePath)
    {
        var (rules, updates) = ReadData(filePath);
        
        return updates
            .Where(update => IsValidUpdate(rules, update))
            .Select(update => update.ElementAt(update.Count / 2))
            .Sum();
    }
    
    public static int Exercise2(string filePath)
    {
        var (rules, updates) = ReadData(filePath);

        return updates
            .Where(update => !IsValidUpdate(rules, update))
            .Select(update => OrderedUpdate(rules, update))
            .Select(update => update.ElementAt(update.Count / 2))
            .Sum();
        
        static List<int> OrderedUpdate(List<(int, int)> rules, List<int> update)
        {
            var rulesCopy = rules.Where(rule => update.Contains(rule.Item1) && update.Contains(rule.Item2)).ToList();
            var updateCopy = update.ToList();

            var ordered = new List<int>();
            
            while (updateCopy.Count > 0)
            {
                var next = updateCopy.First(num => rulesCopy.All(rule => rule.Item2 != num));
                
                rulesCopy = rulesCopy.Where(rule => rule.Item1 != next).ToList();
                updateCopy.Remove(next);
                
                ordered.Add(next);
            }
            
            return ordered;
        }
    }

    static bool IsValidUpdate(List<(int, int)> rules, List<int> update)
        => update.Cartesian(update, (a, b) => (a, b))
            .Where(pair => update.IndexOf(pair.Item1) < update.IndexOf(pair.Item2))
            .All(pair => IsValidPair(rules, pair));

    static bool IsValidPair(List<(int, int)> rules, (int, int) pair)
        => !rules.Any(rule => rule.Item1 == pair.Item2 && rule.Item2 == pair.Item1);

    private static (List<(int, int)> Rules, List<List<int>> Updates) ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var rules = new List<(int, int)>();
        var updates = new List<List<int>>();
        
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }
            
            if (line.Contains('|'))
            {
                var parts = line.Split('|');
                rules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
            }
            else
            {
                updates.Add(line.Split(',').Select(int.Parse).ToList());
            }
        }
        
        return (rules, updates);
    }
    
}