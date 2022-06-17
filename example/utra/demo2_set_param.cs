using System;
using utapi.common;
using utapi.utra;

namespace example.utra
{
    class Program21
    {
        static void _Main(string[] args)
        {
            UtraApiTcp ubot = new UtraApiTcp("192.168.1.90");
            int ret = ubot.set_joint_maxacc(50f);
            Console.WriteLine("[UbotApi ] set_joint_maxacc  ret: " + ret.ToString());
        }
    }
}
