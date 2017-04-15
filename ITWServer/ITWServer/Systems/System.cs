using ITW.Network;
namespace ITWServer.Systems
{
    public abstract class System
    {
        protected ITW.Network.PacketHandler packetHandler;
        public System(ITW.Network.PacketHandler packetHandler)
        {
            this.packetHandler = packetHandler;
        }
    }
}
