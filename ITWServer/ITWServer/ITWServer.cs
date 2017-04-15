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
            int read = session.client.GetStream().EndRead(ar);

            if(read > 0)
            {
                Array.Copy(session.readBuffer, session.packetBuffer, read);
                session.readed = read;
                session.client.GetStream().BeginRead(session.readBuffer, read, 4096 - read, new AsyncCallback(Read), session);
            }
            else
            {
                if(session.readed > 0 )
                {
                    Network.PacketParser.ParseAndDeliver(packetHandler, session, session.packetBuffer);
                    Array.Clear(session.packetBuffer, 0, 4096);
                    session.readed = 0;
                }
            }
        }
        
        public void CloseSession(Vdb.Session session)
        {
            session.client.Close();
            sessions.Remove(session);
        }
    }
}