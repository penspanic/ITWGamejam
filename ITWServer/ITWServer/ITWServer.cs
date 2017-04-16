using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ITWServer
{
    public class ITWServer
    {
        private List<Systems.System> systems = new List<Systems.System>();
        private TcpListener listener;
        private List<Vdb.Session> sessions = new List<Vdb.Session>();
        private Network.PacketHandler packetHandler = new Network.PacketHandler();
        public void Start(string[] args)
        {
            LoadSdb();
            CreateSystems();

            int listenPort = ITW.Config.ConfigManager.GetInt("session", "listenport");
            Console.WriteLine("ListenPort : " + listenPort);
            listener = new TcpListener(IPAddress.Any, listenPort);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(Accept), listener);
            while (true)
            {
                System.Threading.Thread.Sleep(10);
            }
        }

        private void LoadSdb()
        {
            Console.WriteLine("Load Sdb end!");
        }

        private void CreateSystems()
        {
            systems.Add(new Systems.SessionSystem(this, packetHandler));
            Console.WriteLine("Systems created!");
        }

        public delegate void AcceptClient(Vdb.Session session);
        public event AcceptClient OnAcceptClient;
        private void Accept(IAsyncResult ar)
        {
            TcpListener tcpListener = (TcpListener)ar.AsyncState;
            TcpClient client = tcpListener.EndAcceptTcpClient(ar);
            Vdb.Session newSession = new Vdb.Session();
            newSession.client = client;
            sessions.Add(newSession);
            OnAcceptClient(newSession);
            client.GetStream().BeginRead(newSession.readBuffer, 0, 4096, new AsyncCallback(Read), newSession);
        }

        private void Read(IAsyncResult  ar)
        {
            Vdb.Session session = (Vdb.Session)ar.AsyncState;
            int readed = session.client.GetStream().EndRead(ar);
            
            // Packet Header
            if(readed < 0)
            {
                return;
            }

            Byte[] sizeBuffer = new Byte[4];
            Array.Copy(session.readBuffer, sizeBuffer, 4);
            int packetSize = BitConverter.ToInt32(sizeBuffer, 0);

            if(readed < packetSize + 4)
            {
                return;
            }

            byte[] packetBuffer = new byte[packetSize];
            Array.Copy(session.readBuffer, 4, packetBuffer, 0, packetSize);
            Network.PacketParser.ParseAndDeliver(packetHandler, session, packetBuffer);
        }
        
        public void CloseSession(Vdb.Session session)
        {
            session.client.Close();
            sessions.Remove(session);
        }
    }
}