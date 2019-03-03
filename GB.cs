﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;


namespace Winform_PSXEmu
{
    //this is being used for testing emulation development, this is square 1
 /* When the GameBoy is powered up, a 256 byte program
 starting at memory location 0 is executed. This
 program is located in a ROM inside the GameBoy. The
 first thing the program does is read the cartridge
 locations from $104 to $133 and place this graphic of
 a Nintendo logo on the screen at the top. This image
 is then scrolled until it is in the middle of the
 screen. Two musical notes are then played on the
 internal speaker. Again, the cartridge locations $104
 to $133 are read but this time they are compared with
 a table in the internal rom.
 If any byte fails to compare, then the GameBoy stops
 comparing bytes and simply halts all operations.
*/
    class GB
    {
        Byte[] RAM = new Byte[65535];
        Byte A = 0x0;
        Byte B = 0x0;
        Byte D = 0x0;
        Byte H = 0x0;

        Byte F = 0x0;
        Byte C = 0x0;
        Byte E = 0x0;
        Byte L = 0x0;

        UInt16 SP = 0x0000;
        UInt16 PC = 0x0000;
        int count = 0;

        public void StartEmu()
        {

        }

        public void PauseEmu()
        {

        }

        public void StopEmu()
        {

        }

        public void ResetEmu()
        {

        }

        private void MainLogic()
        {
            for (int i = 0; i < 1;)
            {
                //if sleep = false
                //ReadInst
                //DrawScreen
                //PlaySound
                //if sleep = true
                //thread sleep
            }
        }
        public void ReadBIOS(string biosLocation)
        {
            BinaryReader bios = new BinaryReader(File.Open(biosLocation, FileMode.Open));
            long biosLength = bios.BaseStream.Length;
            while (PC < biosLength)
            {
                count++;
                byte[] biosBytes;
                //TODO: Timer to throttle this to proper framerate
                bios.BaseStream.Position = PC;
                if (biosLength - PC < 3)
                {
                    int lenCheck = (int)biosLength - PC;
                    biosBytes = new byte[lenCheck];
                    biosBytes = bios.ReadBytes(lenCheck).ToArray();
                }
                else
                {
                    biosBytes = new byte[2];
                    biosBytes = bios.ReadBytes(3).ToArray();
                }
                Console.WriteLine("Count: " + count.ToString() + "Status output: PC:" + PC.ToString("X2") + " SP:" + SP.ToString("X2"));
                Console.WriteLine("CPU Registers High: A:" + A.ToString("X2")
                                   + "|B:" + B.ToString("X2")
                                   + "|D:" + D.ToString("X2")
                                   + "|H:" + H.ToString("X2")
                                   + "| Low: F:" + F.ToString("X2")
                                   + "|C:" + C.ToString("X2")
                                   + "|E:" + E.ToString("X2")
                                   + "|L:" + L.ToString("X2"));
                Console.WriteLine(" ");
                CPUReadInst(biosBytes);
                //this may be tied to an iprogress object later in order to see what is going on in slow motion on the form controlled by a timer later
                //lsbx_BIOS.Items.Add(biosByte.ToString("x2").ToUpper());
            }
            bios.Close();
            //0XA8 is the start of the scrolling nintendo logo, 0xD7 is the end
        }

