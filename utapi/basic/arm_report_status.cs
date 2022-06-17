using System;
using System.Threading;
using utapi.common;

namespace utapi.basic
{
    class ArmReportStatus
    {
        private bool _is_err;

        private int _rxcnt;

        private bool _is_update;

        private int frame_len;

        private int axis;

        private uint motion_status;

        private uint motion_mode;

        private uint mt_brake;

        private uint mt_able;

        private uint err_code;

        private uint war_code;

        private uint cmd_num;

        private float[] joint;

        private float[] pose;

        private float[] tau;

        SocketTcp _socekt_fp;

        public bool __init__(String ip, int port)
        {
            _is_err = false;
            _rxcnt = 0;
            _is_update = false;

            frame_len = 0;

            axis = 0;
            motion_status = 0;
            motion_mode = 0;
            mt_brake = 0;
            mt_able = 0;
            err_code = 0;
            war_code = 0;
            cmd_num = 0;

            joint = new float[32];
            pose = new float[6];
            tau = new float[32];
            _socekt_fp = new SocketTcp(ip, port, 32);
            if (_socekt_fp.is_error() == true)
            {
                Console.WriteLine("[UbotRStat] Error: SocketTcp failed, ip: " + ip + ", port:" + port.ToString());
                return false;
            }
            Console.WriteLine("[UbotRStat] Tcp Report Status connection successful");
            ThreadStart childref = new ThreadStart(run);
            Thread childThread = new Thread(childref);
            childThread.Start();
            return true;
        }

        private void run()
        {
            while (_is_err == false)
            {
                byte[] rx_data = new byte[1024];
                int len = _socekt_fp.read(rx_data);
                if (len <= 41)
                {
                    continue;
                }
                if (axis == 0)
                {
                    if (len == (41 + 3 * 8))
                    {
                        axis = 3;
                        frame_len = 17 + axis * 4 + 6 * 4 + axis * 4;
                    }
                    if (len == (41 + 4 * 8))
                    {
                        axis = 4;
                        frame_len = 17 + axis * 4 + 6 * 4 + axis * 4;
                    }
                    if (len == (41 + 5 * 8))
                    {
                        axis = 5;
                        frame_len = 17 + axis * 4 + 6 * 4 + axis * 4;
                    }
                    if (len == (41 + 6 * 8))
                    {
                        axis = 6;
                        frame_len = 17 + axis * 4 + 6 * 4 + axis * 4;
                    }
                    if (len == (41 + 7 * 8))
                    {
                        axis = 7;
                        frame_len = 17 + axis * 4 + 6 * 4 + axis * 4;
                    }
                }
                flush_data (rx_data, len);
            }
            _socekt_fp.close();
            Console.WriteLine("[UbotRStat] ubot report status close");
        }

        public void close()
        {
            _is_err = true;
        }

        public bool is_err()
        {
            return _is_err;
        }

        private void flush_data(byte[] rx_data, int len)
        {
            if (len % frame_len != 0)
            {
                Console.WriteLine("[UbotRStat] Error: rx_data len =" + len.ToString());
                _is_err = true;
                return;
            }
            int start_index = len - frame_len;
            uint _axis = (uint) rx_data[start_index + 2];
            if (axis != _axis)
            {
                Console.WriteLine("[UbotRStat] Error: axis = " + _axis.ToString() + " " + axis.ToString());
                _is_err = true;
                return;
            }
            motion_status = (uint) rx_data[start_index + 3];
            motion_mode = (uint) rx_data[start_index + 4];
            mt_brake = bytes_to_uint32_lit(rx_data, start_index + 5);
            mt_able = bytes_to_uint32_lit(rx_data, start_index + 9);
            err_code = (uint) rx_data[start_index + 13];
            war_code = (uint) rx_data[start_index + 14];
            cmd_num = bytes_to_uint16_lit(rx_data, start_index + 15);
            for (int i = 0; i < axis; i++)
            {
                int j1 = start_index + 17 + i * 4;
                int j2 = j1 + axis * 4;
                int j3 = j2 + axis * 4;
                joint[i] = bytes_to_fp32_lit(rx_data, j1);
                pose[i] = bytes_to_fp32_lit(rx_data, j2);
                tau[i] = bytes_to_fp32_lit(rx_data, j3);
            }
            _is_update = true;
        }

        private uint bytes_to_uint16_lit(byte[] data, int start)
        {
            byte[] tem = new byte[2];
            if (BitConverter.IsLittleEndian)
            {
                tem[0] = data[start + 0];
                tem[1] = data[start + 1];
            }
            else
            {
                tem[1] = data[start + 0];
                tem[0] = data[start + 1];
            }
            return BitConverter.ToUInt16(tem, 0);
        }

        private uint bytes_to_uint32_lit(byte[] data, int start)
        {
            byte[] tem = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                tem[0] = data[start + 0];
                tem[1] = data[start + 1];
                tem[2] = data[start + 2];
                tem[3] = data[start + 3];
            }
            else
            {
                tem[3] = data[start + 0];
                tem[2] = data[start + 1];
                tem[1] = data[start + 2];
                tem[0] = data[start + 3];
            }
            return BitConverter.ToUInt32(tem, 0);
        }

        private float bytes_to_fp32_lit(byte[] data, int start)
        {
            byte[] tem = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                tem[0] = data[start + 0];
                tem[1] = data[start + 1];
                tem[2] = data[start + 2];
                tem[3] = data[start + 3];
            }
            else
            {
                tem[3] = data[start + 0];
                tem[2] = data[start + 1];
                tem[1] = data[start + 2];
                tem[0] = data[start + 3];
            }
            return BitConverter.ToSingle(tem);
        }

        public bool is_update()
        {
            bool temp = _is_update;
            _is_update = false;
            return temp;
        }

        public void print_data()
        {
            Console.WriteLine("axis : " + axis.ToString());
            Console.WriteLine("motion_status : " + motion_status.ToString());
            Console.WriteLine("motion_mode : " + motion_mode.ToString());
            Console.WriteLine("mt_brake : " + mt_brake.ToString());
            Console.WriteLine("mt_able : " + mt_able.ToString());
            Console.WriteLine("err_code : " + err_code.ToString());
            Console.WriteLine("war_code : " + war_code.ToString());
            Console.WriteLine("cmd_num : " + cmd_num.ToString());
            Print_Msg.nvect_03f("joint : ", joint, axis);
            Print_Msg.nvect_03f("pose : ", pose, 6);
            Print_Msg.nvect_03f("tau : ", tau, axis);
        }
    }
}
