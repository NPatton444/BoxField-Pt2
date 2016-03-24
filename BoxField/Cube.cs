using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxField
{
    class Cube
    {
        public int x, y, size, speed, colour;

        public Cube (int _x, int _y, int _size, int _speed, int _colour)
        {
            x = _x;
            y = _y;
            size = _size;
            speed = _speed;
            colour = _colour;
        }
    }
}
