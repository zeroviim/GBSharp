using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform_PSXEmu
{
    public partial class Form1 : Form
    {
        //create psx object on opening of application
        GB GB = new GB();

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            //file picker saves location as string to pass to psx object
            string biosGBLocation = @"C:\Users\Michael\Documents\GB emulation development\bios\[BIOS] Nintendo Game Boy Boot ROM (World).gb";
            string unitTestCPULocation = @"C:\Users\Michael\Documents\GB emulation development\unit tests\cpu_instrs\cpu_instrs.gb";
            //GB.ReadBIOS(biosGBLocation);
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {

        }

        private void btn_formbios_Click(object sender, EventArgs e)
        {
            string biosGBLocation = @"C:\Users\Michael\Documents\GB emulation development\bios\[BIOS] Nintendo Game Boy Boot ROM (World).gb";
            BinaryReader bios = new BinaryReader(File.Open(biosGBLocation, FileMode.Open));
            long biosLength = bios.BaseStream.Length;
            int biosPos = 0;
            while (biosPos < biosLength)
            {
                bios.BaseStream.Position = biosPos;
                byte biosByte = bios.ReadByte();
                lsbx_BIOS.Items.Add(biosByte.ToString("x2").ToUpper());
                biosPos += 1;
            }
            bios.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string romLocation = @"C:\Users\Michael\Documents\GB emulation development\bios\[BIOS] Nintendo Game Boy Boot ROM (World).gb";
            Progress<string> lblA, lblB, lblD, lblH, lblF, lblC, lblE, lblL;
            //high register reporting
            lblA = new Progress<string>(s => lbl_A.Text = s);
            lblB = new Progress<string>(s => lbl_B.Text = s);
            lblD = new Progress<string>(s => lbl_D.Text = s);
            lblH = new Progress<string>(s => lbl_H.Text = s);
            //low register reporting
            lblF = new Progress<string>(s => lbl_F.Text = s);
            lblC = new Progress<string>(s => lbl_C.Text = s);
            lblE = new Progress<string>(s => lbl_E.Text = s);
            lblL = new Progress<string>(s => lbl_L.Text = s);
            GB.StartEmu(romLocation, lblA, lblB, lblD, lblH, lblF, lblC, lblE, lblL);
        }
    }
}
