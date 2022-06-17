using System;
using utapi.common;
using utapi.utra;

namespace example.utra
{
    class Program2
    {
        static void _Main(string[] args)
        {
            UtraApiTcp ubot = new UtraApiTcp("192.168.1.90");

            Tuple<int, String> ret1 = ubot.get_uuid();
            Console.WriteLine("[UbotApi ] get uuid  ret: " + ret1.Item1.ToString() + " uuid: " + ret1.Item2);
            Tuple<int, String> ret2 = ubot.get_sw_version();
            Console.WriteLine("[UbotApi ] get_sw_version  ret: " + ret2.Item1.ToString() + " sw_version: " + ret2.Item2);
            Tuple<int, String> ret3 = ubot.get_hw_version();
            Console.WriteLine("[UbotApi ] get_hw_version  ret: " + ret3.Item1.ToString() + " hw_version: " + ret3.Item2);
            Tuple<int, int> ret4 = ubot.get_axis();
            Console.WriteLine("[UbotApi ] get_axis  ret: " + ret4.Item1.ToString() + " axis: " + ret4.Item2.ToString());
            Tuple<int, int> ret5 = ubot.get_motion_mode();
            Console.WriteLine("[UbotApi ] get_motion_mode  ret: " + ret5.Item1.ToString() + " mode: " + ret5.Item2.ToString());
            Tuple<int, uint> ret6 = ubot.get_motion_enable();
            Console.WriteLine("[UbotApi ] get_motion_enable  ret: " + ret6.Item1.ToString() + " motion: " + ret6.Item2.ToString());
            Tuple<int, uint> ret7 = ubot.get_brake_enable();
            Console.WriteLine("[UbotApi ] get_brake_enable  ret: " + ret7.Item1.ToString() + " brake: " + ret7.Item2.ToString());
            Tuple<int, int, int> ret8 = ubot.get_error_code();
            Console.WriteLine("[UbotApi ] get_error_code  ret: " + ret8.Item1.ToString() + "  " + ret8.Item2.ToString() + "  " + ret8.Item3.ToString());
            Tuple<int, String> ret9 = ubot.get_servo_msg();
            Console.WriteLine("[UbotApi ] get_servo_msg  ret: " + ret9.Item1.ToString() + " servo_msg: " + ret9.Item2);
            Tuple<int, int> ret10 = ubot.get_motion_status();
            Console.WriteLine("[UbotApi ] get_motion_status  ret: " + ret10.Item1.ToString() + " status: " + ret10.Item2.ToString());
            Tuple<int, uint> ret11 = ubot.get_cmd_num();
            Console.WriteLine("[UbotApi ] get_cmd_num  ret: " + ret11.Item1.ToString() + " num: " + ret11.Item2.ToString());

            Tuple<int, float> ret12 = ubot.get_tcp_jerk();
            Console.WriteLine("[UbotApi ] get_tcp_jerk  ret: " + ret12.Item1.ToString() + " jerk: " + ret12.Item2.ToString());
            Tuple<int, float> ret13 = ubot.get_tcp_maxacc();
            Console.WriteLine("[UbotApi ] get_tcp_maxacc  ret: " + ret13.Item1.ToString() + " maxacc: " + ret13.Item2.ToString());
            Tuple<int, float> re13 = ubot.get_joint_jerk();
            Console.WriteLine("[UbotApi ] get_joint_jerk  ret: " + re13.Item1.ToString() + " jerk: " + re13.Item2.ToString());
            Tuple<int, float> ret14 = ubot.get_joint_maxacc();
            Console.WriteLine("[UbotApi ] get_joint_maxacc  ret: " + ret14.Item1.ToString() + " maxacc: " + ret14.Item2.ToString());
            Tuple<int, float[]> ret15 = ubot.get_tcp_offset();
            Console.WriteLine("[UbotApi ] get_tcp_offset  ret: " + ret15.Item1.ToString());
            Print_Msg.nvect_03f("get_tcp_offset  : ", ret15.Item2, 6);
            Tuple<int, float[]> ret16 = ubot.get_tcp_load();
            Print_Msg.nvect_03f("get_tcp_load  : ", ret16.Item2, 4);
            Tuple<int, float[]> ret17 = ubot.get_gravity_dir();
            Print_Msg.nvect_03f("get_gravity_dir  : ", ret17.Item2, 3);
            Tuple<int, int> ret18 = ubot.get_collis_sens();
            Console.WriteLine("[UbotApi ] get_collis_sens  ret: " + ret18.Item1.ToString() + " collis: " + ret18.Item2.ToString());

            Tuple<int, int> ret19 = ubot.get_teach_sens();
            Console.WriteLine("[UbotApi ] get_teach_sens  ret: " + ret18.Item1.ToString() + " teach: " + ret19.Item2.ToString());
            Tuple<int, float[]> ret20 = ubot.get_tcp_target_pos();
            Print_Msg.nvect_03f("get_tcp_target_pos  : ", ret20.Item2, 6);
            Tuple<int, float[]> ret21 = ubot.get_joint_target_pos();
            Print_Msg.nvect_03f("get_joint_target_pos  : ", ret21.Item2, ret4.Item2);
        }
    }
}
