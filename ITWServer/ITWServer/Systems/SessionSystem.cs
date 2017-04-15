using System;

namespace ITWServer.Systems
{
    public class SessionSystem : System
    {

        public SessionSystem(Network.PacketHandler packetHander) : base(packetHander)
        {
            packetHander.Bind<ITW.Protocol.ToServer.Connect>(Connect);
        }

        private bool Connect(Vdb.Session session, ITW.Protocol.ToServer.Connect packet)
        {
            Console.Write("Connect : " + packet.Id);
            return true;
        }
    }
}