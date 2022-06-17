using System;
using utapi.basic;
using utapi.common;

namespace utapi.flxiv
{
    class FlxiVlApiSerial : FlxiVlApiBase
    {
        private String DB_FLG;

        public bool _is_err;

        private int id;

        public FlxiVlApiSerial(String port, int baud)
        {
            DB_FLG = "[FlxiE2ApiSerial] ";
            _is_err = false;
            id = 1;

            UtrcDecode bus_decode = new UtrcDecode(0xAA, id);
            SocketSerial socket_fp = new SocketSerial(port, baud, bus_decode);

            if (socket_fp.is_error())
            {
                Console.WriteLine(DB_FLG + "Error: SocketSerial, port:" + port + " baud: " + baud.ToString());
                _is_err = true;
                return;
            }
            socket_fp.flush();
            UtrcClient bus_client = new UtrcClient(socket_fp);

            UtrcType tx_data = new UtrcType();
            tx_data.state = 0x00;
            tx_data.slave_id = (byte) id;
            this._init_(socket_fp, bus_client, tx_data);
        }
    }


}
