using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC22
{
    internal class Brick
    {
        public Coord P1 { get; set; }
        public Coord P2 { get; set; }
        public int Id { get; set; }

        public Brick(string brickLine, int id)
        {
            var parts = brickLine.Split('~');
            P1 = new Coord(parts[0]);
            P2 = new Coord(parts[1]);
            Id = id;
        }

        public List<Coord> GetCoords()
        {
            return getCoords(P1, P2);
        }

        private static List<Coord> getCoords(Coord p1, Coord p2)
        {
            var ret = new List<Coord>();
            for (int x = Math.Min(p1.X, p1.X); x <= Math.Max(p1.X, p2.X); x++)
            {
                for (int y = Math.Min(p1.Y, p1.Y); y <= Math.Max(p1.Y, p2.Y); y++)
                {
                    for (int z = Math.Min(p1.Z, p1.Z); z <= Math.Max(p1.Z, p2.Z); z++)
                    {
                        ret.Add(new Coord(x, y, z));
                    }
                }
            }
            return ret;
        }

        public bool MoveDown(int[,,] grid)
        {
            var newP1 = P1.MoveDown();
            var newP2 = P2.MoveDown();

            if(newP1.Z <= 0 || newP2.Z <= 0)
                return false;

            var newCoords = getCoords(newP1, newP2);

            foreach (var coord in newCoords)
            {
                if (grid[coord.X, coord.Y, coord.Z] != 0 && grid[coord.X, coord.Y, coord.Z] != Id)
                {
                    return false;
                }
            }

            foreach (var coord in GetCoords())
            {
                grid[coord.X, coord.Y, coord.Z] = 0;
            }

            P1 = newP1;
            P2 = newP2;

            foreach (var coord in newCoords)
            {
                grid[coord.X, coord.Y, coord.Z] = Id;
            }

            return true;
        }

        public List<int> IsSupporting(int[,,] grid)
        {
            var ret = new HashSet<int>();
            var coords = GetCoords();

            foreach (var coord in coords)
            {
                if (coord.Z + 1 >= grid.GetLength(2))
                    continue;

                var val = grid[coord.X, coord.Y, coord.Z + 1];
                if (val == 0 || val == Id)
                    continue;

                ret.Add(val);
            }

            return ret.ToList();
        }

        public List<int> SupportedBy(int[,,] grid)
        {
            var ret = new HashSet<int>();
            var coords = GetCoords();

            foreach (var coord in coords)
            {
                if (coord.Z - 1 <= 0)
                    continue;

                var val = grid[coord.X, coord.Y, coord.Z - 1];
                if (val == 0 || val == Id)
                    continue;

                ret.Add(val);
            }

            return ret.ToList();
        }

        public override string ToString()
        {
            return $"{(char)('A' + Id - 1)}";
        }
    }
}
