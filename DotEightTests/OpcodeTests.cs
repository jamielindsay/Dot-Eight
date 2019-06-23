using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotEight;
using SFML;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace DotEightTests
{
    [TestClass]
    public class OpcodeTests
    {
        private CPU cpu = new CPU();

        [TestMethod]
        public void Clear()
        {
            cpu.Execute(0x00E0);
            foreach (RectangleShape pixel in cpu.CurrentFramebuffer.Frame)
            {
                Assert.AreEqual(pixel.FillColor, new Color(0, 0, 0));
            }
        }

        [TestMethod]
        public void Return()
        {
            UInt16 expected = 0x0469;
            cpu.AddressStack.Push(expected);
            cpu.Execute(0x00EE);
            Assert.AreEqual(expected, cpu.ProgramCounter);
        }

        [TestMethod]
        public void RCA1802()
        {
            int expected = 3;
            UInt16 opcode = 0x0120;
            int result = cpu.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Jump()
        {
            cpu.Execute(0x1698);
            Assert.AreEqual(0x0698, cpu.ProgramCounter);
        }

        [TestMethod]
        public void Call()
        {
            UInt16 expected = 0x222;
            cpu.ProgramCounter = expected;
            cpu.Execute(0x2340);
            Assert.AreEqual(expected, cpu.AddressStack.Pop());
            Assert.AreEqual(0x0340, cpu.ProgramCounter);
        }

        [TestMethod]
        public void SkipEquals()
        {
            cpu.V[4] = 0x33;
            cpu.ProgramCounter = 0x202;
            cpu.Execute(0x3433);
            UInt16 expected = 0x202 + 2;
            Assert.AreEqual(expected, cpu.ProgramCounter);
        }

        [TestMethod]
        public void SkipNotEquals()
        {
            cpu.V[5] = 0x33;
            cpu.ProgramCounter = 0x202;
            cpu.Execute(0x4544);
            UInt16 expected = 0x202 + 2;
            Assert.AreEqual(expected, cpu.ProgramCounter);
        }

        [TestMethod]
        public void SkipEqualsRegister()
        {
            cpu.V[4] = 0x33;
            cpu.V[5] = 0x33;
            cpu.ProgramCounter = 0x202;
            cpu.Execute(0x5450);
            UInt16 expected = 0x202 + 2;
            Assert.AreEqual(expected, cpu.ProgramCounter);
        }

        [TestMethod]
        public void SetRegister()
        {
            cpu.V[4] = 0x30;
            cpu.Execute(0x6433);
            Assert.AreEqual(0x33, cpu.V[4]);
        }

        [TestMethod]
        public void AddToRegister()
        {
            cpu.V[4] = 0x30;
            cpu.Execute(0x7403);
            Assert.AreEqual(0x30 + 0x3, cpu.V[4]);
        }

        [TestMethod]
        public void StoreRegister()
        {
            cpu.V[4] = 0x30;
            cpu.V[5] = 0x33;
            cpu.Execute(0x8450);
            Assert.AreEqual(0x33, cpu.V[4]);
        }

        [TestMethod]
        public void Or()
        {
            cpu.V[4] = 0x30;
            cpu.V[5] = 0x33;
            cpu.Execute(0x8451);
            Assert.AreEqual(0x33, cpu.V[4]);
        }

        [TestMethod]
        public void And()
        {
            cpu.V[4] = 0x30;
            cpu.V[5] = 0x33;
            cpu.Execute(0x8452);
            Assert.AreEqual(0x30, cpu.V[4]);
        }

        [TestMethod]
        public void Xor()
        {
            cpu.V[4] = 0x30;
            cpu.V[5] = 0x33;
            cpu.Execute(0x8453);
            Assert.AreEqual(0x3, cpu.V[4]);
        }

        [TestMethod]
        public void AddRegisters()
        {
            cpu.V[4] = 255;
            cpu.V[5] = 255;
            cpu.Execute(0x8454);
            Assert.AreEqual(254, cpu.V[4]);
            Assert.AreEqual(1, cpu.V[0xF]);
        }

        [TestMethod]
        public void SubtractRegisters()
        {
            cpu.V[4] = 255;
            cpu.V[5] = 255;
            cpu.Execute(0x8455);
            Assert.AreEqual(0, cpu.V[4]);
            Assert.AreEqual(1, cpu.V[0xF]);
        }

        [TestMethod]
        public void BitShiftRight()
        {
            cpu.V[4] = 0b1011;
            cpu.Execute(0x8456);
            Assert.AreEqual(0b0101, cpu.V[4]);
            Assert.AreEqual(1, cpu.V[0xF]);
        }

        [TestMethod]
        public void SubtractRegistersReverse()
        {
            cpu.V[4] = 255;
            cpu.V[5] = 254;
            cpu.Execute(0x8457);
            Assert.AreEqual(255, cpu.V[4]);
            Assert.AreEqual(0, cpu.V[0xF]);
        }

        [TestMethod]
        public void BitShiftLeft()
        {
            cpu.V[4] = 0b1010_1011;
            cpu.Execute(0x845E);
            Assert.AreEqual(0b0101_0110, cpu.V[4]);
            Assert.AreEqual(1, cpu.V[0xF]);
        }

        [TestMethod]
        public void SkipNotEqualsRegister()
        {
            cpu.V[4] = 0x30;
            cpu.V[5] = 0x33;
            cpu.ProgramCounter = 0x202;
            cpu.Execute(0x9450);
            UInt16 expected = 0x202 + 2;
            Assert.AreEqual(expected, cpu.ProgramCounter);
        }

        [TestMethod]
        public void SetI()
        {
            cpu.Execute(0xA330);
            Assert.AreEqual(0x330, cpu.I);
        }

        [TestMethod]
        public void JumpPlusV0()
        {
            cpu.ProgramCounter = 0x202;
            cpu.V[0] = 0x20;
            cpu.Execute(0xB222);
            Assert.AreEqual(0x242, cpu.ProgramCounter);
        }

        [TestMethod]
        public void Random()
        {
            int expected = 23;
            UInt16 opcode = 0xC123;
            int result = cpu.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Draw()
        {
            int expected = 24;
            UInt16 opcode = 0xD990;
            int result = cpu.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipIfPresent()
        {
            int expected = 25;
            UInt16 opcode = 0xE39E;
            int result = cpu.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipIfNotPresent()
        {
            int expected = 26;
            UInt16 opcode = 0xE9A1;
            int result = cpu.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetDelayTimer()
        {
            cpu.DelayTimer = 0x69;
            cpu.Execute(0xF407);
            Assert.AreEqual(0x69, cpu.V[4]);
        }

        [TestMethod]
        public void GetKeyPress()
        {
            int expected = 28;
            UInt16 opcode = 0xFD0A;
            int result = cpu.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetDelayTimer()
        {
            cpu.V[4] = 0x11;
            cpu.Execute(0xF415);
            Assert.AreEqual(0x11, cpu.DelayTimer);
        }

        [TestMethod]
        public void SetSoundTimer()
        {
            cpu.V[4] = 0x11;
            cpu.Execute(0xF418);
            Assert.AreEqual(0x11, cpu.SoundTimer);
        }

        [TestMethod]
        public void AddToI()
        {
            cpu.I = 0x200;
            cpu.V[4] = 0x11;
            cpu.Execute(0xF41E);
            Assert.AreEqual(0x211, cpu.I);
        }

        [TestMethod]
        public void SetISpriteLocation()
        {
            int expected = 32;
            UInt16 opcode = 0xF629;
            int result = cpu.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void BCD()
        {
            cpu.I = 0x202;
            cpu.V[2] = 254;
            cpu.Execute(0xF233);
            Assert.AreEqual(2, cpu.Memory[cpu.I]);
            Assert.AreEqual(5, cpu.Memory[cpu.I + 1]);
            Assert.AreEqual(4, cpu.Memory[cpu.I + 2]);
        }

        [TestMethod]
        public void DumpRegisters()
        {
            cpu.I = 0x200;
            cpu.V[0] = 0x01;
            cpu.V[1] = 0x10;
            cpu.V[2] = 0x02;
            cpu.V[3] = 0x20;
            cpu.Execute(0xF355);
            Assert.AreEqual(0x01, cpu.Memory[cpu.I]);
            Assert.AreEqual(0x10, cpu.Memory[cpu.I + 1]);
            Assert.AreEqual(0x02, cpu.Memory[cpu.I + 2]);
            Assert.AreEqual(0x20, cpu.Memory[cpu.I + 3]);
        }

        [TestMethod]
        public void LoadRegisters()
        {
            cpu.I = 0x200;
            cpu.Memory[cpu.I] = 0x01;
            cpu.Memory[cpu.I + 1] = 0x02;
            cpu.Memory[cpu.I + 2] = 0x03;
            cpu.Memory[cpu.I + 3] = 0x04;
            cpu.Execute(0xF365);
            Assert.AreEqual(0x01, cpu.V[0]);
            Assert.AreEqual(0x02, cpu.V[1]);
            Assert.AreEqual(0x03, cpu.V[2]);
            Assert.AreEqual(0x04, cpu.V[3]);
        }
    }
}
