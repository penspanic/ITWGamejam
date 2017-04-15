using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using ITW.Protocol.ToServer;

namespace ITWServer.Network
{
    public static class PacketParser
    {
        public static void ParseAndDeliver(Network.PacketHandler handler, Vdb.Session session, byte[] data)
        {
            IFormatter f = new BinaryFormatter();
            ITW.Protocol.Packet p = (ITW.Protocol.Packet)f.Deserialize(new System.IO.MemoryStream(data));

            switch (p.PacketName)
            {
                case Connect.TypeName:
                    handler.Call(session, p as Connect);
                    break;
                default:
                    // 이상한 패킷
                    break;
            }
        }
    }
}
