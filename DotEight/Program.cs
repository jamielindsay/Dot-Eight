using System;
using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace DotEight
{
    internal class Program
    {
        private static void OnClose(object sender, EventArgs e)
        {
            // Close the window when OnClose event is received
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        private static void Main()
        {
            // Create the main window
            RenderWindow app = new RenderWindow(new VideoMode(1280, 640), "Dot Eight");
            app.Closed += new EventHandler(OnClose);

            // Create framebuffer and draw test image
            Framebuffer fb = new Framebuffer();
            fb.TestDraw();

            // Start the game loop
            app.Clear();
            while (app.IsOpen)
            {
                // Process events
                app.DispatchEvents();

                // Draw pixels
                foreach (RectangleShape pixel in fb.Pixels)
                {
                    app.Draw(pixel);
                }

                // Update the window
                app.Display();
            }
        }
    }
}