using System;
using System.Collections.Generic;

namespace utapi.common
{
    class UTRC_RX_ERROR
    {
        public static int M_ID = -1;

        public static int S_ID = -2;

        public static int TIMEOUT = -3;

        public static int STATE = -4;

        public static int LEN = -5;

        public static int RW = -6;

        public static int CMD = -7;

        public static int CRC = -8;

        public static int CONNECT = -9;

        public static int LEN_MIN = -10;
    }

    class UTRC_RW
    {
        public static byte R = 0;

        public static byte W = 1;
    }

    class UtrcType
    {
        public byte id;

        public byte master_id;

        public byte slave_id;

        public byte state;

        public byte len;

        public byte rw;

        public byte cmd;

        public byte[] data;

        public byte[] crc;

        public byte[] buf;

        public UtrcType()
        {
            master_id = 0xAA;
            slave_id = 0;
            state = 0;
            len = 0;
            rw = 0;
            cmd = 0;
            data = new byte[125];
            crc = new byte[2] { 0, 0 };
            // buf = new byte[128];
        }

        public byte[] pack()
        {
            this.buf = new byte[len + 5];
            this.buf[0] = this.master_id;
            this.buf[1] = this.slave_id;
            this.buf[2] = (byte)(((this.state & 0x01) << 7) + (this.len & 0x7F));
            this.buf[3] = (byte)(((this.rw & 0x01) << 7) + (this.cmd & 0x7F));
            for (int i = 0; i < this.len - 1; i++)
            {
                buf[i + 4] = data[i];
            }
            byte[] crc = CRC16.modbus(this.buf, this.len + 3);
            this.buf[len + 3] = crc[0];
            this.buf[len + 4] = crc[1];

            // Print_Msg.nhex(buf, len + 5);
            return this.buf;
        }

        public int unpack(byte[] buf, int length)
        {
            this.buf = new byte[length];
            for (int i = 0; i < length; i++)
            {
                this.buf[i] = buf[i];
            }
            if (length < 6)
            {
                Console.WriteLine("[UtrcType] Error: UTRC_RX_ERROR.LEN: " + length.ToString());
                return UTRC_RX_ERROR.LEN;
            }
            this.len = (byte)(buf[2] & 0x7F);
            if (this.len + 5 != length)
            {
                Console.WriteLine("[UtrcType] Error: UTRC_RX_ERROR.LEN: " + this.len.ToString() + " " + length.ToString());
                return UTRC_RX_ERROR.LEN;
            }
            this.master_id = buf[0];
            this.slave_id = buf[1];
            this.state = (byte)((buf[2] & 0x80) >> 7);
            this.rw = (byte)((buf[3] & 0x80) >> 7);
            this.cmd = (byte)(buf[3] & 0x7F);

            int data_len = this.len - 1;
            for (int i = 0; i < data_len; i++)
            {
                this.data[i] = buf[4 + i];
            }
            this.crc[0] = buf[data_len + 4];
            this.crc[1] = buf[data_len + 5];
            return 0;
        }
    }

    class UtrcClient
    {
        SocketFP port_fp;

        public UtrcClient(SocketFP _socket_fp)
        {
            port_fp = _socket_fp;
            port_fp.flush();
        }

        public int connect_device(uint baud= 0xFFFFFFFF)
        {
            UtrcType tx_utrc = new UtrcType();
            tx_utrc.master_id = 0xAA;
            tx_utrc.slave_id = 0x55;
            tx_utrc.state = 0;
            tx_utrc.len = 0x08;
            tx_utrc.rw = 0;
            tx_utrc.cmd = 0x7F;
            for(int i=0;i<8;i++)
            {
                tx_utrc.data[i] = 0x7F;
            }
            HexData.uint32_to_bytes_big(baud,tx_utrc.data);
            byte[] buf = tx_utrc.pack();
            this.port_fp.flush(tx_utrc.slave_id, tx_utrc.master_id);
            this.port_fp.write(buf);
            UtrcType rx_utrc = new UtrcType();
            int ret = this.pend(rx_utrc,tx_utrc, 1,1000);
            return ret;
        }

        public void send(UtrcType tx_utrc)
        {
            byte[] buf = tx_utrc.pack();
            port_fp.flush(tx_utrc.slave_id, tx_utrc.master_id);
            port_fp.write (buf);
        }


