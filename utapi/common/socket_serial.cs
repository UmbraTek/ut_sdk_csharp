using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace utapi.common
{
    class SocketSerial : SocketFP
    {
        SerialPort serialPort = null;

        public UtrcDecode rx_decoder = null;

        public SocketSerial(String port, int baud, UtrcDecode bus_decode = null, int rxque_max = 10)
        {
            DB_FLG = "[SockeSer]";
            try
            {
                serialPort = new SerialPort();
                serialPort.PortName = port;
                serialPort.BaudRate = baud;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;
                serialPort.Parity = Parity.None;
                serialPort.Open();
                if (serialPort.IsOpen == false)
                {
                    throw new Exception("can not connect");
                }

                serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);

                rx_que = new Queue<byte[]>();
                this.rxque_max = rxque_max;

                this.rx_decoder = bus_decode;

                is_err = false;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(DB_FLG + "Error: __init__, port:" + port + ", baud:" + baud.ToString());
                is_err = true;
            }
        }


        private void ReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort _SerialPort = (SerialPort) sender;

            int _bytesToRead = _SerialPort.BytesToRead;
            byte[] recvData = new byte[_bytesToRead];

            int readlength = _SerialPort.Read(recvData, 0, _bytesToRead);

            // Print_Msg.nhex (recvData, readlength);
            this.rx_decoder.put(recvData, readlength, this.rx_que);
        }

        public override void close()
        {
            if (is_err == false)
            {
                serialPort.Close();
                is_err = true;
            }
        }

        public override bool flush(int master_id = -1, int slave_id = -1)
        {
            if (is_err)
            {
                return false;
            }
            rx_que.Clear();
            if (rx_decoder != null)
            {
                rx_decoder.flush (master_id, slave_id);
            }
            return true;
        }

        public int get_baud()
        {
            if (is_err == false)
            {
                return this.serialPort.BaudRate;
            }
            else
            {
                return 0;
            }
        }

        public void set_baud(int baud)
        {
            if (is_err == false)
            {
                this.serialPort.BaudRate = baud;
            }
        }

        public override bool write(byte[] data)
        {
            if (is_err)
            {
                return false;
            }
            try
            {
                serialPort.Write(data, 0, data.Length);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(DB_FLG + "Error: write");
                is_err = true;
                return false;
            }
            return true;
        }

        public override int read(byte[] buf, int timeout_s)
        {
            if (is_err)
            {
                return -1;
            }
            int sleepCount = timeout_s * 100;
            while (sleepCount > 0)
            {
                if (rx_que.Count > 0)
                {
                    byte[] tem = (byte[]) rx_que.Dequeue();
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
    }


}