        //the switch case is just to ensure all the algorithm work, well, works.
        //TODO: Replace with proper bit checking to simplify code
        private void CPUReadInst(byte[] instBytes)
        {
            UInt16 MemLoc = 0xFF00;
            UInt16 localPC = PC;
            UInt16 Reg16 = 0x0000;
            string mnemonic = instBytes[0].ToString("X2") + " | ";
            byte check = 0;
            switch(instBytes[0])
            {
                case 0x05:
                    mnemonic += string.Format("WIP DEC B[{0}]", B.ToString("X2"));
                    PC += 1;
                    break;
                case 0x06:
                    mnemonic += "LD B[" + B.ToString("X2") + "],d8[" + instBytes[1].ToString("X2") + "]";
                    B = instBytes[1];
                    PC += 2;
                    break;
                case 0x0C:
                    mnemonic += "INC C[" + C.ToString("X2") + "]";
                    C++;
                    check = (byte)(F << 1);
                    check = (byte)(check >> 7);
                    if (check == 1)
                    {
                        F = (byte)(BitToggle(F, 6, 0));
                    }
                    PC++;
                    break;
                case 0x0E:
                    mnemonic += "LD C, d8:[" + instBytes[1].ToString("X2") + "]";
                    C = (byte)(instBytes[1]);
                    PC += 2;
                    break;
                case 0x11:
                    mnemonic += "LD DE[" + D.ToString("X2") + E.ToString("X2") + "]," + instBytes[1].ToString("X2") + instBytes[2].ToString("X2");
                    E = instBytes[1];
                    D = instBytes[2];
                    PC += 3;
                    break;
                case 0x17:
                    mnemonic += string.Format("RLA[{0}]", A.ToString("X2"));
                    A = RL(A);
                    PC += 1;
                    break;
                case 0x1A:
                    mnemonic += "LD A[" + A.ToString("X2") + "],(DE[" + D.ToString("X2") + E.ToString("X2") + "])";
                    Reg16 = RegD16(D, E);
                    RAM[Reg16] = A;
                    PC++;
                    break;
                case 0x20:
                    mnemonic += "JR NZ, [" + instBytes[1].ToString("X4") + "]";
                    check = (byte)(F >> 7);
                    switch(check)
                    {
                        case 0:
                            mnemonic += " False, continuing as normal";
                            PC += 2;
                            break;
                        case 1:
                            SByte signedByte = (SByte)instBytes[1];
                            PC = (UInt16)(PC + 2 + signedByte);
                            mnemonic += " True, jumping to: " + PC.ToString("X4");
                            break;
                    }
                    break;
                case 0x21:
                    mnemonic += "LD HL, d16[" + instBytes[1].ToString("X2") + " " + instBytes[2].ToString("X2") + "]";
                    H = (byte)(instBytes[2]);
                    L = (byte)(instBytes[1]);
                    PC += 3;
                    break;
                case 0x26:
                    mnemonic += "LD H, d8[" + instBytes[1].ToString("X2") + "]";
                    H = (byte)(instBytes[1]);
                    PC += 2; 
                    break;
                case 0x31:
                    mnemonic += "LD SP, d16[" + instBytes[1].ToString("X2") + "," + instBytes[2].ToString("X2")+"]";
                    SP = (UInt16)(instBytes[2] << 8 | instBytes[1]);
                    PC += 3;
                    break;
                case 0x32:
                    mnemonic += "LD (HL-[" + H.ToString("X2") + L.ToString("X2")+ "]), " + "A:" + A.ToString("X2");
                    Reg16 = RegD16(H, L);
                    RAM[Reg16] = A;
                    Reg16--;
                    H = (byte)(Reg16 >> 8);
                    L = (byte)(Reg16);
                    PC += 1;
                    break;
                case 0x3E:
                    mnemonic += "LD A, d8: " + instBytes[1].ToString("X2");
                    A = (byte)instBytes[1];
                    PC += 2;
                    break;
                case 0x4F:
                    mnemonic += "LD C[" + C.ToString("X2") + "],A[" + A.ToString("X2") + "]";
                    C = A;
                    PC++;
                    break;
                case 0x77:
                    mnemonic += "LD (HL[" + H.ToString("X2") + L.ToString("X2") + "]), A[" + A.ToString("X2") + "]";
                    Reg16 = RegD16(H, L);
                    RAM[Reg16] = A;
                    PC++;
                    break;
                case 0xAF:
                    //TODO: Proper XOR
                    mnemonic += "XOR A";
                    A = 0x0;
                    PC++;
                    break;
                case 0xC1:
                    mnemonic += string.Format("POP BC[{0},{1}]", B.ToString("X2"), C.ToString("X2"));
                    B = (byte)(SP >> 8);
                    C = (byte)(SP);
                    SP += 2;
                    PC += 1;
                    break;
                case 0xC3:
                    mnemonic +=  "WIP JP a16";
                    break;
                case 0xC5:
                    mnemonic += "PUSH BC";
                    Reg16 = RegD16(B, C);
                    SP = Reg16;
                    SP -= 2;
                    PC++;
                    break;
                case 0xCB:
                    mnemonic += "look at next byte:";
                    break;
                case 0xCD:
                    mnemonic += "CALL a16[" + instBytes[1].ToString("X2") + instBytes[2].ToString("X2") + "]";
                    PC += 3;
                    RAM[SP - 1] = (byte)(PC >> 8);
                    RAM[SP] = (byte)(PC);
                    UInt16 newPC = RegD16(instBytes[2], instBytes[1]);
                    PC = newPC;
                    SP -= 2;
                    break;
                case 0xE0:
                    mnemonic += "LDH ($FF" + instBytes[1].ToString("X2") + "), A[" + A.ToString("X2") + "]";
                    RAM[MemLoc + instBytes[1]] = A;
                    PC += 2;
                    break;
                case 0xE2:
                    mnemonic += "LD A[" + A.ToString("X2") + "], ($FF" + C.ToString("X2") + ")";
                    RAM[MemLoc + C] = A;
                    PC++;
                    break;

                case 0xDB:
                case 0xDD:
                case 0xE3:
                case 0xE4:
                case 0xEB:
                case 0xEC:
                case 0xED:
                case 0xF4:
                case 0xFC:
                case 0xFD:
                    {
                        Console.WriteLine("invalid instruction in switch for cpu:" + instBytes[0].ToString("X2") + " at location:" + PC.ToString("X4"));
                        PC += 1;
                        break;
                    }
                default:
                    Console.WriteLine("Undefined byte in switch for cpu:" + instBytes[0].ToString("X2") + " at location:" + PC.ToString("X4"));
                    PC += 1;
                    break;
            }
            //prefix swap on CB
            if (instBytes[0] == 0xCB)
            {
                mnemonic += " " + instBytes[1].ToString("X2") + " ";
                switch (instBytes[1])
                {
                    case 0x11:
                        mnemonic += string.Format("RL C[{0}]", C.ToString("X2"));
                        C = RL(C);
                        PC += 2;
                        break;
                    case 0x7C:
                        mnemonic += "BIT 7, H[" + H.ToString("X2") + "]";
                        H = BIT(7, H);
                        PC += 2;
                        break;
                    default:
                        Console.WriteLine("Undefined byte in switch for CB prefix in CPU:" + instBytes[1].ToString("X2") + " at location:" + PC.ToString("X4"));
                        PC += 2;
                        break;
                }
            }
            Console.WriteLine(localPC.ToString("X2") + " | " + mnemonic);
        }

