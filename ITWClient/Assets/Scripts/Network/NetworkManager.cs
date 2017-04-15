using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class NetworkManager : MonoBehaviour
{
    #region Singleton
    private static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    public string hostName;
    public int port;

    private TcpClient client;
    private bool initialized = false;

    #region EventHandler
    public event Action OnConnect;
    #endregion

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        client = new TcpClient();
        client.BeginConnect(hostName, port, new AsyncCallback(Connected), client);
    }

    public void Send<T>(T packet) where T : ITW.Protocol.Packet
    {
        System.IO.MemoryStream stream = new System.IO.MemoryStream();
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, packet);

        int size = stream.GetBuffer().Length;
        client.GetStream().BeginWrite(stream.GetBuffer(), 0, size, new AsyncCallback(WriteEnd), null);
    }

    private void WriteEnd(IAsyncResult ar)
    {

    }

    private void Connected(IAsyncResult ar)
    {
        TcpClient client = (TcpClient)ar.AsyncState;
        client.EndConnect(ar);
        initialized = true;

        Debug.Log("Connected to server!");

        Debug.Log("Send Connect...");
        ITW.Protocol.ToServer.Connect packet = new ITW.Protocol.ToServer.Connect();
        packet.Id = "1";
        Send(packet);

        OnConnect();
    }
}