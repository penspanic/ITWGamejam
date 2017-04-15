using System;

namespace ITWServer.Systems
{
    public class SessionSystem : System
    {

        public SessionSystem(ITWServer server, Network.PacketHandler packetHander) : base(server, packetHander)
        {
            server.OnAcceptClient += OnAcceptClient;
            packetHander.Bind<ITW.Protocol.ToServer.Session.Connect>(Connect);
            packetHander.Bind<ITW.Protocol.ToServer.Session.Disconnect>(Disconnect);
        }

        private void OnAcceptClient(Vdb.Session session)
        {
            Console.WriteLine("AcceptClient : " + session.client.Client.Handle);
        }

        private bool Connect(Vdb.Session session, ITW.Protocol.ToServer.Session.Connect packet)
        {
            Console.Write("Connect : " + packet.Id);
            return true;
        }

        private bool Disconnect(Vdb.Session session, ITW.Protocol.ToServer.Session.Disconnect packet)
        {
            Console.Write("Disconnect : " + session.client.Client.Handle);
            server.CloseSession(session);
            return true;
        }
    }
}