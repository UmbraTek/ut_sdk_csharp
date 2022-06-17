using System;
using utapi.adra;
using utapi.common;

namespace example.adra
{
    class Program2
    {
        static void _Main(string[] args)
        {
            AdraApiSerial adra = new AdraApiSerial("COM7", 921600); //  # instantiate the adra executor api class
            adra.connect_to_id(1); //'  # The ID of the connected target actuator, where the ID is 1

            int ret = adra.set_motion_mode(2); //  # Set actuator motion mode 1: position mode
            Console.WriteLine("set_motion_mode ret: " + ret.ToString());
            System.Threading.Thread.Sleep(500);
            ret = adra.set_motion_enable(1); //  # Enable actuator
            Console.WriteLine("set_motion_enable ret: " + ret.ToString());
            System.Threading.Thread.Sleep(500);
            ret = adra.set_vel_target(50); //  # Set the actuator to move to a position of 50 radians
            Console.WriteLine("set_vel_target ret: " + ret.ToString());
            System.Threading.Thread.Sleep(500);
            Tuple<int, float> rets = adra.get_vel_target(); //  # Set the actuator to move to a position of 50 radians
            Console.WriteLine("set_vel_target ret: " + rets.Item2.ToString());
            System.Threading.Thread.Sleep(500);
            Tuple<int, float> rets1 = adra.get_vel_current(); //  # Set the actuator to move to a position of 50 radians
            Console.WriteLine("set_vel_target ret: " + rets1.Item2.ToString());
            System.Threading.Thread.Sleep(500);
            adra.set_motion_enable(0);
            // ret = adra.set_motion_enable(0);
            // ret = adra.set_pos_target(-50); //  # Set the actuator to move to -50 rad
            // Console.WriteLine("set_pos_target ret: " + ret.ToString());
        }
    }
}
