using System;
using System.Threading;
using utapi.common;

namespace utapi.basic
{
    class ArmReportConfig
    {
        private bool _is_err;

        private int _rxcnt;

        private bool _is_update;

        public float trs_maxacc;

        public float trs_jerk;

        public float rot_maxacc;

        public float rot_jerk;

        public float p2p_maxacc;

        public float p2p_jerk;

        public float[] tcp_offset;

        public float[] tcp_load;

        public float[] gravity_dir;

        public uint collis_sens;

        public uint teach_sens;

        SocketTcp _socekt_fp;

        public bool __init__(String ip, int port = 30003)
        {
            _is_err = false;
            _rxcnt = 0;
            _is_update = false;

            trs_maxacc = 0;
            trs_jerk = 0;
            rot_maxacc = 0;
            rot_jerk = 0;
            p2p_maxacc = 0;
            p2p_jerk = 0;
            tcp_offset = new float[6] { 0, 0, 0, 0, 0, 0 };
            tcp_load = new float[4] { 0, 0, 0, 0 };
            gravity_dir = new float[3] { 0, 0, 0 };
            collis_sens = 0;
            teach_sens = 0;

            _socekt_fp = new SocketTcp(ip, port, 32);
            if (_socekt_fp.is_error() == true)
            {
                Console.WriteLine("[UbotRConf] Error: SocketTcp failed, ip: " + ip + ", port:" + port.ToString());
                return false;
            }
            Console.WriteLine("[UbotRConf] Tcp Report Status connection successful");
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
                if (len < 80)
                {
                    continue;
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
            if (len % 80 != 0)
            {
                Console.WriteLine("[UbotRConf] Error: rx_data len =" + len.ToString());
                _is_err = true;
                return;
            }
            trs_maxacc = bytes_to_fp32_lit(rx_data, len - 78);
            trs_jerk = bytes_to_fp32_lit(rx_data, len - 74);
            rot_maxacc = bytes_to_fp32_lit(rx_data, len - 70);
            rot_jerk = bytes_to_fp32_lit(rx_data, len - 66);
            p2p_maxacc = bytes_to_fp32_lit(rx_data, len - 62);
            p2p_jerk = bytes_to_fp32_lit(rx_data, len - 58);
            tcp_offset =
                new float[6]
                {
                    bytes_to_fp32_lit(rx_data, len - 54),
                    bytes_to_fp32_lit(rx_data, len - 50),
                    bytes_to_fp32_lit(rx_data, len - 46),
                    bytes_to_fp32_lit(rx_data, len - 42),
                    bytes_to_fp32_lit(rx_data, len - 38),
                    bytes_to_fp32_lit(rx_data, len - 34)
                };
            tcp_load = new float[4] { bytes_to_fp32_lit(rx_data, len - 30), bytes_to_fp32_lit(rx_data, len - 26), bytes_to_fp32_lit(rx_data, len - 22), bytes_to_fp32_lit(rx_data, len - 28) };
            gravity_dir = new float[3] { bytes_to_fp32_lit(rx_data, len - 14), bytes_to_fp32_lit(rx_data, len - 10), bytes_to_fp32_lit(rx_data, len - 6) };
            collis_sens = (uint) rx_data[len - 2];
            teach_sens = (uint) rx_data[len - 1];
            _is_update = true;
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
            Console.WriteLine("trs_maxacc : " + trs_maxacc.ToString());
            Console.WriteLine("trs_jerk : " + trs_jerk.ToString());
            Console.WriteLine("rot_maxacc : " + rot_maxacc.ToString());
            Console.WriteLine("rot_jerk : " + rot_jerk.ToString());
            Console.WriteLine("p2p_maxacc : " + p2p_maxacc.ToString());
            Console.WriteLine("p2p_jerk : " + p2p_jerk.ToString());
            Console.WriteLine("collis_sens : " + collis_sens.ToString());
            Console.WriteLine("teach_sens : " + teach_sens.ToString());
            Print_Msg.nvect_03f("tcp_offset : ", tcp_offset, 6);
            Print_Msg.nvect_03f("tcp_load : ", tcp_load, 4);
            Print_Msg.nvect_03f("gravity_dir : ", gravity_dir, 3);
        }
    }
}
