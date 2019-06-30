using System;
using SFML.Graphics;
using SFML.Window;

namespace DotEight
{
    class Emulator
    {
        private const int ClockSpeed = 500;
        private const int TimerSpeed = 60;
        private const int FrameSpeed = 60;

        private static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        private bool IsTime(long lastTime, int speed)
        {
            long iterationLength = 1000 / speed;
            long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return currentTime - lastTime >= iterationLength ? true : false;
        }

        public void Run(string rom)
        {
            // Create main window
            RenderWindow app = new RenderWindow(new VideoMode(1280, 640), "Dot Eight");
            app.Closed += new EventHandler(OnClose);

            // Initialize emulator
            CPU cpu = new CPU();
            cpu.LoadROM(rom);

            long lastClock = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long lastTimer = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long lastFrame = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            // Start the main loop
            app.Clear();
            while (app.IsOpen && cpu.ProgramCounter < 4096)
            {
                // Process events
                app.DispatchEvents();

                // Update emulator
                if (IsTime(lastClock, ClockSpeed))
                {
                    cpu.NextInstruction();
                    lastClock = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }
                if (IsTime(lastTimer, TimerSpeed))
                {
                    cpu.TimerTick();
                    lastTimer = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }
                if (IsTime(lastFrame, FrameSpeed))
                {
                    Framebuffer fb = cpu.CurrentFramebuffer;
                    foreach (RectangleShape pixel in fb.Frame)
                    {
                        app.Draw(pixel);
                    }
                    lastFrame = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }

                // Update the window
                app.Display();
            }
        }
    }
}
