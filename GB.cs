using System;
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
		//GB components
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
		//personal use instruction counter
        int count = 0;
		
		//class components
		Task taskMainLogic;
		bool loop = false;
		
        public void StartEmu()
        {
			MainLogicTask = new Task(()=> MainLogic());
			if (MainLogicTask == TaskStatus.Running)
			{
				Loop = true;
				return;
			}
			else
			{
				Loop = true;
				MainLogicTask.Start();
			}
        }

        public void PauseEmu()
        {
			if (Loop == true)
			{
				Loop = false;
			}
			if (Loop == false)
			{
				Loop = true;
			}
        }

        public void StopEmu()
        {
			MainLogicTask.Stop();
			Reinitialize();
			MainLogicTask = new Task(()=> MainLogic());
			MainLogicTask.Start();
        }

        public void ResetEmu()
        {
			Loop = false;
			MainLogicTask.Stop();
			Reinitialize();
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
                    B = DEC(B);
                    PC += 1;
                    break;
                case 0x06:
					mnemonic += string.Format("LD B[{0}],d8[{1}]", B.ToString("X2"), instBytes[1].ToString("X2"));
					B = LD8(B,instBytes[1]);
                    PC += 2;
                    break;
                case 0x0C:
					mnemonic += string.Format("INC C[{0}]", C.ToString("X2"));
					C = INC(C);
                    PC++;
                    break;
                case 0x0E:
					mnemonic += string.Format("LD C[{0}], d8[{1}]", C.ToString("X2"),instBytes[1].ToString("X2"));
					C = LD(C, instBytes[1]);
                    PC += 2;
                    break;
                case 0x11:
					mnemonic += string.Format("LD DE[{0},{1}],{2},{3}", D.ToString("X2"), E.ToString("X2"), instBytes[1].ToString("X2"), instBytes[2].ToString("X2"));
					E = LD(E,instBytes[1]);
					D = LD(D,instBytes[2]);
                    PC += 3;
                    break;
                case 0x17:
                    mnemonic += string.Format("RLA[{0}]", A.ToString("X2"));
                    A = RL(A);
                    PC += 1;
                    break;
                case 0x1A:
					mnemonic += string.Format("LD A[{0}], (DE[{1},{2}])", A.ToString("X2"), D.ToString("X2"), E.ToString("X2"));
                    Reg16 = RegD16(D, E);
					LD8RAM(A, Reg16);
                    PC++;
                    break;
                case 0x20:
					mnemonic += string.Format("JR NZ, [{1},{2}]", instBytes[].ToString("X2"), instBytes[2].ToString("X2"))
					//TODO: Did this work originally??? Review Logging...
                    //mnemonic += "JR NZ, [" + instBytes[1].ToString("X4") + "]";
					//TODO: debating shoving this in a method of its own or not, do want to but may need to work more in jump instructions before I do
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
                            mnemonic += string.Format(" True, jumping to: {0}", PC.ToString("X4"));
                            break;
                    }
                    break;
                case 0x21:
                    mnemonic += string.Format("LD HL[{0},{1}], d16[{2},{3}]", H.ToString("X2"), L.ToString("X2"), instBytes[1].ToString("X2"), instBytes[2].ToString("X2"));
					H = LD8(H,instBytes[2]);
					L = LD8(L,instBytes[1]);
                    PC += 3;
                    break;
                case 0x26:
					mnemonic += string.Format("LD H[{0}], d8[{1}]", H.ToString("X2"), instBytes[1].ToString("X2"));
					H = LD8(H, instBytes[1]);
                    PC += 2; 
                    break;
                case 0x31:
					mnemonic += string.Format("LD SP[{0}], d16[{1},{2}]", SP.ToString("X4"), instBytes[1].ToString("X2"),instBytes[1].ToString("X2"));
					SP = RegD16(instBytes[2], instBytes[1])
                    PC += 3;
                    break;
                case 0x32:
					//TODO: Im tired, move this into its own method, somehow, maybe?.
                    mnemonic += string.Format("LD (HL-[{0},{1}]), A[{2}]", H.ToString("X2"), L.ToString("X2"), A.ToString("X2"));
                    Reg16 = RegD16(H, L);
					LD8RAM(A, Reg16)
                    Reg16--;
                    H = (byte)(Reg16 >> 8);
                    L = (byte)(Reg16);
                    PC++;
                    break;
                case 0x3E:
                    mnemonic += string.Format("LD A[{0}], d8[{1}]", A.ToString("X2"), instBytes[1].ToString("X2"));
					A = LD8(A, instBytes[1]);
                    PC += 2;
                    break;
                case 0x4F:
                    mnemonic += string.Format("LD C[{0}], A[{1}]", C.ToString("X2"), A.ToString("X2"));
					LD8(C,A);
                    PC++;
                    break;
                case 0x77:
					mnemonic += string.Format("LD (HL[{0},{1}]), A[{2}]", H.ToString("X2"), L.ToString("X2"), A.ToString("X2"));
                    Reg16 = RegD16(H, L);
					LD8RAM(A, Reg16);
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
		
		private void Reinitialize()
		{
			for (int i = 0; i < 65535; i++) 
			{
				RAM[i] = 0x00;
			}
			A,B,C,D,H,F,C,E,L = 0x00;
			SP,PC = 0x0000;
		}
		
        #region RegInstMethods
		private Byte BIT(Byte loc, Byte reg)
        {
            byte check = (byte)(reg << loc - 1);
            check = (byte)(reg >> 7);
            check = (byte)(~check);
            return BitToggle(reg, loc, check);
        }

        private Byte DEC(Byte reg)
        {
			if ((byte)(reg & 0x0F) != 0) //check if not borrowing from upper nybble to set H
			{
				BitToggle(F, 5, 1);
			}
			//TODO: Verify that H doesn't need to be reset if a carry does happen
            reg--; //decrease reg
            BitToggle(F, 6, 1); //set N
            if (reg == 0x00) //if reg = 0, toggle zero flag
            {
                BitToggle(F, 7, 1);
            }
            return reg;
        }
		
		private Byte INC(Byte Reg)
		{
			check = (byte)(F << 1);
            check = (byte)(check >> 7);
            if (check == 1)
            {
                F = (byte)(BitToggle(F, 6, 0));
            }
			return Reg++;
		}
		
		private Byte LD8(Byte Reg, Byte d8) 
		{
			return Reg = d8;
		}
		
		private void LD8RAM(Byte Reg, UInt16 Reg2)
		{
			RAM[Reg2] = Reg;
		}

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
