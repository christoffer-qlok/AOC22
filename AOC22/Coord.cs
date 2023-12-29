using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC22
{
    internal struct Coord
    {
        public int X;
        public int Y;
        public int Z;

        public Coord(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Coord(string s)
        {
            var coords = s.Split(',').Select(int.Parse).ToArray();
            X = coords[0];
            Y = coords[1];
            Z = coords[2];
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        public Coord MoveDown()
        {
            return new Coord(X, Y, Z - 1);
        }
    }
}
