using System;
using utapi.basic;
using utapi.common;

namespace utapi.adra
{
    class AdraApiTcp : AdraApiBase
    {
        private String DB_FLG;

        public bool _is_err;

        private int id;

        public AdraApiTcp(String ip, int port=6001, int bus_type=0, int is_reset=1, int udp_port=5001, uint baud =0xFFFFFFFF)
        {
            //u"""AdraApiTcp is an interface class that controls the ADRA actuator through a EtherNet TCP.
            //EtherNet-to-RS485 or EtherNet-to-CAN module hardware is required to connect the computer and the actuator.

            //Args:
            //    ip (string): IP address of the EtherNet module.
            //    port (int): TCP port of EtherNet module. The default value is 6001.
            //    bus_type (int, optional): 0 indicates the actuator that uses the RS485 port.
            //                            1 indicates the actuator that uses the CAN port.
            //                            Defaults to 0.
            //    is_reset (int, optional): Defaults to 1. Whether to reset can be reset in the following situations.
            //            1. If connection type is UDP and DataLink is connected to TCP after being powered on, reset is required.
            //            2. If connection type is UDP and DataLink is not connected to TCP after being powered on, you do not need to reset.
            //            3. If connection type is TCP and DataLink is connected to TCP or UDP after being powered on, reset is required.
            //            4. If connection type is TCP and DataLink is not connected to TCP or UDP after being powered on, you do not need to reset.
            //            Note: In any case, it is good to use reset, but the initialization time is about 3 seconds longer than that without reset.
            //            Note: After DataLink is powered on and connected to USB, it needs to be powered on again to connect to TCP or UDP.
            //    udp_port (int, optional): UDP port of EtherNet module. The default value is 5001.
            //    baud (int, optional): Set the baud rate of the EtherNet to RS485/CAN module to be the same as that of the actuator.
            //                        If the baud rate is set to 0xFFFFFFFF, the baud rate of the EtherNet to RS485/CAN module is not set.
            //                        The default value is 0xFFFFFFFF.
            //"""
            DB_FLG = "[Adra Tcp] ";
            _is_err = false;
            id = 1;
            Console.WriteLine(DB_FLG + "SocketTcp, ip:" + ip + ", tcp_port:" + port.ToString() + ", baud:" + baud.ToString());
            if(bus_type == 0)
            {
                if(is_reset == 0)
                {
                    //this.
                }
                UtrcDecode bus_decode = new UtrcDecode(0xAA, id);
                SocketTcp socket_fp = new SocketTcp(ip, port);
                if(socket_fp.is_error())
                {
                    Console.WriteLine(DB_FLG + "Error: SocketTcp, ip:" +ip+ ", port:"+port.ToString());
                    _is_err = true;
                    return;
                }
                socket_fp.flush();
                UtrcClient bus_client = new UtrcClient(socket_fp);
                int ret = bus_client.connect_device(baud);
                if(ret != 0)
                {
                    _is_err = true;
                    Console.WriteLine(DB_FLG + "Error: connect_device: ret = " + ret.ToString());
                    return;
                }
                UtrcType tx_data = new UtrcType();
                tx_data.state = 0x00;
                tx_data.slave_id = (byte)id;
                this._init_(socket_fp, bus_client, tx_data);
            }
            else if(bus_type == 1)
            {

            }
        }
    }


}
