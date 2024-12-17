using AdventOfCode2024.Day1;
using AdventOfCode2024.Day2;
using AdventOfCode2024.Day3;
using AdventOfCode2024.Day4;
using AdventOfCode2024.Day5;
using AdventOfCode2024.Day6;
using AdventOfCode2024.Day7;
using AdventOfCode2024.Day8;
using AdventOfCode2024.Day9;
using AdventOfCode2024.Day10;
using AdventOfCode2024.Day11;
using AdventOfCode2024.Day12;
using AdventOfCode2024.Day13;


var day = "Day13";
var file = "data.txt";

var filePath = Path.Combine(Directory.GetCurrentDirectory(), day, file);

Console.WriteLine($"Solution for exercise 1: {Day13.Exercise1(filePath)}");
Console.WriteLine($"Solution for exercise 2: {Day13.Exercise2(filePath)}");
