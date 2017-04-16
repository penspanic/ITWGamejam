using System;
using System.Net;
using System.Net.Sockets;

namespace ITWServer.Vdb
{
    public class Session
    {
        public static readonly int BufferSize = 4096;
        public Session()
        {
            readBuffer = new byte[BufferSize];
        }
        public TcpClient client;
        public byte[] readBuffer;
    }
}
