using SFML.Graphics;
using SFML.System;

namespace DotEight
{
    public class Framebuffer
    {
        private const int Width = 64;
        private const int Height = 32;

        private const int PixelScale = 20; // Window is 1280x640 pixels

        public RectangleShape[,] Frame;

        public Framebuffer()
        {
            Frame = new RectangleShape[Height, Width];
            ClearScreen();
        }

        private static RectangleShape NewPixel(Vector2f position, byte red, byte green, byte blue)
        {
            RectangleShape pixel = new RectangleShape
            {
                Size = new Vector2f(PixelScale, PixelScale),
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
                    Frame[y, x] = NewPixel(new Vector2f(x * PixelScale, y * PixelScale), 0, 0, 0);
                }
            }
        }

        public int DrawSprite(int x, int y, byte[] pixels)
        {
            int collision = 0;
            for (int i = y; i < y + pixels.Length; i++)
            {
                byte row = pixels[i - y];
                for (int j = x; j < x + 8; j++)
                {
                    if ((row & 0b1000_0000) == 0b1000_0000)
                    {
                        if (Frame[i % Height, j % Width].FillColor.Equals(new Color(255, 255, 255)))
                        {
                            Frame[i % Height, j % Width].FillColor = new Color(0, 0, 0);
                            collision = 1;
                        }
                        else
                        {
                            Frame[i % Height, j % Width].FillColor = new Color(255, 255, 255);
                        }
                    }
                    row <<= 1;
                }
            }
            return collision;
        }
    }
}