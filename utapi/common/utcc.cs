namespace utapi.common
{
    class UTCC_RX_ERROR
    {
        public static int HEAD = -1;

        public static int ID = -2;

        public static int TIMEOUT = -3;

        public static int STATE = -4;

        public static int LEN = -5;

        public static int RW = -6;

        public static int CMD = -7;

        public static int CRC = -8;

        public static int CONNECT = -9;
    }

    class UtccType
    {
        private byte head;

        private byte state;

        private byte id;

        private byte len;

        private byte rw;

        private byte cmd;

        private byte[] data;

        private byte[] crc;

        private byte[] buf;

        public UtccType()
        {
            head = 0xAA;
            state = 0;
            id = 0;
            len = 0;
            rw = 0;
            cmd = 0;
            data = new byte[7] { 0, 0, 0, 0, 0, 0, 0 };
            crc = new byte[2] { 0, 0 };
            buf = new byte[128];
        }

        public int pack()
        {
            this.buf[0] = head;

            // this.buf[1] = HexData.
            this.buf[2] = len;
            this.buf[3] = (byte)(((rw & 0x01) << 7) + (cmd & 0x7F));
            for (int i = 0; i < len - 1; i++)
            {
                buf[i + 4] = data[i];
            }
            byte[] crc = CRC16.modbus(buf, len + 3);
            buf[len + 3] = crc[0];
            buf[len + 4] = crc[1];
            return len + 5;
        }

        public int unpack(byte[] buf, int length)
        {
            return 0;
        }
    }
}
