using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Linq;

public class CreateServer : MonoBehaviour
{
    public static CreateServer Instance { get; private set; }

    private Socket tcpSocket;
    private Socket udpSocket;

    private List<Socket> clientSockets = new List<Socket>();
    private List<EndPoint> clientEPs = new List<EndPoint>();

    private Thread tcpThread;
    private Thread udpThread;

    private IPEndPoint tcpEP;
    private IPEndPoint udpEP;

    private EndPoint remoteClient;

    private byte[] buffer = new byte[512];
    private byte[] sendBuffer = new byte[512];
    private byte[] posBuffer = new byte[512];

    private string sendMsg;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        DontDestroyOnLoad(gameObject);
    }

    public void Server()
    {
        StartCoroutine(StartServer());
    }

    private IEnumerator StartServer()
    {
        yield return null;
        IPAddress ip = IPAddress.Parse(GetLocalIPv4());
        NetInfo.Instance.SetIP(ip.ToString());

        tcpEP = new IPEndPoint(ip, NetInfo.Instance.GetTCPPort());
        tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        tcpThread = new Thread(() =>
        {
            tcpSocket.Bind(tcpEP);
            tcpSocket.Listen(10);
            tcpSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        });
        tcpThread.Start();

        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpEP = new IPEndPoint(ip, NetInfo.Instance.GetUDPPort());
        udpSocket.Bind(udpEP);

        remoteClient = new IPEndPoint(IPAddress.Any, 0);

        udpThread = new Thread(() =>
        {
            while (true) {
                // receive position update
                int recv = udpSocket.ReceiveFrom(posBuffer, ref remoteClient);

                if (!clientEPs.Contains((IPEndPoint)remoteClient))
                {
                    clientEPs.Add((IPEndPoint)remoteClient);
                }

                // send it to other client
                foreach(var client in clientEPs)
                {
                    if ((client) != (remoteClient))
                        udpSocket.SendTo(posBuffer, client);
                }
            }
        });
        udpThread.Start();

    }

    #region TCP

    private void AcceptCallBack(IAsyncResult result)
    {
        Socket socket = tcpSocket.EndAccept(result);
        if (!clientSockets.Contains(socket))
            clientSockets.Add(socket);
        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallBack), socket);

        //Debug.Log($"Client connected (TCP)! Currently {clientSockets.Count} clients are connected.");

        foreach(var client in clientSockets)
        {
            string m = "4^" + (DateTime.Now.Millisecond).ToString();
            client.Send(Encoding.ASCII.GetBytes(m));
            //Debug.Log("Client : " + client.RemoteEndPoint);
            //Dbuggr.instance.AddText("client: " + client.RemoteEndPoint);
        }

        if(clientSockets.Count < 3)
        tcpSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
    }

    private void ReceiveCallBack(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int recv = socket.EndReceive(result);
        byte[] data = new byte[recv];

        //Array.Copy(buffer, data, recv);

        string msg = Encoding.ASCII.GetString(buffer);
        string indicator = msg.Split('^')[0];

        Debug.Log("Received message: " + msg);

        foreach (var soc in clientSockets)
        {
            // if it's a gameplay message, send only to second player
            if (indicator == "!" || indicator == "@")
            {
                if (soc.RemoteEndPoint != socket.RemoteEndPoint)
                    soc.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallBack), soc);
            }
            else if (indicator == "1" || indicator == "2" || indicator == "3" || indicator == "4" || indicator == "5" || indicator == "0")
            { // its a chat message so send to all
                Debug.Log("Sending message to all, currently: " + soc.RemoteEndPoint);
                soc.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallBack), soc);
            }
        }
        buffer = new byte[512];
        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallBack), socket);
    }

    private void SendCallBack(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        socket.EndSend(result);
    }
    #endregion

    private void OnApplicationQuit()
    {
        EndServer();
    }

    public void EndServer()
    {
        tcpSocket.Shutdown(SocketShutdown.Both);
        tcpSocket.Close();

        clientSockets.Clear();
        clientEPs.Clear();
    }
    public string GetLocalIPv4() => Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
}
