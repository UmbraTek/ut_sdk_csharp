using System;
using utapi.common;
using utapi.flxiv;

namespace example.flxiv
{
    class Program1
    {
        static void _Main(string[] args)
        {
            FlxiVlApiSerial flxi = new FlxiVlApiSerial("COM7", 921600);
            flxi.connect_to_id(101);

            int ret1 = flxi.set_motion_mode(1);
            Console.WriteLine("set_motion_mode  ret: " + ret1.ToString());
            int ret2 = flxi.set_motion_enable(1);
            Console.WriteLine(" set_motion_enable  ret: " + ret2.ToString());
            System.Threading.Thread.Sleep(5000);

            ret2 = flxi.set_motion_enable(0);
            Console.WriteLine(" set_motion_enable  ret: " + ret2.ToString());
        }
    }
}
