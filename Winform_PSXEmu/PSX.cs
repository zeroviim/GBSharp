using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winform_PSXEmu
{
    class PSX
    {
        //properties
        //array of tk value registers for cpu
        //array for ram
        //pause boolean control
        //stop boolean control
        
        //methods
        //start emulation+kick off task loop
        //pause emulation
        //stop emulation

        //main logic loop
        //
        //cpu pull instruction
            //call write to ram/read from ram to get ~32bit value to put in register
            //gte read if not cpu instruction
        //gpu update
            //update frame buffer
        //write to ram
        //or read from ram
        //spu update
        //mdec update
        //pio output
        //if cd read: cd rom decoder call
        //sio output
        //poll input from player (controller, memory card)

    }
}
