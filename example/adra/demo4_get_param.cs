using System;
using utapi.adra;
using utapi.common;

namespace example.adra
{
    class Program4
    {
        static void _Main(string[] args)
        {
            AdraApiSerial adra = new AdraApiSerial("COM11", 921600); //  # instantiate the adra executor api class
            //AdraApiUdp adra = new AdraApiUdp("192.168.1.165");
            adra.connect_to_id(1); //'  # The ID of the connected target actuator, where the ID is 1

            Tuple<int, String> ret1 = adra.get_uuid(); //  # Set actuator motion mode 1: position mode
            Console.WriteLine("get_uuid ret: " + ret1.Item2);
            ret1 = adra.get_sw_version(); //  # Set actuator motion mode 1: position mode
            Console.WriteLine("get_sw_version ret: " + ret1.Item2);
            ret1 = adra.get_hw_version(); //  # Set actuator motion mode 1: position mode
            Console.WriteLine("get_hw_version ret: " + ret1.Item2);
            ret1 = adra.get_multi_version(); //  # Set actuator motion mode 1: position mode
            Console.WriteLine("get_multi_version ret: " + ret1.Item2);

            Tuple<int, float> ret2 = adra.get_mech_ratio();
            Console.WriteLine("get_mech_ratio ret: " + ret2.Item2.ToString());
            ret2 = adra.get_elec_ratio();
            Console.WriteLine("get_elec_ratio ret: " + ret2.Item2.ToString());
            Tuple<int, int> ret3 = adra.get_motion_dir();
            Console.WriteLine("get_motion_dir ret: " + ret3.Item2.ToString());
            //adra.set_iwdg_cyc(2);
            Tuple<int, int> ret_3 = adra.get_iwdg_cyc();
            Console.WriteLine("get_iwdg_cyc ret: " + ret_3.Item2.ToString());
            Tuple<int, int, int> ret4 = adra.get_temp_limit();
            Console.WriteLine("get_temp_limit min: " + ret4.Item2.ToString() + " max : " + ret4.Item3.ToString());
            ret4 = adra.get_volt_limit();
            Console.WriteLine("get_volt_limit min: " + ret4.Item2.ToString() + " max : " + ret4.Item3.ToString());
            ret2 = adra.get_curr_limit();
            Console.WriteLine("get_curr_limit ret: " + ret2.Item2.ToString());

            ret3 = adra.get_motion_mode();
            Console.WriteLine("get_motion_mode ret: " + ret3.Item2.ToString());
            ret3 = adra.get_motion_enable();
            Console.WriteLine("get_motion_enable ret: " + ret3.Item2.ToString());
            ret3 = adra.get_brake_enable();
            Console.WriteLine("get_brake_enable ret: " + ret3.Item2.ToString());
            ret2 = adra.get_temp_driver();
            Console.WriteLine("get_temp_driver ret: " + ret2.Item2.ToString());
            ret2 = adra.get_temp_motor();
            Console.WriteLine("get_temp_motor ret: " + ret2.Item2.ToString());
            ret2 = adra.get_bus_volt();
            Console.WriteLine("get_bus_volt ret: " + ret2.Item2.ToString());
            ret2 = adra.get_bus_curr();
            Console.WriteLine("get_bus_curr ret: " + ret2.Item2.ToString());
            ret2 = adra.get_multi_volt();
            Console.WriteLine("get_multi_volt ret: " + ret2.Item2.ToString());
            ret3 = adra.get_error_code();
            Console.WriteLine("get_error_code ret: " + ret3.Item2.ToString());

            ret2 = adra.get_pos_limit_min();
            Console.WriteLine("get_pos_limit_min ret: " + ret2.Item2.ToString());
            ret2 = adra.get_pos_limit_max();
            Console.WriteLine("get_pos_limit_max ret: " + ret2.Item2.ToString());
            ret2 = adra.get_pos_limit_diff();
            Console.WriteLine("get_pos_limit_diff ret: " + ret2.Item2.ToString());
            ret2 = adra.get_vel_limit_min();
            Console.WriteLine("get_vel_limit_min ret: " + ret2.Item2.ToString());
            ret2 = adra.get_vel_limit_max();
            Console.WriteLine("get_vel_limit_max ret: " + ret2.Item2.ToString());
            ret2 = adra.get_vel_limit_diff();
            Console.WriteLine("get_vel_limit_diff ret: " + ret2.Item2.ToString());
            ret2 = adra.get_tau_limit_min();
            Console.WriteLine("get_tau_limit_min ret: " + ret2.Item2.ToString());
            ret2 = adra.get_tau_limit_max();
            Console.WriteLine("get_tau_limit_max ret: " + ret2.Item2.ToString());
            ret2 = adra.get_tau_limit_diff();
            Console.WriteLine("get_tau_limit_diff ret: " + ret2.Item2.ToString());

            ret2 = adra.get_pos_target();
            Console.WriteLine("get_pos_target ret: " + ret2.Item2.ToString());
            ret2 = adra.get_pos_current();
            Console.WriteLine("get_pos_current ret: " + ret2.Item2.ToString());
            ret2 = adra.get_vel_target();
            Console.WriteLine("get_vel_target ret: " + ret2.Item2.ToString());
            ret2 = adra.get_vel_current();
            Console.WriteLine("get_vel_current ret: " + ret2.Item2.ToString());
            ret2 = adra.get_tau_target();
            Console.WriteLine("get_tau_target ret: " + ret2.Item2.ToString());
            ret2 = adra.get_tau_current();
            Console.WriteLine("get_tau_current ret: " + ret2.Item2.ToString());

            ret2 = adra.get_pos_pidp();
            Console.WriteLine("get_pos_pidp ret: " + ret2.Item2.ToString());
            ret2 = adra.get_vel_pidp();
            Console.WriteLine("get_vel_pidp ret: " + ret2.Item2.ToString());
            ret2 = adra.get_vel_pidi();
            Console.WriteLine("get_vel_pidi ret: " + ret2.Item2.ToString());
            ret2 = adra.get_tau_pidp();
            Console.WriteLine("get_tau_pidp ret: " + ret2.Item2.ToString());
            ret2 = adra.get_tau_pidi();
            Console.WriteLine("get_tau_pidi ret: " + ret2.Item2.ToString());

            ret3 = adra.get_pos_smooth_cyc();
            Console.WriteLine("get_pos_smooth_cyc ret: " + ret3.Item2.ToString());
            ret3 = adra.get_vel_smooth_cyc();
            Console.WriteLine("get_vel_smooth_cyc ret: " + ret3.Item2.ToString());
            ret3 = adra.get_tau_smooth_cyc();
            Console.WriteLine("get_tau_smooth_cyc ret: " + ret3.Item2.ToString());
        }
    }
}
