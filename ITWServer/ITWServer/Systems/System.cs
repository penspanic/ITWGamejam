using ITW.Network;
namespace ITWServer.Systems
{
    public abstract class System
    {
        protected Network.PacketHandler packetHandler;
        public System(Network.PacketHandler packetHandler)
        {
            this.packetHandler = packetHandler;
        }
    }
}