        public int pend(UtrcType rx_utrc, UtrcType tx_utrc, byte r_len, int timeout_s)
        {
            // UtrcType rx_utrc = new UtrcType();
            int ret = UTRC_RX_ERROR.TIMEOUT;
            byte[] rx_data = new byte[128];
            int length = port_fp.read(rx_data, timeout_s);
            if (length == -1)
            {
                return ret;
            }
            ret = rx_utrc.unpack(rx_data, length);
            if (ret != 0)
            {
                return ret;
            }
            else if (rx_utrc.master_id != tx_utrc.slave_id && tx_utrc.slave_id != 0x55)
            {
                Console.WriteLine("[UtrcCli] Error: UTRC_RX_ERROR.M_ID: " + rx_utrc.master_id.ToString() + " " + tx_utrc.slave_id.ToString());
                ret = UTRC_RX_ERROR.M_ID;
            }
            else if (rx_utrc.slave_id != tx_utrc.master_id)
            {
                Console.WriteLine("[UtrcCli] Error: UTRC_RX_ERROR.S_ID: " + rx_utrc.slave_id.ToString() + " " + tx_utrc.master_id.ToString());
                ret = UTRC_RX_ERROR.S_ID;
            }
            else if (rx_utrc.state != 0)
            {
                ret = UTRC_RX_ERROR.STATE;
            }
            else if (rx_utrc.len != r_len + 1)
            {
                Console.WriteLine("[UtrcCli] Error: UTRC_RX_ERROR.LEN: " + rx_utrc.len.ToString() + " " + r_len.ToString());
                ret = UTRC_RX_ERROR.LEN;
            }
            else if (rx_utrc.rw != tx_utrc.rw)
            {
                Console.WriteLine("[UtrcCli] Error: UTRC_RX_ERROR.RW: " + rx_utrc.rw.ToString() + " " + tx_utrc.rw.ToString());
                ret = UTRC_RX_ERROR.RW;
            }
            else if (rx_utrc.cmd != tx_utrc.cmd)
            {
                Console.WriteLine("[UtrcCli] Error: UTRC_RX_ERROR.CMD: " + rx_utrc.cmd.ToString() + " " + tx_utrc.cmd.ToString());
                ret = UTRC_RX_ERROR.CMD;
            }
            return ret;
        }
    }

    class UX2HEX_RXSTART
    {
        public static int FROMID = 0;

        public static int TOID = 1;

        public static int LEN = 2;

        public static int DATA = 3;

        public static int CRC1 = 4;

        public static int CRC2 = 5;

        public static int RXLEN_MAX = 64;
    }

    class UtrcDecode
    {
        private String DB_FLG;

        private int rxstate;

        private int data_idx;

        private int len;

        private int fromid;

        private int toid;

        private Queue<byte> rxbuf;


        public UtrcDecode(int fromid, int toid)
        {
            DB_FLG = "[ux2 ptcl] ";
            rxstate = UX2HEX_RXSTART.FROMID;
            data_idx = 0;
            len = 0;
            this.fromid = fromid;
            this.toid = toid;
            rxbuf = new Queue<byte>();
        }

        public void flush(int fromid = -1, int toid = -1)
        {
            this.rxstate = UX2HEX_RXSTART.FROMID;
            this.data_idx = 0;
            this.len = 0;
            if (fromid != -1)
            {
                this.fromid = fromid;
            }
            if (toid != -1)
            {
                this.toid = toid;
            }
        }

        public void put(byte[] rxstr, int length, Queue<byte[]> rx_que)
        {
            if (rxstr.Length < length)
            {
                Console.WriteLine(DB_FLG + "error: len(rxstr) < length");
            }
            for (int i = 0; i < length; i++)
            {
                byte rxch = rxstr[i];
                if (UX2HEX_RXSTART.FROMID == this.rxstate)
                {
                    if (this.fromid == rxch || this.fromid == 0x55)
                    {
                        this.rxbuf.Clear();
                        this.rxbuf.Enqueue(rxch);
                        this.rxstate = UX2HEX_RXSTART.TOID;
                    }
                    
                }
                else if (UX2HEX_RXSTART.TOID == this.rxstate)
                {
                    if (this.toid == rxch)
                    {
                        this.rxbuf.Enqueue(rxch);
                        this.rxstate = UX2HEX_RXSTART.LEN;
                    }
                    else
                    {
                        this.rxstate = UX2HEX_RXSTART.FROMID;
                    }
                }
                else if (UX2HEX_RXSTART.LEN == this.rxstate)
                {
                    if ((rxch & 0x7F) < UX2HEX_RXSTART.RXLEN_MAX)
                    {
                        this.rxbuf.Enqueue(rxch);
                        this.len = rxch & 0x7F;
                        this.data_idx = 0;
                        this.rxstate = UX2HEX_RXSTART.DATA;
                    }
                    else
                    {
                        this.rxstate = UX2HEX_RXSTART.FROMID;
                    }
                }
                else if (UX2HEX_RXSTART.DATA == this.rxstate)
                {
                    if (this.data_idx < this.len)
                    {
                        this.rxbuf.Enqueue(rxch);
                        this.data_idx += 1;
                        if (this.data_idx == this.len)
                        {
                            this.rxstate = UX2HEX_RXSTART.CRC1;
                        }
                    }
                    else
                    {
                        this.rxstate = UX2HEX_RXSTART.FROMID;
                    }
                }
                else if (UX2HEX_RXSTART.CRC1 == this.rxstate)
                {
                    this.rxbuf.Enqueue(rxch);
                    this.rxstate = UX2HEX_RXSTART.CRC2;
                }
                else if (UX2HEX_RXSTART.CRC2 == this.rxstate)
                {
                    this.rxbuf.Enqueue(rxch);
                    this.rxstate = UX2HEX_RXSTART.FROMID;
                    byte[] buf = this.rxbuf.ToArray();
                    byte[] crc = CRC16.modbus(buf, this.len + 3);
                    if (crc[0] == buf[this.len + 3] && crc[1] == buf[this.len + 4])
                    {
                        //rx_que.Clear();
                        rx_que.Enqueue (buf);
                    }
                }
            }
        }
    }
}
