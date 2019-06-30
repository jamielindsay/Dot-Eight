namespace DotEight
{
    internal class Program
    {
        private static void Main()
        {
            Emulator emu = new Emulator();
            string rom = "";
            emu.Run(rom);
        }
    }
}
