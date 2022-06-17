using System;
using utapi.common;
using utapi.utra;

namespace example.utra
{
    class Program3
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
            Console.WriteLine("[UbotApi ] moveto_joint_p2p  ret: " + ret5.ToString());
            float[] joint1 = new float[6] { 1.248f, 1.416f, 1.155f, -0.252f, -1.248f, -0.003f };
            float[] joint2 = new float[6] { 0.990f, 1.363f, 1.061f, -0.291f, -0.990f, -0.006f };
            float[] joint3 = new float[6] { 1.169f, 1.022f, 1.070f, 0.058f, -1.169f, -0.004f };
            int ret9 = ubot.moveto_joint_p2p(joint3, speed, acc, 60);
            Console.WriteLine("[UbotApi ] moveto_joint_p2p  ret: " + ret9.ToString());
            int ret6 = ubot.moveto_joint_p2p(joint1, speed, acc, 60);
            Console.WriteLine("[UbotApi ] moveto_joint_p2p  ret: " + ret6.ToString());
            int ret7 = ubot.moveto_joint_p2p(joint2, speed, acc, 60);
            Console.WriteLine("[UbotApi ] moveto_joint_p2p  ret: " + ret7.ToString());
            int ret8 = ubot.moveto_joint_p2p(joint3, speed, acc, 60);
            Console.WriteLine("[UbotApi ] moveto_joint_p2p  ret: " + ret8.ToString());
        }
    }
}
