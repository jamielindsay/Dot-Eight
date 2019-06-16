using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DotEight
{
    public class CPU
    {
        // Fonts
        byte[] chip8_fontset = new byte[]
        {
            0xF0, 0x90, 0x90, 0x90, 0xF0, //0
            0x20, 0x60, 0x20, 0x20, 0x70, //1
            0xF0, 0x10, 0xF0, 0x80, 0xF0, //2
            0xF0, 0x10, 0xF0, 0x10, 0xF0, //3
            0x90, 0x90, 0xF0, 0x10, 0x10, //4
            0xF0, 0x80, 0xF0, 0x10, 0xF0, //5
            0xF0, 0x80, 0xF0, 0x90, 0xF0, //6
            0xF0, 0x10, 0x20, 0x40, 0x40, //7
            0xF0, 0x90, 0xF0, 0x90, 0xF0, //8
            0xF0, 0x90, 0xF0, 0x10, 0xF0, //9
            0xF0, 0x90, 0xF0, 0x90, 0x90, //A
            0xE0, 0x90, 0xE0, 0x90, 0xE0, //B
            0xF0, 0x80, 0x80, 0x80, 0xF0, //C
            0xE0, 0x90, 0x90, 0x90, 0xE0, //D
            0xF0, 0x80, 0xF0, 0x80, 0xF0, //E
            0xF0, 0x80, 0xF0, 0x80, 0x80  //F
        };

        // Memory
        public byte[] MEMORY;

        // Registers
        public UInt16 ProgramCounter; // Stores currently executing address
        public Stack<UInt16> AddressStack; // Stack of addresses to return to after subroutine finishes

        public byte[] V;

        public byte DelayTimer;
        public byte SoundTimer;

        public UInt16 I; // Used to store a memory address for some opcodes

        public Random rand;

        // Screen
        public Framebuffer CurrentFramebuffer { get; set; }

        public CPU()
        {
            MEMORY = new byte[4096];

            for (int i = 0; i < 80; i++)
                MEMORY[i] = chip8_fontset[i];

            ProgramCounter = 0x0200;
            AddressStack = new Stack<UInt16>(16);

            V = new byte[16];

            DelayTimer = 0;
            SoundTimer = 0;

            I = 0;

            rand = new Random();

            CurrentFramebuffer = new Framebuffer();
        }

        public void LoadROM(string file)
        {
            byte[] ROM = File.ReadAllBytes(file);
            int i2 = 0x200;
            foreach (byte b in ROM)
            {
                MEMORY[i2] = b;
                i2++;
            }
        }

        public int Execute(UInt16 opcode)
        {
            string ophex = String.Format("{0:X}", opcode);
            Console.WriteLine("{0} : {1}", (ProgramCounter - 0x200), ophex);
            switch (opcode & 0xF000)
            {
                case 0x0000:
                    switch (opcode)
                    {
                        case 0x00E0:
                            CurrentFramebuffer.ClearScreen();
                            ProgramCounter += 2;
                            return 1;
                        case 0x00EE:
                            ProgramCounter = AddressStack.Pop();
                            ProgramCounter += 2;
                            return 2;
                        default: // Calls RCA 1802 program at address NNN. Not necessary for most ROMs.
                            throw new Exception("Undefined instruction");
                    }
                case 0x1000:
                    ProgramCounter = (UInt16)(opcode & 0x0FFF);
                    return 4;
                case 0x2000:
                    AddressStack.Push(ProgramCounter);
                    ProgramCounter = (UInt16)(opcode & 0x0FFF);
                    return 5;
                case 0x3000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00) >> 8] == (UInt16)(opcode & 0x00FF) ? (UInt16)4 : (UInt16)2;
                    return 6;
                case 0x4000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00) >> 8] != (UInt16)(opcode & 0x00FF) ? (UInt16)4 : (UInt16)2;
                    return 7;
                case 0x5000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00) >> 8] == V[(UInt16)(opcode & 0x00F0) >> 4] ? (UInt16)4 : (UInt16)2;
                    return 8;
                case 0x6000:
                    V[(UInt16)(opcode & 0x0F00) >> 8] = (byte)(opcode & 0x00FF);
                    ProgramCounter += 2;
                    return 9;
                case 0x7000:
                    V[(UInt16)(opcode & 0x0F00) >> 8] += (byte)(opcode & 0x00FF);
                    ProgramCounter += 2;
                    return 10;
                case 0x8000:
                    switch (opcode & 0xF00F)
                    {
                        case 0x8000:
                            V[(UInt16)(opcode & 0x0F00) >> 8] = V[(UInt16)(opcode & 0x00F0) >> 4];
                            ProgramCounter += 2;
                            return 11;
                        case 0x8001:
                            V[(UInt16)(opcode & 0x0F00) >> 8] |= V[(UInt16)(opcode & 0x00F0) >> 4];
                            ProgramCounter += 2;
                            return 12;
                        case 0x8002:
                            V[(UInt16)(opcode & 0x0F00) >> 8] &= V[(UInt16)(opcode & 0x00F0) >> 4];
                            ProgramCounter += 2;
                            return 13;
                        case 0x8003:
                            V[(UInt16)(opcode & 0x0F00) >> 8] ^= V[(UInt16)(opcode & 0x00F0) >> 4];
                            ProgramCounter += 2;
                            return 14;
                        case 0x8004:
                            UInt16 r8004 = (UInt16)(V[(UInt16)(opcode & 0x0F00) >> 8] + V[(UInt16)(opcode & 0x00F0) >> 4]);
                            V[(UInt16)(opcode & 0x0F00) >> 8] = (byte)r8004;
                            V[0xF] = (byte)(r8004 / 256);
                            ProgramCounter += 2;
                            return 15;
                        case 0x8005:
                            UInt16 r8005 = (UInt16)(V[(UInt16)(opcode & 0x0F00) >> 8] - V[(UInt16)(opcode & 0x00F0) >> 4]);
                            V[0xF] = V[(UInt16)(opcode & 0x0F00) >> 8] > V[(UInt16)(opcode & 0x00F0) >> 4] ? (byte)1 : (byte)0;
                            V[(UInt16)(opcode & 0x0F00) >> 8] = (byte)r8005;
                            ProgramCounter += 2;
                            return 16;
                        case 0x8006:
                            V[0xF] = (byte)(V[(UInt16)(opcode & 0x0F00) >> 8] & 1);
                            V[(UInt16)(opcode & 0x0F00) >> 8] >>= 1;
                            ProgramCounter += 2;
                            return 17;
                        case 0x8007:
                            UInt16 r8006 = (UInt16)(V[(UInt16)(opcode & 0x00F0) >> 4] - V[(UInt16)(opcode & 0x0F00) >> 8]);
                            V[0xF] = V[(UInt16)(opcode & 0x0F00) >> 8] > V[(UInt16)(opcode & 0x00F0) >> 4] ? (byte)0 : (byte)1;
                            V[(UInt16)(opcode & 0x0F00) >> 8] = (byte)r8006;
                            ProgramCounter += 2;
                            return 18;
                        case 0x800E:
                            V[0xF] = (byte)(V[(UInt16)(opcode & 0x0F00) >> 8] >> 7);
                            V[(UInt16)(opcode & 0x0F00) >> 8] <<= 1;
                            ProgramCounter += 2;
                            return 19;
                        default:
                            throw new Exception("Undefined instruction");
                    }
                case 0x9000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00) >> 8] != V[(UInt16)(opcode & 0x00F0) >> 4] ? (UInt16)4 : (UInt16)2;
                    return 20;
                case 0xA000:
                    I = (UInt16)(opcode & 0x0FFF);
                    ProgramCounter += 2;
                    return 21;
                case 0xB000:
                    ProgramCounter = (UInt16)(V[0] + (opcode & 0x0FFF));
                    return 22;
                case 0xC000:
                    V[(UInt16)(opcode & 0x0F00) >> 8] = (byte)((UInt16)rand.Next(0, 0xFF + 1) & (UInt16)(opcode & 0x00FF));
                    ProgramCounter += 2;
                    return 23;
                case 0xD000:
                    int dx = V[(UInt16)(opcode & 0x0F00) >> 8];
                    int dy = V[(UInt16)(opcode & 0x00F0) >> 4];
                    int n = (UInt16)(opcode & 0x000F);

                    byte[] pixels = new byte[n];
                    for (int j = I; j < n + I; j++)
                    {
                        pixels[j - I] = MEMORY[j];
                    }

                    V[0xF] = (byte)CurrentFramebuffer.DrawSprite(dx, dy, pixels);
                    ProgramCounter += 2;
                    return 24;
                case 0xE000:
                    switch (opcode & 0xF0FF)
                    {
                        case 0xE09E:
                            ProgramCounter += Input.IsPressed(V[(UInt16)(opcode & 0x0F00) >> 8]) ? (byte)4 : (byte)2;
                            return 25;
                        case 0xE0A1:
                            ProgramCounter += !Input.IsPressed(V[(UInt16)(opcode & 0x0F00) >> 8]) ? (byte)4 : (byte)2;
                            return 26;
                        default:
                            throw new Exception("Undefined instruction");
                    }
                case 0xF000:
                    switch (opcode & 0xF0FF)
                    {
                        case 0xF007:
                            V[(UInt16)(opcode & 0x0F00) >> 8] = DelayTimer;
                            ProgramCounter += 2;
                            return 27;
                        case 0xF00A:
                            ProgramCounter += 2;
                            return 28;
                        case 0xF015:
                            DelayTimer = V[(UInt16)(opcode & 0x0F00) >> 8];
                            ProgramCounter += 2;
                            return 29;
                        case 0xF018:
                            SoundTimer = V[(UInt16)(opcode & 0x0F00) >> 8];
                            ProgramCounter += 2;
                            return 30;
                        case 0xF01E:
                            I += V[(UInt16)(opcode & 0x0F00) >> 8];
                            ProgramCounter += 2;
                            return 31;
                        case 0xF029:
                            I = (byte)(V[(opcode & 0x0F00) >> 8] * 0x5);
                            ProgramCounter += 2;
                            return 32;
                        case 0xF033:
                            int x = V[(UInt16)(opcode & 0x0F00) >> 8];
                            MEMORY[I] = (byte)(x / 100);
                            MEMORY[I + 1] = (byte)((x / 10) % 10);
                            MEMORY[I + 2] = (byte)(x % 10);
                            ProgramCounter += 2;
                            return 33;
                        case 0xF055:
                            for (int fi = 0; fi <= (opcode & 0x0F00) >> 8; fi++)
                                MEMORY[fi + I] = V[fi];
                            ProgramCounter += 2;
                            return 34;
                        case 0xF065:
                            for (int fii = 0; fii <= (opcode & 0x0F00) >> 8; fii++)
                                V[fii] = MEMORY[fii + I];
                            ProgramCounter += 2;
                            return 35;
                        default:
                            throw new Exception("Undefined instruction");
                    }
                default:
                    throw new Exception("Undefined instruction");
            }
        }
    }
}