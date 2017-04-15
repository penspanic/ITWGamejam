using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITW.Protocol.ToServer
{
    public class Connect : Packet
    {
        public const string TypeName = "ToServer::Session::Connect";
        public Connect()
        {
            base.PacketName = TypeName;
        }

        public string Id { get; set; }
    }
}
