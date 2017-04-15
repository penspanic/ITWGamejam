using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ITWServer
{
    class ITWServer
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
            systems.Add(new Systems.SessionSystem(packetHandler));
            Console.WriteLine("Systems created!");
        }

        private void Accept(IAsyncResult ar)
        {
            TcpListener tcpListener = (TcpListener)ar.AsyncState;
            Socket client = tcpListener.AcceptSocket();
            Vdb.Session newSession = new Vdb.Session();
            newSession.socket = client;
            newSession.readBuffer = new byte[4096];
            sessions.Add(newSession);
            client.BeginReceive(newSession.readBuffer, 0, 4096, 0, new AsyncCallback(Read), newSession);
        }

        private void Read(IAsyncResult ar)
        {
            Vdb.Session session = (Vdb.Session)ar.AsyncState;
            int read = session.socket.EndReceive(ar);

            if(read > 0)
            {
                Array.Copy(session.readBuffer, session.packetBuffer, read);
                session.readed = read;
                session.socket.BeginReceive(session.readBuffer, 0, 4096, 0, new AsyncCallback(Read), session);
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
    }
}