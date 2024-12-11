using System.Diagnostics.Metrics;

namespace AdventOfCode2024.Day6;

public static class Day6
{
    private static readonly char[] Positions = ['^', '>', 'v', '<'];

    public static int Exercise1(string filePath)
    {
        var data = ReadData(filePath);

        var visited = new HashSet<(int, int)>();
        var pos = CurrentPosition(data);
        var dir = data[pos.X, pos.Y];

        while (true)
        {
            visited.Add(pos);

            var nextPosition = (X: pos.X + NextStep(dir).X, Y: pos.Y + NextStep(dir).Y);

            if (IsOutOfBounds(data, nextPosition))
            {
                break;
            }
            else if (data[nextPosition.X, nextPosition.Y] == '#')
            {
                dir = GetNextDirection(dir);
                pos = (pos.X + NextStep(dir).X, pos.Y + NextStep(dir).Y);
            }
            else
            {
                pos = nextPosition;
            }
        }
        return visited.Count;
    }

    public static int Exercise2(string filePath)
{
    var originalData = ReadData(filePath);
    var counter = 0;

    for (var i = 0; i < originalData.GetLength(0); i++)
    {
        for (var j = 0; j < originalData.GetLength(1); j++)
        {
            // Skip the guard's current position and walls
            if (originalData[i, j] == '#' || Positions.Contains(originalData[i, j]))
                continue;

            var data = (char[,])originalData.Clone();
            data[i, j] = '#';

            var path = new HashSet<((int, int), char)>();

            var pos = CurrentPosition(data);
            var dir = data[pos.X, pos.Y];

            while (true)
            {
                if (!path.Add((pos, dir)))
                {
                    counter++;
                    break;
                }

                var nextPos = (X: pos.X + NextStep(dir).X, Y: pos.Y + NextStep(dir).Y);

                if (IsOutOfBounds(data, nextPos))
                {
                    // If out of bounds, guard can't proceed; break out
                    break;
                }
                else if (data[nextPos.X, nextPos.Y] == '#')
                {
                    // Attempt to turn right up to 4 times
                    bool moved = false;
                    for (int k = 0; k < 4; k++)
                    {
                        dir = GetNextDirection(dir);
                        var attemptPos = (X: pos.X + NextStep(dir).X, Y: pos.Y + NextStep(dir).Y);

                        if (!IsOutOfBounds(data, attemptPos) && data[attemptPos.X, attemptPos.Y] != '#')
                        {
                            pos = attemptPos;
                            moved = true;
                            break;
                        }
                    }

                    // If after checking all directions we still can't move, guard is stuck
                    if (!moved)
                    {
                        // Guard stuck in place - no loop detected here unless it coincides with a visited state
                        break;
                    }
                }
                else
                {
                    // Move forward in the current direction
                    pos = nextPos;
                }
            }
        }
    }

    return counter;
}

    static (int X, int Y) CurrentPosition(char[,] data)
    {
        for (var i = 0; i < data.GetLength(0); i++)
            for (var j = 0; j < data.GetLength(1); j++)
                if (Positions.Contains(data[i, j]))
                    return (i, j);
        throw new Exception("No position found");
    }

    static bool IsOutOfBounds(char[,] data, (int X, int Y) pos)
        => pos.X < 0 || pos.X >= data.GetLength(0) || pos.Y < 0 || pos.Y >= data.GetLength(1);

    static char GetNextDirection(char direction) => direction switch
    {
        '^' => '>',
        '>' => 'v',
        'v' => '<',
        '<' => '^',
        _ => ' '
    };

    static (int X, int Y) NextStep(char direction) => direction switch
    {
        '^' => (-1, 0),
        '>' => (0, 1),
        'v' => (1, 0),
        '<' => (0, -1),
        _ => (0, 0)
    };

    private static char[,] ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var data = new char[lines.Length, lines[0].Length];

        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[i].Length; j++)
            {
                data[i, j] = lines[i][j];
            }
        }

        return data;
    }
}