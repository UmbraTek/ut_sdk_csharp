using System;
using utapi.common;
using utapi.utra;

namespace example.utra
{
    class Program5
    {
        static void _Main(string[] args)
        {
            UtraApiTcp ubot = new UtraApiTcp("192.168.1.90");

            int ret1 = ubot.reset_err();
            Console.WriteLine("[UbotApi ] reset_err  ret: " + ret1.ToString());
            int ret2 = ubot.set_motion_mode(0);
            Console.WriteLine("[UbotApi ] set_motion_mode  ret: " + ret2.ToString());
            int ret3 = ubot.set_motion_enable(8, 1);
            Console.WriteLine("[UbotApi ] set_motion_enable  ret: " + ret3.ToString());
            int ret4 = ubot.set_motion_status(0);
            Console.WriteLine("[UbotApi ] set_motion_status  ret: " + ret4.ToString());

            float[] joint = new float[6] { 0, 0, 0, 0, 0, 0 };
            float speed = 0.1f;
            float acc = 3f;
            int ret5 = ubot.moveto_joint_p2p(joint, speed, acc, 60);

            float[] pos1 = new float[6] { -0.0f, -360.0f, 800.0f, 1.58f, 0.0f, 0.0f };
            float[] pos2 = new float[6] { -8.0f, -560.0f, 600.0f, 1.58f, 0.0f, 0.0f };
            float[] pos3 = new float[6] { -180.0f, -560.0f, 600.0f, 1.58f, 0.0f, 0.0f };
            speed = 20.0f;
            acc = 10000.0f;
            int ret9 = ubot.moveto_cartesian_lineb(pos1, speed, acc, 5.0f, 80);
            Console.WriteLine("[UbotApi ] moveto_cartesian_lineb  ret: " + ret9.ToString());
            int ret6 = ubot.moveto_cartesian_lineb(pos2, speed, acc, 5.0f, 80);
            Console.WriteLine("[UbotApi ] moveto_cartesian_lineb  ret: " + ret6.ToString());
            int ret7 = ubot.moveto_cartesian_lineb(pos3, speed, acc, 5.0f, 80);
            Console.WriteLine("[UbotApi ] moveto_cartesian_lineb  ret: " + ret7.ToString());
        }
    }
}
