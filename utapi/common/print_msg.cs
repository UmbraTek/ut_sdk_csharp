using System;

namespace utapi.common
{
    class Print_Msg
    {
        public static void nhex(byte[] data, int len)
        {
            String str = "";
            for (int i = 0; i < len; i++)
            {
                str = str + data[i].ToString("x02") + " ";
            }
            Console.WriteLine (str);
        }

        public static void nvect_03f(String str, float[] data, int len)
        {
            for (int i = 0; i < len; i++)
            {
                str = str + data[i].ToString() + " ";
            }
            Console.WriteLine (str);
        }
    }
}
