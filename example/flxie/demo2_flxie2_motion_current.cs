using System;
using utapi.common;
using utapi.flxie;

namespace example.flxie
{
    class Program2
    {
        static void _Main(string[] args)
        {
            FlxiE2ApiSerial flxi = new FlxiE2ApiSerial("COM7", 921600);
            flxi.connect_to_id(101);

            int ret1 = flxi.set_motion_mode(3);
            Console.WriteLine("set_motion_mode  ret: " + ret1.ToString());
            int ret2 = flxi.set_motion_enable(1);
            Console.WriteLine(" set_motion_enable  ret: " + ret2.ToString());
            int ret3 = flxi.set_tau_target(0.8f);
            Console.WriteLine(" set_tau_target  ret: " + ret3.ToString());
            System.Threading.Thread.Sleep(5000);

        }
    }
}
