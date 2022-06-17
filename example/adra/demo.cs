using System;
using utapi.adra;
using utapi.common;

namespace example.adra
{
    class ProgramDemo
    {
        static void Main(string[] args)
        {
            AdraApiSerial adra = new AdraApiSerial("COM11", 921600); //  # instantiate the adra executor api class
            adra.connect_to_id(1); //'  # The ID of the connected target actuator, where the ID is 1

            // Tuple<int, String> ret1 = adra.get_uuid(); //  # Set actuator motion mode 1: position mode
            // Console.WriteLine("get_uuid ret: " + ret1.Item2);
            float[] pos = new float[] {0.2f,0.3f};
            float[] tau = new float[] { 0.1f, 0.5f };
            //adra.set_ctau_target(1,2,pos); //  # Set actuator motion mode 1: position mode
            //adra.set_cpostau_target(1, 2, pos,tau);
            //Tuple<int, int, float, float> ret = adra.get_spostau_current();
            //Console.WriteLine("get_spostau_current num: " + ret.Item2.ToString());
            //Console.WriteLine("get_spostau_current pos: " + ret.Item3.ToString());
            //Console.WriteLine("get_spostau_current tua: " + ret.Item4.ToString());
            Tuple<int[], int[], float[], float[]> rets = adra.get_cpostau_current(1, 2);
            Console.WriteLine(" pos1: " + rets.Item3[0].ToString());
            Console.WriteLine(" pos2: " + rets.Item3[1].ToString());
        }
    }
}
