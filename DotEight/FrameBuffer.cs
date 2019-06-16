using System;
using System.Collections.Generic;
using System.Text;
using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace DotEight
{
    public class Framebuffer
    {
        public const int Width = 64;
        public const int Height = 32;

        public RectangleShape[,] Screen { get; }

        public Framebuffer()
        {
            Screen = new RectangleShape[Height, Width];
            ClearScreen();
        }

        private static RectangleShape NewPixel(Vector2f position, byte red, byte green, byte blue)
        {
            RectangleShape pixel = new RectangleShape
            {
                Size = new Vector2f(20, 20),
                FillColor = new Color(red, green, blue),
                Position = position
            };
            return pixel;
        }

        public void ClearScreen()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Screen[y, x] = NewPixel(new Vector2f(x * 20, y * 20), 0, 0, 0);
                }
            }
        }

        public int DrawSprite(int x, int y, Sprite sprite)
        {
            int collision = 0;
            int z = 0;
            for (int i = y; i < y + sprite.Pixels.Length; i++)
            {
                byte row = sprite.Pixels[z];
                for (int j = x; j < x + 8; j++)
                {
                    if ((row & 0b1000_0000) == 0b1000_0000)
                    {
                        if (Screen[i % Height, j % Width].FillColor.Equals(new Color(255, 255, 255)))
                        {
                            //Console.WriteLine("Collision");
                            Screen[i % Height, j % Width].FillColor = new Color(0, 0, 0);
                            collision = 1;
                        }
                        else
                        {
                            //Console.WriteLine("Draw pixel");
                            Screen[i % Height, j % Width].FillColor = new Color(255, 255, 255);
                        }
                    }
                    else
                    {
                        //Console.WriteLine("No pixel");
                        //Screen[i, j].FillColor = new Color(0, 0, 0);
                    }

                    row <<= 1;
                    //Console.WriteLine(row);
                }
                z++;
            }
            return collision;
        }

        public void TestDraw()
        {
            Screen[10, 10].FillColor = new Color(255, 0, 0);
            Screen[11, 10].FillColor = new Color(255, 255, 0);
            Screen[12, 10].FillColor = new Color(255, 0, 0);
            Screen[13, 10].FillColor = new Color(255, 0, 255);
        }
    }
}