using System;
using System.Collections.Generic;
using System.Text;

namespace DotEight
{
    public class CPU
    {
        // Memory
        private byte[] MEMORY = new byte[4096];

        // Registers
        private UInt16 ProgramCounter; // Stores currently executing address
        private Stack<UInt16> AddressStack; // Stack of addresses to return to after subroutine finishes

        private byte[] V;

        private UInt16 I; // Used to store a memory address for some opcodes

        // Screen
        public Framebuffer CurrentFramebuffer { get; set; }

        public CPU()
        {
            ProgramCounter = 0x0200;
            AddressStack = new Stack<UInt16>(16);

            V = new byte[16];

            I = 0;

            CurrentFramebuffer = new Framebuffer();
        }

        public int Execute(UInt16 opcode)
        {
            switch (opcode & 0xF000)
            {
                case 0x0000:
                    switch (opcode)
                    {
                        case 0x00E0:
                            CurrentFramebuffer.ClearScreen();
                            return 1;
                        case 0x00EE:
                            ProgramCounter = AddressStack.Pop();
                            return 2;
                        default: // Calls RCA 1802 program at address NNN. Not necessary for most ROMs.
                            return 3;
                    }
                case 0x1000:
                    ProgramCounter = (UInt16)(opcode & 0x0FFF);
                    return 4;
                case 0x2000:
                    AddressStack.Push(ProgramCounter);
                    ProgramCounter = (UInt16)(opcode & 0x0FFF);
                    return 5;
                case 0x3000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00)] == (UInt16)(opcode & 0x00FF) ? (UInt16)2 : (UInt16)0;
                    return 6;
                case 0x4000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00)] != (UInt16)(opcode & 0x00FF) ? (UInt16)2 : (UInt16)0;
                    return 7;
                case 0x5000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00)] == (UInt16)(opcode & 0x00F0) ? (UInt16)2 : (UInt16)0;
                    return 8;
                case 0x6000:
                    V[(UInt16)(opcode & 0x0F00)] = (byte)(opcode & 0x00FF);
                    return 9;
                case 0x7000:
                    V[(UInt16)(opcode & 0x0F00)] += (byte)(opcode & 0x00FF);
                    return 10;
                case 0x8000:
                    switch (opcode & 0xF00F)
                    {
                        case 0x8000:
                            V[(UInt16)(opcode & 0x0F00)] = V[(UInt16)(opcode & 0x00F0)];
                            return 11;
                        case 0x8001:
                            V[(UInt16)(opcode & 0x0F00)] |= V[(UInt16)(opcode & 0x00F0)];
                            return 12;
                        case 0x8002:
                            V[(UInt16)(opcode & 0x0F00)] &= V[(UInt16)(opcode & 0x00F0)];
                            return 13;
                        case 0x8003:
                            V[(UInt16)(opcode & 0x0F00)] ^= V[(UInt16)(opcode & 0x00F0)];
                            return 14;
                        case 0x8004:
                            UInt16 r8004 = (UInt16)(V[(UInt16)(opcode & 0x0F00)] + V[(UInt16)(opcode & 0x00F0)]);
                            V[(UInt16)(opcode & 0x0F00)] = (byte)r8004;
                            V[0xF] = (byte)(r8004 % 255);
                            return 15;
                        case 0x8005:
                            UInt16 r8005 = (UInt16)(V[(UInt16)(opcode & 0x0F00)] - V[(UInt16)(opcode & 0x00F0)]);
                            V[(UInt16)(opcode & 0x0F00)] = (byte)r8005;
                            V[0xF] = (byte)(r8005 % 255);
                            return 16;
                        case 0x8006:
                            V[(UInt16)(opcode & 0x0F00)] >>= 1;
                            return 17;
                        case 0x8007:
                            UInt16 r8006 = (UInt16)(V[(UInt16)(opcode & 0x00F0)] - V[(UInt16)(opcode & 0x0F00)]);
                            V[(UInt16)(opcode & 0x0F00)] = (byte)r8006;
                            V[0xF] = (byte)(r8006 % 255);
                            return 18;
                        case 0x800E:
                            V[(UInt16)(opcode & 0x0F00)] <<= 1;
                            return 19;
                        default:
                            return 0;
                    }
                case 0x9000:
                    ProgramCounter += V[(UInt16)(opcode & 0x0F00)] != (UInt16)(opcode & 0x00F0) ? (UInt16)2 : (UInt16)0;
                    return 20;
                case 0xA000:
                    I = (UInt16)(opcode & 0x0FFF);
                    return 21;
                case 0xB000:
                    ProgramCounter = (UInt16)(V[0] + (byte)(opcode & 0x0FFF));
                    return 22;
                case 0xC000:
                    V[(UInt16)(opcode & 0x0F00)] = (byte)((UInt16)new Random(255).Next() & (UInt16)(opcode & 0x00FF));
                    return 23;
                case 0xD000:
                    return 24;
                case 0xE000:
                    switch (opcode & 0xF0FF)
                    {
                        case 0xE09E:
                            return 25;
                        case 0xE0A1:
                            return 26;
                        default:
                            return 0;
                    }
                case 0xF000:
                    switch (opcode & 0xF0FF)
                    {
                        case 0xF007:
                            return 27;
                        case 0xF00A:
                            return 28;
                        case 0xF015:
                            return 29;
                        case 0xF018:
                            return 30;
                        case 0xF01E:
                            I += V[(UInt16)(opcode & 0x0F00)];
                            return 31;
                        case 0xF029:
                            return 32;
                        case 0xF033:
                            return 33;
                        case 0xF055:
                            return 34;
                        case 0xF065:
                            return 35;
                        default:
                            return 0;
                    }
                default:
                    return 0;
            }
        }
    }
}