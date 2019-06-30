namespace DotEight
{
    internal class Program
    {
        private static void Main()
        {
            Emulator emu = new Emulator();
            string rom = "C:\\Users\\jamie\\Downloads\\Trip8.ch8";
            emu.Run(rom);
        }
    }
}