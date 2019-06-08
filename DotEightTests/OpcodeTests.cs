using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotEight;

namespace DotEightTests
{
    [TestClass]
    public class OpcodeTests
    {
        [TestMethod]
        public void Clear()
        {
            int expected = 1;
            UInt16 opcode = 0x00E0;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Return()
        {
            int expected = 2;
            UInt16 opcode = 0x00EE;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void RCA1802()
        {
            int expected = 3;
            UInt16 opcode = 0x0120;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Jump()
        {
            int expected = 4;
            UInt16 opcode = 0x1698;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Call()
        {
            int expected = 5;
            UInt16 opcode = 0x2340;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipEquals()
        {
            int expected = 6;
            UInt16 opcode = 0x3333;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipNotEquals()
        {
            int expected = 7;
            UInt16 opcode = 0x4444;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipEqualsRegister()
        {
            int expected = 8;
            UInt16 opcode = 0x5460;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetRegister()
        {
            int expected = 9;
            UInt16 opcode = 0x6666;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AddToRegister()
        {
            int expected = 10;
            UInt16 opcode = 0x7777;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void StoreRegister()
        {
            int expected = 11;
            UInt16 opcode = 0x8110;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Or()
        {
            int expected = 12;
            UInt16 opcode = 0x8001;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void And()
        {
            int expected = 13;
            UInt16 opcode = 0x8002;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Xor()
        {
            int expected = 14;
            UInt16 opcode = 0x8003;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AddRegisters()
        {
            int expected = 15;
            UInt16 opcode = 0x8004;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SubtractRegisters()
        {
            int expected = 16;
            UInt16 opcode = 0x8005;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void BitShiftRight()
        {
            int expected = 17;
            UInt16 opcode = 0x8006;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SubtractRegistersReverse()
        {
            int expected = 18;
            UInt16 opcode = 0x8007;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void BitShiftLeft()
        {
            int expected = 19;
            UInt16 opcode = 0x800E;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipNotEqualsRegister()
        {
            int expected = 20;
            UInt16 opcode = 0x9110;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetI()
        {
            int expected = 21;
            UInt16 opcode = 0xAAAA;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void JumpPlusV0()
        {
            int expected = 22;
            UInt16 opcode = 0xBEEE;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Random()
        {
            int expected = 23;
            UInt16 opcode = 0xC123;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Draw()
        {
            int expected = 24;
            UInt16 opcode = 0xD990;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipIfPresent()
        {
            int expected = 25;
            UInt16 opcode = 0xE39E;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SkipIfNotPresent()
        {
            int expected = 26;
            UInt16 opcode = 0xE9A1;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetDelayTimer()
        {
            int expected = 27;
            UInt16 opcode = 0xF207;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetKeyPress()
        {
            int expected = 28;
            UInt16 opcode = 0xFD0A;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetDelayTimer()
        {
            int expected = 29;
            UInt16 opcode = 0xF115;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetSoundTimer()
        {
            int expected = 30;
            UInt16 opcode = 0xF918;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AddToI()
        {
            int expected = 31;
            UInt16 opcode = 0xFF1E;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SetISpriteLocation()
        {
            int expected = 32;
            UInt16 opcode = 0xF629;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void BCD()
        {
            int expected = 33;
            UInt16 opcode = 0xF133;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void DumpRegisters()
        {
            int expected = 34;
            UInt16 opcode = 0xF755;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LoadRegisters()
        {
            int expected = 35;
            UInt16 opcode = 0xF265;
            int result = CPU.Execute(opcode);
            Assert.AreEqual(expected, result);
        }
    }
}
