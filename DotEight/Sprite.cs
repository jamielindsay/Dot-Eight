using System;
using System.Collections.Generic;
using System.Text;

namespace DotEight
{
    public class Sprite
    {
        public byte[] Pixels;

        public Sprite()
        {
            Pixels = new byte[15];
        }

        public Sprite(byte[] image)
        {
            Pixels = image;
        }
    }
}
