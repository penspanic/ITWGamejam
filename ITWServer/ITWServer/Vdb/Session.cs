using System;
using System.Net;
using System.Net.Sockets;

namespace ITWServer.Vdb
{
    public class Session
    {
        public Session()
        {
        }
        public Socket socket;
        public byte[] readBuffer;
        public byte[] packetBuffer;
        public int readed = 0;
    }
}
