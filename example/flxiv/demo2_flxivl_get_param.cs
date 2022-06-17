using System;
using utapi.common;
using utapi.flxiv;

namespace example.flxiv
{
    class Program2
    {
        static void _Main(string[] args)
        {
            FlxiVlApiSerial flxi = new FlxiVlApiSerial("COM7", 921600);
            flxi.connect_to_id(101);

            Tuple<int, String> ret1 = flxi.get_uuid();
            Console.WriteLine(" get uuid  ret: " + ret1.Item1.ToString() + " uuid: " + ret1.Item2);
            Tuple<int, String> ret2 = flxi.get_sw_version();
            Console.WriteLine(" get_sw_version  ret: " + ret2.Item1.ToString() + " sw_version: " + ret2.Item2);
            Tuple<int, String> ret3 = flxi.get_hw_version();
            Console.WriteLine(" get_hw_version  ret: " + ret3.Item1.ToString() + " hw_version: " + ret3.Item2);

            Tuple<int, int, int> ret4 = flxi.get_temp_limit();
            Console.WriteLine(" get_temp_limit  ret: " + ret4.Item1.ToString() + " min: " + ret4.Item2.ToString()+ " max: " + ret4.Item3.ToString());
            Tuple<int, int, int> ret5 = flxi.get_volt_limit();
            Console.WriteLine(" get_volt_limit  ret: " + ret5.Item1.ToString() + " min: " + ret5.Item2.ToString()+ " max: " + ret4.Item3.ToString());
           

            Tuple<int, int> ret7 = flxi.get_motion_mode();
            Console.WriteLine(" get_motion_mode  ret: " + ret7.Item1.ToString() + " mode: " + ret7.Item2.ToString());
            Tuple<int, int> ret8 = flxi.get_motion_enable();
            Console.WriteLine(" get_motion_enable  ret: " + ret8.Item1.ToString() + " enable:  " + ret8.Item2.ToString() );
            Tuple<int, float> ret9 = flxi.get_temp_driver();
            Console.WriteLine(" get_temp_driver  ret: " + ret9.Item1.ToString() + " driver: " + ret9.Item2);
            Tuple<int, float> ret10 = flxi.get_temp_motor();
            Console.WriteLine(" get_temp_motor  ret: " + ret10.Item1.ToString() + " motor: " + ret10.Item2.ToString());
            Tuple<int, float> ret11 = flxi.get_bus_volt();
            Console.WriteLine(" get_bus_volt  ret: " + ret11.Item1.ToString() + " volt: " + ret11.Item2.ToString());
            Tuple<int, int> ret13 = flxi.get_error_code();
            Console.WriteLine(" get_error_code  ret: " + ret13.Item1.ToString() + " error_code: " + ret13.Item2.ToString());

        }
    }
}
