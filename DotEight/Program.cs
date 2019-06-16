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
            int UPDATES_PER_SECOND = 500;
            int WAIT_TICKS = 1000 / UPDATES_PER_SECOND;
            int MAX_FRAMESKIP = 5;

            long next_update = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            int MAX_UPDATES_PER_SECOND = 500;
            int MIN_WAIT_TICKS = 1000 / MAX_UPDATES_PER_SECOND;

            long last_update = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            int frames_skipped;

            CPU cpu = new CPU();
            cpu.LoadROM("C:\\Users\\jamie\\Downloads\\Clock Program.ch8");

            // Create the main window
            RenderWindow app = new RenderWindow(new VideoMode(1280, 640), "Dot Eight");
            app.Closed += new EventHandler(OnClose);

            // Start the game loop
            app.Clear();
            while (app.IsOpen && cpu.ProgramCounter < 4096)
            {
                // Process events
                app.DispatchEvents();

                // Delay if needed
                while (DateTimeOffset.Now.ToUnixTimeMilliseconds() < last_update + MIN_WAIT_TICKS)
                {
                    System.Threading.Thread.Sleep(0);
                }
                last_update = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                // Update game:
                frames_skipped = 0;
                while (DateTimeOffset.Now.ToUnixTimeMilliseconds() > next_update
                        && frames_skipped < MAX_FRAMESKIP && cpu.ProgramCounter < 4095)
                {
                    // Update input, move objects, do collision detection...
                    UInt16 opcode = (UInt16)((cpu.MEMORY[cpu.ProgramCounter] << 8) + cpu.MEMORY[cpu.ProgramCounter + 1]);
                    int test = cpu.Execute(opcode);
                    if (cpu.DelayTimer > 0)
                        cpu.DelayTimer--;

                    // Schedule next update:
                    next_update += WAIT_TICKS;
                    frames_skipped++;
                }

                // Draw pixels
                Framebuffer fb = cpu.CurrentFramebuffer;
                foreach (RectangleShape pixel in fb.Screen)
                {
                    app.Draw(pixel);
                }

                // Update the window
                app.Display();
            }
        }
    }
}