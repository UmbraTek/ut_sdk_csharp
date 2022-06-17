using System;
using utapi.common;
using utapi.flxie;

namespace example.flxie
{
    class Program3
    {
        static void _Main(string[] args)
        {
            FlxiE2ApiSerial flxi = new FlxiE2ApiSerial("COM7", 921600);
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
            Tuple<int, float > ret6 = flxi.get_curr_limit();
            Console.WriteLine(" get_curr_limit  ret: " + ret6.Item1.ToString() + " motion: " + ret6.Item2.ToString());

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
            Tuple<int, float> ret12 = flxi.get_bus_curr();
            Console.WriteLine(" get_bus_curr  ret: " + ret12.Item1.ToString() + " curr: " + ret12.Item2.ToString());
            Tuple<int, int> ret13 = flxi.get_error_code();
            Console.WriteLine(" get_error_code  ret: " + ret13.Item1.ToString() + " error_code: " + ret13.Item2.ToString());

            Tuple<int, float> re13 = flxi.get_pos_limit_min();
            Console.WriteLine(" get_pos_limit_min  ret: " + re13.Item1.ToString() + " limit_min: " + re13.Item2.ToString());
            Tuple<int, float> ret14 = flxi.get_pos_limit_max();
            Console.WriteLine(" get_pos_limit_max  ret: " + ret14.Item1.ToString() + " limit_max: " + ret14.Item2.ToString());
           
            Tuple<int, float> ret17 = flxi.get_tau_limit_min();
            Console.WriteLine(" get_tau_limit_min  ret: " + ret17.Item1.ToString() + " limit_min: " + ret17.Item2.ToString());
            Tuple<int, float> ret18 = flxi.get_tau_limit_max();
            Console.WriteLine(" get_tau_limit_max  ret: " + ret18.Item1.ToString() + " limit_max: " + ret18.Item2.ToString());

            ret18 = flxi.get_pos_target();
            Console.WriteLine(" get_pos_target  ret: " + ret18.Item1.ToString() + " value: " + ret18.Item2.ToString());
            ret18 = flxi.get_pos_current();
            Console.WriteLine(" get_pos_current  ret: " + ret18.Item1.ToString() + " value: " + ret18.Item2.ToString());
            ret18 = flxi.get_tau_target();
            Console.WriteLine(" get_tau_target  ret: " + ret18.Item1.ToString() + " value: " + ret18.Item2.ToString());
            ret18 = flxi.get_tau_current();
            Console.WriteLine(" get_tau_current  ret: " + ret18.Item1.ToString() + " value: " + ret18.Item2.ToString());

            ret9 = flxi.get_pos_pidp();
            Console.WriteLine(" get_pos_pidp  ret: " + ret9.Item1.ToString() + " value: " + ret9.Item2.ToString());
            ret9 = flxi.get_tau_pidp();
            Console.WriteLine(" get_tau_pidp  ret: " + ret9.Item1.ToString() + " value: " + ret9.Item2.ToString());
            ret9 = flxi.get_tau_pidi();
            Console.WriteLine(" get_tau_pidi  ret: " + ret9.Item1.ToString() + " value: " + ret9.Item2.ToString());

            ret13 = flxi.get_pos_smooth_cyc();
            Console.WriteLine(" get_pos_smooth_cyc  ret: " + ret13.Item1.ToString() + " value: " + ret13.Item2.ToString());
            ret13 = flxi.get_tau_smooth_cyc();
            Console.WriteLine(" get_tau_smooth_cyc  ret: " + ret13.Item1.ToString() + " value: " + ret13.Item2.ToString());

        }
    }
}
