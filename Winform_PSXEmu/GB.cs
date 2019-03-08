using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
		bool loopEmu = false;
        bool stopEmu = false;
        //TODO: string passing for this object is really bad, fix it.
        string romLocation = "";

        public void StartEmu(string form1RomLocation)
        {
            romLocation = form1RomLocation;
			taskMainLogic = new Task(()=> MainLogic(romLocation));
			if (taskMainLogic.Status == TaskStatus.Running)
			{
				loopEmu = true;
				return;
			}
			else
			{
				loopEmu = true;
				taskMainLogic.Start();
			}
        }

        public void PauseEmu()
        {
			if (loopEmu == true)
			{
				loopEmu = false;
			}
			if (loopEmu == false)
			{
				loopEmu = true;
			}
        }

        public void ResetEmu()
        {
            loopEmu = false;
			taskMainLogic.Wait();
			Reinitialize();
            StartEmu(romLocation);
            
        }

        public void StopEmu()
        {
			loopEmu = false;
            stopEmu = true;
			taskMainLogic.Wait();
			Reinitialize();
        }

        private void MainLogic(string romLocation)
        {
            //TODO: still need that timer control
            //TODO: vet this out and make sure it works as the report objects need to be dropped in here
            BinaryReader rom = new BinaryReader(File.Open(romLocation, FileMode.Open));
            
            for (int i = 0; i < 1;)
            {
                if (loopEmu == true) 
                {
                    count++;
                    byte[] romBytes;
                    ReadROM(rom, out romBytes);
                    Console.WriteLine("Count: " + count.ToString() + " | Status output: PC:" + PC.ToString("X4") + " | SP:" + SP.ToString("X4"));
                    Console.WriteLine(string.Format("CPU Registers High: A:[{0}] | B:[{1}] | D:[{2}] | H:[{3}] | Low: F:[{4}] | C:[{5}] | E:[{6}] | L:[{7}]", 
                                     A.ToString("X2"), B.ToString("X2"), D.ToString("X2"), H.ToString("X2"), 
                                     F.ToString("X2"), C.ToString("X2"), E.ToString("X2"), L.ToString("X2")));
                    Console.WriteLine(" ");
                    CPUDecodeInst(romBytes);
                    //DrawScreen
                    //PlaySound
                }
                else
                {
                    if (stopEmu == true)
                    {
                        rom.Close();
                        return;
                    }
                    Thread.Sleep(10);
                }
            }
        }

        private void ReadROM(BinaryReader rom, out byte[] romBytes) 
        {
            long romLength = rom.BaseStream.Length;
            rom.BaseStream.Position = PC;
            if (romLength - PC < 3)
            {
                int len = (int)romLength - 3;
                romBytes = new byte[len];
                romBytes = rom.ReadBytes(len).ToArray();
            }
            else
            {
                romBytes = new byte[2];
                romBytes = rom.ReadBytes(3).ToArray();
            }
        }
        //the switch case is just to ensure all the algorithm work, well, works.
        //TODO: Replace with proper bit checking to simplify code
        private void CPUDecodeInst(byte[] instBytes)
        {
            UInt16 MemLoc = 0xFF00;
            UInt16 localPC = PC;
            UInt16 Reg16 = 0x0000;
            string mnemonic = instBytes[0].ToString("X2") + " | ";
            byte check = 0;
            switch(instBytes[0])
            {
                #region 0x
				case 0x00:
                    mnemonic += "WIP NOP";
					PC++;
					break;
				case 0x01:
					mnemonic += string.Format("LD BC[{0},{1}], d16[{2},{3}]", L.ToString("X2"), D.ToString("X2"), instBytes[1].ToString("X2"), instBytes[2].ToString("X2"));
					D = LD8(D, instBytes[1]);
					L = LD8(L, instBytes[2]);
					PC += 3;
					break;
				case 0x02:
					mnemonic += string.Format("LD (BC[{0},{1}),A{2}", B.ToString("X2"), C.ToString("X2"), A.ToString("X2"));
                    Reg16 = RegD16(B,C);
					LD8RAM(A, Reg16);
					PC++;
					break;
                case 0x05:
                    mnemonic += string.Format("DEC B[{0}]", B.ToString("X2"));
                    B = DEC(B);
                    PC++;
                    break;
                case 0x06:
					mnemonic += string.Format("LD B[{0}],d8[{1}]", B.ToString("X2"), instBytes[1].ToString("X2"));
					B = LD8(B,instBytes[1]);
                    PC += 2;
                    break;
                case 0x0C:
					mnemonic += string.Format("INC C[{0}]", C.ToString("X2"));
					C = INC8(C);
                    PC++;
                    break;
                case 0x0E:
					mnemonic += string.Format("LD C[{0}], d8[{1}]", C.ToString("X2"),instBytes[1].ToString("X2"));
					C = LD8(C, instBytes[1]);
                    PC += 2;
                    break;
                    #endregion
                #region 1x
                case 0x11:
					mnemonic += string.Format("LD DE[{0},{1}], d16[{2},{3}]", D.ToString("X2"), E.ToString("X2"), instBytes[1].ToString("X2"), instBytes[2].ToString("X2"));
					E = LD8(E,instBytes[1]);
					D = LD8(D,instBytes[2]);
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
                    #endregion
                #region 2x
                case 0x20:
                    mnemonic += string.Format("JR NZ, [{0},{1}]", instBytes[1].ToString("X2"), instBytes[2].ToString("X2"));
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
                case 0x22:
                    mnemonic += string.Format("LD (HL+[{0},{1}]), A[{2}]", H.ToString("X2"), L.ToString("X2"), A.ToString("X2"));
                    //TODO: this also included in the make all HL memory writes their own method thing
                    Reg16 = RegD16(H, L);
                    LD8RAM(A, Reg16);
                    Reg16++;
                    H = (byte)(Reg16 >> 8);
                    L = (byte)(Reg16);
                    PC++;
                    break;
                case 0x23:
                    mnemonic += string.Format("INC HL[{0},{1}]", H.ToString("X2"), L.ToString("X2"));
                    Reg16 = RegD16(H, L);
                    Reg16 = INC16(Reg16);
                    H = (byte)(Reg16 >> 8);
                    L = (byte)(Reg16);
                    PC++;
                    break;
                case 0x26:
					mnemonic += string.Format("LD H[{0}], d8[{1}]", H.ToString("X2"), instBytes[1].ToString("X2"));
					H = LD8(H, instBytes[1]);
                    PC += 2; 
                    break;
                    #endregion
                #region 3x
                case 0x31:
					mnemonic += string.Format("LD SP[{0}], d16[{1},{2}]", SP.ToString("X4"), instBytes[1].ToString("X2"),instBytes[1].ToString("X2"));
                    SP = RegD16(instBytes[2], instBytes[1]);
                    PC += 3;
                    break;
                case 0x32:
					//TODO: Im tired, move this into its own method, somehow, maybe?.
                    mnemonic += string.Format("LD (HL-[{0},{1}]), A[{2}]", H.ToString("X2"), L.ToString("X2"), A.ToString("X2"));
                    Reg16 = RegD16(H, L);
                    LD8RAM(A, Reg16);
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
                #endregion
                #region 4x
                case 0x4F:
                    mnemonic += string.Format("LD C[{0}], A[{1}]", C.ToString("X2"), A.ToString("X2"));
					LD8(C,A);
                    PC++;
                    break;
                #endregion
                #region 5x
                #endregion
                #region 6x
                #endregion
                #region 7x
                case 0x77:
					mnemonic += string.Format("LD (HL[{0},{1}]), A[{2}]", H.ToString("X2"), L.ToString("X2"), A.ToString("X2"));
                    Reg16 = RegD16(H, L);
					LD8RAM(A, Reg16);
                    PC++;
                    break;
                #endregion
                #region 8x
                #endregion
                #region 9x
                #endregion
                #region Ax
                case 0xAF:
                    //TODO: Proper XOR
                    mnemonic += "XOR A";
                    A = 0x0;
                    PC++;
                    break;
                #endregion
                #region Bx
                #endregion
                #region Cx
                case 0xC1:
                    mnemonic += string.Format("POP BC[{0},{1}]", B.ToString("X2"), C.ToString("X2"));
                    B = RAM[SP+1];
                    C = RAM[SP];
                    SP += 2;
                    PC++;
                    break;
                case 0xC3:
                    mnemonic +=  "WIP JP a16";
                    break;
                case 0xC5:
                    mnemonic += "PUSH BC";
                    //correcting this based on pg 278 gb prog manual
                    RAM[SP - 1] = B;
                    RAM[SP - 2] = C;
                    SP -= 2;
                    PC++;
                    break;
                case 0xC9:
                    mnemonic += string.Format("RET");
                    PC = (UInt16)(RAM[SP+1] << 8 | RAM[SP]);
                    SP += 2;
                    break;
                case 0xCB:
                    mnemonic += "look at next byte:";
                    break;
                case 0xCD:
                    mnemonic += string.Format("CALL a16[{0},{1}]", instBytes[1].ToString("X2"), instBytes[2].ToString("X2"));
                    RAM[SP - 1] = (byte)(PC >> 8);
                    RAM[SP - 2] = (byte)(PC); //pg 283 gb prog manual
                    //TODO: can probably take this out and just return it onto PC
                    UInt16 newPC = RegD16(instBytes[2], instBytes[1]);
                    PC = newPC;
                    SP -= 2;
                    break;
                #endregion
                #region Dx
                #endregion
                #region Ex
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
                #endregion
                #region Fx
                #endregion
                #region Invalids
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
                        Console.WriteLine("Attemtping to advance PC by 1 to get back on track...");
                        PC += 1;
                        break;
                    }
                #endregion
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
                    #region CB_0x
                    case 0x11:
                        mnemonic += string.Format("RL C[{0}]", C.ToString("X2"));
                        C = RL(C);
                        PC += 2;
                        break;
                    #endregion
                    #region CB_1x
                    #endregion
                    #region CB_2x
                    #endregion
                    #region CB_3x
                    #endregion
                    #region CB_4x
                    #endregion
                    #region CB_5x
                    #endregion
                    #region CB_6x
                    #endregion
                    #region CB_7x
                    case 0x7C:
                        mnemonic += "BIT 7, H[" + H.ToString("X2") + "]";
                        H = BIT(7, H);
                        PC += 2;
                        break;
                    #endregion
                    #region CB_8x
                    #endregion
                    #region CB_9x
                    #endregion
                    #region CB_Ax
                    #endregion
                    #region CB_Bx
                    #endregion
                    #region CB_Cx
                    #endregion
                    #region CB_Dx
                    #endregion
                    #region CB_Ex
                    #endregion
                    #region CB_Fx
                    #endregion
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
            //RAM[0x8000]-RAM[0x9FFF] is the vram zone

            //8000-87FF	Tile set #1: tiles 0-127
            //8800-8FFF	Tile set #1: tiles 128-255
                        //Tile set #0: tiles -1 to -128
            //9000-97FF	Tile set #0: tiles 0-127
            //9800-9BFF	Tile map #0
            //9C00-9FFF	Tile map #1

            //FE00h-FE9Fh: OAM-RAM (Holds display data for 40 objects)
            //registers SCR0LLX and SCR0LLY are where the upper left corner of the screen are 
            //on the 256,256 background screen buffer
            //http://www.huderlem.com/demos/gameboy2bpp.html
            //http://imrannazar.com/GameBoy-Emulation-in-JavaScript:-GPU-Timings
            color px0 = new color(100,255,255,255); // white
            color px1 = new color(100,192,192,192); //light grey
            color px2 = new color(100,96,96,96); //dark grey
            color px3 = new color(400,0,0,0); //black

            //0xff40 = lcd and gpu control
                //bit 0: BG | off/on
                //bit 1: sprites | off/on
                //bit 2: sprite size | 8x8/8x16
                //bit 3: background tile map | #0/#1
                //bit 4: background tile set | #0/#1
                //bit 5: window on/off | off/on
                //bit 6: window: tile map | #0/#1
                //bit 7: display on/off | off/on
            //0xff42 scroll y
            //0xff43 scroll x
            //0xff44 current scan line (read only)
            //0xff47 background palette (write only)

            //for (int a = 0; a < 256; a++)
            //    for (int b = 0; b < 256; b++)
            //       create background screen buffer
            //for (int y = 0; y < 144; y++)
            //    for (int x = 0; x < 160; x++)
            //        pull from bcb to bitmap output?
            //        hblank on every line, vblank on end of y to start of y


            //so the system works like this as 2bpp
            //byte 1, bit 1 + byte 2, bit 1
                //if both are 1: case 3(black)
                //if the latter is 1: case 2(dark grey)
                //if the former is 1: case 1(light grey)
                //if neither is 1: case 0(white)
            //this determines every single pixel
            //each byte pair is a row

            //public static Bitmap outputDisplay = new Bitmap(640, 480);
            //outputDisplay.SetPixel(row, pixel, Color.FromArgb(100, intRed, intGreen, intBlue));
            //for (int row = 0; row < 640; row++)
            //for (int pixel = 0; pixel < 480; pixel++)
            
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
			A=B=C=D=H=F=C=E=L = 0x00;
			SP=PC = 0x0000;
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
		
		private Byte INC8(Byte Reg)
		{
			byte check = (byte)(F << 1);
            check = (byte)(check >> 7);
            if (check == 1)
            {
                F = (byte)(BitToggle(F, 6, 0));
            }
			return Reg++;
		}
		
        private UInt16 INC16(UInt16 reg16)
        {
            reg16++;
            return reg16;
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
            byte mask = (byte)(1 << p);
            return (byte)((n & ~mask) | ((b << p) & mask));
        }

        private UInt16 RegD16(byte reg1, byte reg2)
        {
            return (UInt16)(reg1 << 8 | reg2);
        }
        #endregion
    }
}


/*notes
 * 
            //http://www.folder101.com/Control/Notes/BitMasking/BitMasking.htm#Using%20XOR%20Masking
            //THIS IS THE ONLY THING THAT EXPLAINS XOR ON THE SAME REGISTER
            //basically zeroes out the accumulator(A register)
            
            //F, in order of the toggle option
            //7 = Zero Flag (z)
            //6 = Subtract Flag (n)
            //5 = Half-Carry (h)
            //4 = Carry (c)
            //rest of bits unused
*/
