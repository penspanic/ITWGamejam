using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace ITW.Network
{
    public class PacketHandler
    {


        private Dictionary<Type, List<object>> bindedMethods;

        public delegate bool HandlerMethod<T>(T packet) where T : Protocol.Packet;
        public void Bind<T>(HandlerMethod<T> method) where T : Protocol.Packet
        {
            bindedMethods[typeof(T)].Add(method);

        }

        public void Call<T>(T packet) where T : Protocol.Packet
        {
            foreach(object obj in bindedMethods[packet.GetType()])
            {
                (obj as HandlerMethod<T>)(packet);
            }
        }
    }
}
