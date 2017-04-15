using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class NetworkManager : Singleton<NetworkManager>
{
    public string hostName;
    public int port;

    private TcpClient client;
    private bool initialized = false;

    #region EventHandler
    public event Action OnConnect;
    #endregion

    private void Awake()
    {
        client = new TcpClient();
        client.BeginConnect(hostName, port, new AsyncCallback(Connected), client);
    }

    public void Send<T>(T packet) where T : ITW.Protocol.Packet
    {
        //packet.serial
        //byte[] data = formatter.Serialize()
        //client.GetStream().BeginWrite()
    }

    private void Connected(IAsyncResult ar)
    {
        TcpClient client = (TcpClient)ar.AsyncState;
        client.EndConnect(ar);
        initialized = true;

        OnConnect();
    }
}
