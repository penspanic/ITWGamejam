using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITW.Protocol.FromServer
{
    [Serializable]
    public class Connect : Packet
    {
        public const string TypeName = "FromServer::Session::Connect";
        
        public Connect()
        {
            base.PacketName = TypeName;
        }

        public enum ErrorResult
        {
            SUCCESS,
            FAIL,
        }
        public ErrorResult Result { get; set; }
    }
}
