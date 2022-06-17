using System;

namespace utapi.common
{
    class HexData
    {
        public static int bytes_to_int8(byte data)
        {
            sbyte tem = (sbyte) data;
            return (int) tem;
        }

        public static uint bytes_to_uint32_big(byte[] data, int start_index = 0)
        {
            byte[] tem = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                tem[3] = data[start_index];
                tem[2] = data[start_index + 1];
                tem[1] = data[start_index + 2];
                tem[0] = data[start_index + 3];
            }
            else
            {
                tem[0] = data[start_index];
                tem[1] = data[start_index + 1];
                tem[2] = data[start_index + 2];
                tem[3] = data[start_index + 3];
            }
            uint i = BitConverter.ToUInt32(tem, 0);
            return i;
        }

        public static int bytes_to_int32_big(byte[] data)
        {
            byte[] tem = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                tem[3] = data[0];
                tem[2] = data[1];
                tem[1] = data[2];
                tem[0] = data[3];
            }
            else
            {
                tem[0] = data[0];
                tem[1] = data[1];
                tem[2] = data[2];
                tem[3] = data[3];
            }
            int i = BitConverter.ToInt32(tem, 0);
            return i;
        }

        public static void bytes_to_fp32_big(byte[] data, float[] value, int length = 1, int index = 0)
        {
            for (int i = 0; i < length; i++)
            {
                byte[] tem = new byte[4];
                if (BitConverter.IsLittleEndian)
                {
                    tem[3] = data[i * 4 + 0 + index];
                    tem[2] = data[i * 4 + 1 + index];
                    tem[1] = data[i * 4 + 2 + index];
                    tem[0] = data[i * 4 + 3 + index];
                }
                else
                {
                    tem[0] = data[i * 4 + 0 + index];
                    tem[1] = data[i * 4 + 1 + index];
                    tem[2] = data[i * 4 + 2 + index];
                    tem[3] = data[i * 4 + 3 + index];
                }
                value[i] = BitConverter.ToSingle(tem);
            }
        }

        public static void int32_to_bytes_big(int value, byte[] data)
        {
            byte[] tem = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                data[3] = tem[0];
                data[2] = tem[1];
                data[1] = tem[2];
                data[0] = tem[3];
            }
            else
            {
                data[0] = tem[0];
                data[1] = tem[1];
                data[2] = tem[2];
                data[3] = tem[3];
            }
        }

        public static void uint32_to_bytes_big(uint value, byte[] data)
        {
            byte[] tem = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                data[3] = tem[0];
                data[2] = tem[1];
                data[1] = tem[2];
                data[0] = tem[3];
            }
            else
            {
                data[0] = tem[0];
                data[1] = tem[1];
                data[2] = tem[2];
                data[3] = tem[3];
            }
        }

        public static void fp32_to_bytes_big(float[] values, byte[] data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tem = BitConverter.GetBytes(values[i]);
                if (BitConverter.IsLittleEndian)
                {
                    data[i * 4 + 3] = tem[0];
                    data[i * 4 + 2] = tem[1];
                    data[i * 4 + 1] = tem[2];
                    data[i * 4 + 0] = tem[3];
                }
                else
                {
                    data[i * 4 + 0] = tem[0];
                    data[i * 4 + 1] = tem[1];
                    data[i * 4 + 2] = tem[2];
                    data[i * 4 + 3] = tem[3];
                }
            }
        }

        public static void fp32_to_bytes_big(float[] values, byte[] data, int start)
        {
            for (int i = 0; i < values.Length; i++)
            {
                byte[] tem = BitConverter.GetBytes(values[i]);
                if (BitConverter.IsLittleEndian)
                {
                    data[start + i * 4 + 3] = tem[0];
                    data[start + i * 4 + 2] = tem[1];
                    data[start + i * 4 + 1] = tem[2];
                    data[start + i * 4 + 0] = tem[3];
                }
                else
                {
                    data[start + i * 4 + 0] = tem[0];
                    data[start + i * 4 + 1] = tem[1];
                    data[start + i * 4 + 2] = tem[2];
                    data[start + i * 4 + 3] = tem[3];
                }
            }
        }
    }
}
