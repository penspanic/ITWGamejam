using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ITWServer
{
    class ITWServer
    {
        static void Main(string[] args)
        {
            // 시스템들 생성해야 함
            // SDB 로드?


            TcpListener listener = new TcpListener(IPAddress.Any, 3355);
            listener.Start();
            while(true)
            {
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}
