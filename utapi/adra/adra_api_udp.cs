using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using utapi.basic;
using utapi.common;

namespace utapi.adra
{
    class AdraApiUdp : AdraApiBase
    {
        private String DB_FLG;

        public bool _is_err;

        private int id;

        public AdraApiUdp(String ip, int port = 5001, int bus_type = 0, int is_reset = 1, int tcp_port = 6001, uint baud = 0xFFFFFFFF)
        {
            //u"""AdraApiUdp is an interface class that controls the ADRA actuator through a EtherNet UDP.
            //EtherNet - to - RS485 or EtherNet-to - CAN module hardware is required to connect the computer and the actuator.
            //Args:
            //    ip(string): IP address of the EtherNet module.
            //    port(int): UDP port of EtherNet module.The default value is 5001.
            //bus_type(int, optional): 0 indicates the actuator that uses the RS485 port.
            //                            1 indicates the actuator that uses the CAN port.
            //                            Defaults to 0.
            //    is_reset(int, optional): Defaults to 1.Whether to reset can be reset in the following situations.
            //            1.If connection type is UDP and DataLink is connected to TCP after being powered on, reset is required.
            //            2.If connection type is UDP and DataLink is not connected to TCP after being powered on, you do not need to reset.
            //            3.If connection type is TCP and DataLink is connected to TCP or UDP after being powered on, reset is required.
            //            4.If connection type is TCP and DataLink is not connected to TCP or UDP after being powered on, you do not need to reset.
            //        Note: In any case, it is good to use reset, but the initialization time is about 3 seconds longer than that without reset.
            //            Note: After DataLink is powered on and connected to USB, it needs to be powered on again to connect to TCP or UDP.
            //    tcp_port(int, optional): TCP port of EtherNet module.The default value is 6001.
            //baud(int, optional): Set the baud rate of the EtherNet to RS485 / CAN module to be the same as that of the actuator.
            //                        If the baud rate is set to 0xFFFFFFFF, the baud rate of the EtherNet to RS485/ CAN module is not set.
            //                        The default value is 0xFFFFFFFF.
            //"""
            DB_FLG = "[Adra Udp] ";
            _is_err = false;
            id = 1;
            Console.WriteLine(DB_FLG + "SocketUDP, ip:" + ip + ", udp_port:" + port.ToString() + ", baud:" + baud.ToString());
            if (bus_type == 0)
            {
                if (is_reset == 0)
                {
                    this._reset_net_rs485(ip, tcp_port, port);
                }
                UtrcDecode bus_decode = new UtrcDecode(0xAA, id);
                SocketUDP socket_fp = new SocketUDP(ip, port);
                if (socket_fp.is_error())
                {
                    Console.WriteLine(DB_FLG + "Error: SocketTcp, ip:" + ip + ", port:" + port.ToString());
                    _is_err = true;
                    return;
                }
                socket_fp.flush();
                UtrcClient bus_client = new UtrcClient(socket_fp);
                int ret = bus_client.connect_device(baud);
                if (ret != 0)
                {
                    _is_err = true;
                    Console.WriteLine(DB_FLG + "Error: connect_device: ret = " + ret.ToString());
                    return;
                }
                UtrcType tx_data = new UtrcType();
                tx_data.state = 0x00;
                tx_data.slave_id = (byte) id;
                this._init_(socket_fp, bus_client, tx_data);
            }
            else if (bus_type == 1)
            {
            }
        }

        private void _reset_net_rs485(String ip, int tcp_port, int udp_port)
        {
            UtrcType tx_utrc = new UtrcType();
            tx_utrc.master_id = 0xAA;
            tx_utrc.slave_id = 0x55;
            tx_utrc.state = 0;
            tx_utrc.len = 0x08;
            tx_utrc.rw = 0;
            tx_utrc.cmd = 0x7F;
            for (int i = 0; i < 8; i++)
            {
                tx_utrc.data[i] = 0x7F;
            }
            byte[] buf = tx_utrc.pack();
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), tcp_port);
                Socket fp = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                fp.Blocking = true;
                fp.Connect (endPoint);
                if (fp.Connected == false)
                {
                    throw new Exception("can not connect the server");
                }
                fp.Send (buf);
                fp.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Thread.Sleep(100);
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), udp_port);
                Socket fp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                fp.SendTo (buf, endPoint);
                fp.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Thread.Sleep(3000);
        }
    }
}
