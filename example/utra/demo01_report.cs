using System;
using utapi.common;
using utapi.utra;

namespace example.utra
{
    class Program1
    {
        static void _Main(string[] args)
        {
            UtraReportConfig10Hz ubot = new UtraReportConfig10Hz("192.168.1.90");

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                if (ubot.is_update())
                {
                    ubot.print_data();
                }
            }
        }
    }
}
