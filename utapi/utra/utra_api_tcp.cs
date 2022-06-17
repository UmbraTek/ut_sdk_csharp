using System;
using utapi.basic;
using utapi.common;

namespace utapi.utra
{
    class UtraApiTcp : _ArmApiBase
    {
        SocketTcp socket_fp;

        public UtraApiTcp(String ip)
        {
            socket_fp = new SocketTcp(ip, 502);
            if (socket_fp.is_error() == true)
            {
                Console.WriteLine("[UtraApiTcp ] Error: SocketTCP ");
                return;
            }
            this._init_(socket_fp);
        }
    }
}
