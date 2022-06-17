using System;
using utapi.adra;
using utapi.common;

namespace example.adra
{
    class Program1
    {
        static void _Main(string[] args)
        {
            AdraApiSerial adra = new AdraApiSerial("COM7", 921600); //  # instantiate the adra executor api class
            //AdraApiUdp adra = new AdraApiUdp("192.168.1.165");
            adra.connect_to_id(1); //'  # The ID of the connected target actuator, where the ID is 1

            int ret = adra.set_motion_mode(1); //  # Set actuator motion mode 1: position mode
            Console.WriteLine("set_motion_mode ret: " + ret.ToString());
            ret = adra.set_motion_enable(1); //  # Enable actuator
            Console.WriteLine("set_motion_enable ret: " + ret.ToString());
            ret = adra.set_pos_target(50); //  # Set the actuator to move to a position of 50 radians
            Console.WriteLine("set_pos_target ret: " + ret.ToString());
            System.Threading.Thread.Sleep(3000);
            ret = adra.set_pos_target(-50); //  # Set the actuator to move to -50 rad
            Console.WriteLine("set_pos_target ret: " + ret.ToString());
        }
    }
}
