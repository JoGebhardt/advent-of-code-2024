using System.Text.RegularExpressions;
using MoreLinq;

namespace AdventOfCode2024.Day9;

public static class Day9
{
    public static long Exercise1(string filePath)
    {
        var data = ReadData(filePath);
        var expandedDiskMap = data.ToExpandedDiskMap();

        while (expandedDiskMap.SwitchLastDigitWithFirstDot()) { }

        return expandedDiskMap.CheckSum();
    }

    public static long Exercise2(string filePath)
    {
        var data = ReadData(filePath);
        var blocks = data.ToExpandedDiskMap().GetBlocks();
        var blockIndex = blocks.Count - 1;

        while (blocks.SwitchBlockWithDots(ref blockIndex))
            blockIndex--;

        return blocks.SelectMany(_ => _).ToList().CheckSum();
    }
    
    private static long CheckSum(this List<long> list) => list
        .Select((c, index) => (c, index))
        .Where(_ => _.c != -1)
        .Sum(_ => _.c * _.index);

    private static List<long> ToExpandedDiskMap(this string s)
    {
        return s.Select((c, index) => (c, index)).Aggregate(new List<long>(), (acc, tuple) =>
        {
            var (c, index) = tuple;
            var (num, id) = (int.Parse(c.ToString()), index / 2);
            
            for (int i = 0; i < num; i++)
                acc.Add(index % 2 == 0 ? id : -1);

            return acc;
        });
    }

    private static bool SwitchLastDigitWithFirstDot(this List<long> list)
    {
        var indexOfFirstDot = list.GetIndexOfFirstDot();
        var indexOfLastDigit = list.GetIndexOfLastDigit();
        
        if (indexOfFirstDot >= indexOfLastDigit)
            return false;

        list[indexOfFirstDot] = list[indexOfLastDigit];
        list[indexOfLastDigit] = -1;

        return true;
    }

    private static int GetIndexOfFirstDot(this List<long> list) => list.IndexOf(-1);
    private static int GetIndexOfLastDigit(this List<long> list) => list.Select((c, index) => (c, index)).Last(_ => _.c != -1).index;
    
    private static bool SwitchBlockWithDots(this List<List<long>> blocks, ref int index)
    {
        if (index == 0) return false;
        
        var block = blocks[index];
        
        if (block.Contains(-1)) return true;
        
        var indexCopy = index;
        var (dots, indexOfDots) = blocks.Select((b, i) => (b, i))
            .Where(_ => _.i < indexCopy)
            .FirstOrDefault(_ => _.b.Contains(-1) && _.b.Count >= block.Count);
        
        if (dots == null) return true;
        
        if (dots.Count == block.Count)
            (blocks[index], blocks[indexOfDots]) = (dots, block);
        else
        {
            var newDots = dots.Skip(block.Count).ToList();
            dots = dots.Take(block.Count).ToList();
            (blocks[index], blocks[indexOfDots]) = (dots, block);
            blocks.Insert(indexOfDots + 1, newDots);
            index++;
        }
        
        return true;
    }
    
    private static List<List<long>> GetBlocks(this List<long> list)
    {
        var blocks = new List<List<long>>();
        var block = new List<long>();
        var current = list[0];
        
        foreach (var c in list)
        {
            if (c == current)
            {
                block.Add(c);
            }
            else
            {
                blocks.Add(block);
                block = [c];
                current = c;
            }
        }
        
        blocks.Add(block);
        return blocks;
    }

    private static string ReadData(string filePath) => File.ReadAllText(filePath);
}