using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace ITWServer.Network
{
    public class PacketHandler
    {


        private Dictionary<Type, List<object>> bindedMethods = new Dictionary<Type, List<object>>();

        public delegate bool HandlerMethod<T>(Vdb.Session session, T packet) where T : ITW.Protocol.Packet;
        public void Bind<T>(HandlerMethod<T> method) where T : ITW.Protocol.Packet
        {
            if (bindedMethods.ContainsKey(typeof(T)) == false)
            {
                bindedMethods.Add(typeof(T), new List<object>());
            }

            bindedMethods[typeof(T)].Add(method);
        }

        public void Call<T>(Vdb.Session session, T packet) where T : ITW.Protocol.Packet
        {
            foreach (object obj in bindedMethods[packet.GetType()])
            {
                (obj as HandlerMethod<T>)(session, packet);
            }
        }
    }
}
