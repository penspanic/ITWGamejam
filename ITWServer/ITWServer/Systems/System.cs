using ITW.Network;
namespace ITWServer.Systems
{
    public abstract class System
    {
        protected ITWServer server;
        protected Network.PacketHandler packetHandler;
        public System(ITWServer server, Network.PacketHandler packetHandler)
        {
            this.server = server;
            this.packetHandler = packetHandler;
        }
    }
}
