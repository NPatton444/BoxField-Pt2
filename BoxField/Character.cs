using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoxField
{
    class Character
    {
        public int x, y, size, speed;

        public Character(int _x, int _y, int _size, int _speed)
        {
            x = _x;
            y = _y;
            size = _size;
            speed = _speed;
        }

        public void move(Character ch, string direction)
        {
            if (direction == "left")
            {
                ch.x -= ch.speed;
            }
            else
            {
                ch.x += ch.speed;
            }
        }

        public bool collision(Character ch, Cube c)
        {
            Rectangle chRec = new Rectangle(ch.x, ch.y, ch.size, ch.size);
            Rectangle cRec = new Rectangle(c.x, c.y, c.size, c.size);

            if(chRec.IntersectsWith(cRec))
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
