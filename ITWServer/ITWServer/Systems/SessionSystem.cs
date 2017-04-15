
namespace ITWServer.Systems
{
    public class SessionSystem : System
    {

        public SessionSystem(ITW.Network.PacketHandler packetHander) : base(packetHander)
        {
            packetHander.Bind<ITW.Protocol.ToServer.Connect>(Connect);
        }

        private bool Connect(ITW.Protocol.ToServer.Connect packet)
        {
            return true;
        }
    }
}
