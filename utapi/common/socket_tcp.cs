using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace utapi.common
{
    class SocketTcp : SocketFP
    {
        private Socket fp;

        public SocketTcp(String ip, int port,  int rxque_max = 10, int rxdata_len = 128)
        {
            DB_FLG = "[SocketTcp]";
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);

                fp = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                fp.Blocking = true;
                fp.Connect (endPoint);
                if (fp.Connected == false)
                {
                    throw new Exception("can not connect the server");
                }

                rx_que = new Queue<byte[]>();
                this.rxque_max = rxque_max;
                this.rxdata_len = rxdata_len;

                ThreadStart childref = new ThreadStart(run);
                Thread childThread = new Thread(childref);
                childThread.Start();
                is_err = false;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(DB_FLG + "Error: __init__, ip:" + ip + ", port:" + port.ToString());
                is_err = true;
            }
        }

        private void run()
        {
            recv_proc();
        }

        public override void close()
        {
            if (is_err == false)
            {
                fp.Shutdown(SocketShutdown.Both);
                fp.Close();
                is_err = true;
            }
        }

        public override bool flush(int master_id = -1, int slave_id = -1)
        {
            if (is_err)
            {
                return false;
            }
            while (rx_que.Count > 0)
            {
                rx_que.Dequeue();
            }
            return true;
        }

        public override bool write(byte[] data)
        {
            if (is_err)
            {
                return false;
            }
            try
            {
                Print_Msg.nhex(data, data.Length);
                fp.Send (data);
                return true;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(DB_FLG + "Error: write");
                is_err = true;
                return false;
            }
        }

        public override int read(byte[] buf, int timeout_s = 1)
        {
            if (is_err)
            {
                Console.WriteLine(DB_FLG + "Error: read() is_err != true");
                return -1;
            }
            int sleepCount = timeout_s * 100;
            while (sleepCount > 0)
            {
                if (rx_que.Count > 0)
                {
                    byte[] tem =  rx_que.Dequeue();
                    for (int i = 0; i < tem.Length; i++)
                    {
                        buf[i] = tem[i];
                    }
                    return tem.Length;
                }
                Thread.Sleep(10);
                sleepCount--;
            }
            return -1;
        }

        private void recv_proc()
        {
            Console.WriteLine(DB_FLG + "recv_proc thread start");
            try
            {
                while (is_err == false)
                {
                    byte[] rx_data = new byte[rxdata_len];
                    int data_len = fp.Receive(rx_data);

                    // Console.WriteLine(DB_FLG + "recv_proc date_len " + data_len.ToString());
                    byte[] queue_data = new byte[data_len];
                    for (int i = 0; i < data_len; i++)
                    {
                        queue_data[i] = rx_data[i];
                    }

                    Print_Msg.nhex (queue_data, data_len);
                    if (data_len == 0)
                    {
                        is_err = true;
                        break;
                    }
                    if (rx_que.Count >= rxque_max)
                    {
                        rx_que.Dequeue();
                    }
                    rx_que.Enqueue (queue_data);
                }
            }
            catch (System.Exception e)
            {
                close();
                Console.WriteLine(e.Message);
                Console.WriteLine(DB_FLG + "Error: recv_proc");
            }
            Console.WriteLine(DB_FLG + "recv_proc exit");
        }

    }
}
