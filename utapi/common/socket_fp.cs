using System;
using System.Collections.Generic;

namespace utapi.common
{
    abstract class SocketFP
    {
        public bool is_err = true;

        public String DB_FLG = "[SocketFP]";

        public Queue<byte[]> rx_que;

        public int rxque_max;

        public int rxdata_len;

        public bool is_error()
        {
            return is_err;
        }

        public abstract void close();

        public abstract bool flush(int master_id = -1, int slave_id = -1);

        public abstract bool write(byte[] data);

        public abstract int read(byte[] buf, int timeout_s);
    }
}
