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
        private Stack<UInt16> STACK; // Memory address stack
        private UInt16 I; // Stores memory addresses

        private byte V0;
        private byte V1;
        private byte V2;
        private byte V3;
        private byte V4;
        private byte V5;
        private byte V6;
        private byte V7;
        private byte V8;
        private byte V9;
        private byte VA;
        private byte VB;
        private byte VC;
        private byte VD;
        private byte VE;
        private byte VF;

        // Screen
        private Framebuffer framebuffer;

        public CPU()
        {
            V0 = 0;
            V1 = 0;
            V2 = 0;
            V3 = 0;
            V4 = 0;
            V5 = 0;
            V6 = 0;
            V7 = 0;
            V8 = 0;
            V9 = 0;
            VA = 0;
            VB = 0;
            VC = 0;
            VD = 0;
            VE = 0;
            VF = 0;

            framebuffer = new Framebuffer();
        }

        public int Execute(UInt16 opcode)
        {
            switch (opcode & 0xF000)
            {
                case 0x0000:
                    switch (opcode)
                    {
                        case 0x00E0:
                            framebuffer.ClearScreen();
                            return 1;
                        case 0x00EE:
                            return 2;
                        default: // Calls RCA 1802 program at address NNN. Not necessary for most ROMs.
                            return 3;
                    }
                case 0x1000:
                    return 4;
                case 0x2000:
                    return 5;
                case 0x3000:
                    return 6;
                case 0x4000:
                    return 7;
                case 0x5000:
                    return 8;
                case 0x6000:
                    return 9;
                case 0x7000:
                    return 10;
                case 0x8000:
                    switch (opcode & 0xF00F)
                    {
                        case 0x8000:
                            return 11;
                        case 0x8001:
                            return 12;
                        case 0x8002:
                            return 13;
                        case 0x8003:
                            return 14;
                        case 0x8004:
                            return 15;
                        case 0x8005:
                            return 16;
                        case 0x8006:
                            return 17;
                        case 0x8007:
                            return 18;
                        case 0x800E:
                            return 19;
                        default:
                            return 0;
                    }
                case 0x9000:
                    return 20;
                case 0xA000:
                    return 21;
                case 0xB000:
                    return 22;
                case 0xC000:
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