namespace AOC22
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var bricks = new List<Brick>();
            for (int i = 0; i < lines.Length; i++)
            {
                bricks.Add(new Brick(lines[i], i + 1));
            }
            Console.WriteLine($"Bricks: {bricks.Count()}");

            int maxX = bricks.Select(b => Math.Max(b.P1.X, b.P2.X)).Max();
            int maxY = bricks.Select(b => Math.Max(b.P1.Y, b.P2.Y)).Max();
            int maxZ = bricks.Select(b => Math.Max(b.P1.Z, b.P2.Z)).Max();
            Console.WriteLine($"Max: X: {maxX}, Y: {maxY}, Z: {maxZ}");
            int xLen = maxX + 1;
            int yLen = maxY + 1;
            int zLen = maxZ + 1;
            var grid = new int[xLen, yLen, zLen];

            for (int i = 0; i < bricks.Count(); i++)
            {
                foreach (var c in bricks[i].GetCoords())
                {
                    grid[c.X, c.Y, c.Z] = i + 1;
                }
            }

            bool brickMoved = true;
            while (brickMoved)
            {
                var processed = new HashSet<int>();
                brickMoved = false;
                for (int z = 2; z < zLen; z++)
                {
                    for (int y = 0; y < yLen; y++)
                    {
                        for (int x = 0; x < xLen; x++)
                        {
                            var id = grid[x, y, z];

                            if (id != 0 && !processed.Contains(id))
                            {
                                bool moved = bricks[id - 1].MoveDown(grid);
                                brickMoved = brickMoved || moved;
                                processed.Add(id);
                            }
                        }
                    }
                }
            }

            var supports = new Dictionary<int, List<int>>();
            var supportedBy = new Dictionary<int, List<int>>();

            foreach (var b in bricks)
            {
                supports[b.Id] = b.IsSupporting(grid);
                supportedBy[b.Id] = b.SupportedBy(grid);
            }

            int countSafeBricks = supports.Count(kvp => kvp.Value.All(b => supportedBy[b].Count > 1));
            Console.WriteLine($"Bricks that can be disintegrated without any falling (part 1): {countSafeBricks}");

            long countFalling = 0;
            foreach (var b in bricks)
            {
                var fallen = new HashSet<int>();
                var frontier = new Queue<int>();
                frontier.Enqueue(b.Id);
                countFalling--; // don't count the first
                while (frontier.Count > 0)
                {
                    var cur = frontier.Dequeue();

                    fallen.Add(cur);
                    countFalling++;
                    foreach (var n in supports[cur])
                    {
                        if (fallen.Contains(n))
                            continue;

                        if (supportedBy[n].All(fallen.Contains))
                            frontier.Enqueue(n);

                    }
                }
            }
            Console.WriteLine($"Total falling (part 2): {countFalling}");

            //PrintGridX(grid);
            //Console.WriteLine();
            //PrintGridY(grid);
        }

        static void PrintGridY(int[,,] grid)
        {
            for (int z = grid.GetLength(2) - 1; z >= 0; z--)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (z == 0)
                    {
                        Console.Write('~');
                        continue;
                    }
                    var digit = 0;
                    for (int x = 0; x < grid.GetLength(0); x++)
                    {
                        if (grid[x, y, z] != 0)
                        {
                            digit = grid[x, y, z];
                            break;
                        }
                    }
                    if (digit == 0)
                        Console.Write(' ');
                    else
                        Console.Write((char)('A' + digit - 1));
                }
                Console.WriteLine();
            }
        }

        static void PrintGridX(int[,,] grid)
        {
            for (int z = grid.GetLength(2) - 1; z >= 0; z--)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    if (z == 0)
                    {
                        Console.Write('~');
                        continue;
                    }
                    var digit = 0;
                    for (int y = 0; y < grid.GetLength(0); y++)
                    {
                        if (grid[x, y, z] != 0)
                        {
                            digit = grid[x, y, z];
                            break;
                        }
                    }
                    if (digit == 0)
                        Console.Write(' ');
                    else
                        Console.Write((char)('A' + digit - 1));
                }
                Console.WriteLine();
            }
        }
    }
}