        private void DrawScreen()
        {
            //registers SCR0LLX and SCR0LLY are where the upper left corner of the screen are 
            //on the 256,256 background screen buffer
            //https://realboyemulator.files.wordpress.com/2013/01/gbcpuman.pdf
            //graphic sections have good reads on this when we get to the point where
            //the vram is filled
        }

        private void PlaySound()
        {

        }

        #region RegInstMethods
        private Byte RL(Byte Reg)
        {
            byte oldMSB = (byte)(Reg >> 7); // grab old MSB of reg
            byte newLSB = (byte)(F << 3); //grab LSB of current carry flag
            newLSB = (byte)(newLSB >> 7); //step 2
            Reg = (byte)(Reg << 1); //rotate left everything over 1
            Reg = BitToggle(Reg, 0, newLSB); //add last bit
            if (Reg == 0x00) //if reg = 0, toggle zero flag
            {
                BitToggle(F, 7, 1);
            }
            return Reg;
        }
        
        private Byte BIT(Byte loc, Byte reg)
        {
            byte check = (byte)(reg << loc - 1);
            check = (byte)(reg >> 7);
            check = (byte)(~check);
            return BitToggle(reg, loc, check);
        }

        private Byte DEC(Byte reg)
        {
            //TODO: H - Set if no borrow from bit 4. ????
            BitToggle(F, 6, 1); //set N
            if (reg == 0x00) //if reg = 0, toggle zero flag
            {
                BitToggle(F, 7, 1);
            }
            return reg;
        }
        #endregion
        #region GeneralBitOps
        private Byte BitToggle(byte n, int p, int b) //n: original number, p: position of bit, b: bit value
        {
            //F, in order of the toggle option
            //7 = Zero Flag (z)
            //6 = Subtract Flag (n)
            //5 = Half-Carry (h)
            //4 = Carry (c)
            //rest of bits unused
            byte mask = (byte)(1 << p);
            return (byte)((n & ~mask) | ((b << p) & mask));
        }

        private UInt16 RegD16(byte A, byte B)
        {
            return (UInt16)(A << 8 | B);
        }
        #endregion
    }
}


/*notes
 * 
            //http://www.folder101.com/Control/Notes/BitMasking/BitMasking.htm#Using%20XOR%20Masking
            //THIS IS THE ONLY THING THAT EXPLAINS XOR ON THE SAME REGISTER
            //basically zeroes out the accumulator(A register)
*/