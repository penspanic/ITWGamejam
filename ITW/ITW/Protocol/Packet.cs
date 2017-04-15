using System.Runtime.Serialization;
namespace ITW.Protocol
{
    public abstract class Packet
    {
        /// <summary>
        /// 패킷방향::파일명::클래스명
        /// 예 : ToServer::Session::Session
        /// </summary>
        public string PacketName { get; protected set; }
        public Packet()
        {
            PacketName = "Packet";
        }
    }
}
