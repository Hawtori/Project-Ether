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

    private void OnEnable()
    {
        StartServer();
    }

    private void StartServer()
    {
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
                int recv = udpSocket.ReceiveFrom(posBuffer, ref remoteClient);

                if (!clientEPs.Contains((IPEndPoint)remoteClient))
                {
                    clientEPs.Add((IPEndPoint)remoteClient);
                }

                foreach(var client in clientEPs)
                {
                    if (((IPEndPoint)client).Port != ((IPEndPoint)remoteClient).Port)
                        udpSocket.SendTo(posBuffer, client);
                }
            }
        });
    }

    #region TCP

    private void AcceptCallBack(IAsyncResult result)
    {
        Socket socket = tcpSocket.EndAccept(result);
        if (!clientSockets.Contains(socket))
            clientSockets.Add(socket);
        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallBack), socket);

        Debug.Log($"Client connected (TCP)! Currently {clientSockets.Count} clients are connected.");

        //if(clientSockets.Count > 1)
        {
            // send a package upon connecting that says the client's name
            //clientTxt.text = "Client " + Dns.GetHostName() /*Encoding.ASCII.GetString(buffer)*/ + " has connected! " + clientSockets.Count;
        }

        tcpSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
    }

    private void ReceiveCallBack(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int recv = socket.EndReceive(result);
        byte[] data = new byte[recv];


        Array.Copy(buffer, data, recv);

        // depending on which client sent, add an indicator and concatinate the message, send to all clients
        // if it says 1 then msg, show it as client 1, if it says 2, show it as client 2, if it says 3, show it as system

        string msg = Encoding.ASCII.GetString(data);
        //Console.WriteLine($"Revceived {msg} from " + socket.RemoteEndPoint.ToString());

        //dbug.text = "Received: " + msg;
        //
        //if (msg[0] != '1' || msg[0] != '2' || msg[0] != '3')
        //    clientTxt.text += "\nClient " + msg + " has connected!";

        if (socket.RemoteEndPoint.ToString() == clientSockets[1].RemoteEndPoint.ToString()) sendMsg = "1" + msg + " ";
        else sendMsg = "2" + msg + " ";

        sendBuffer = new byte[512];
        sendBuffer = Encoding.ASCII.GetBytes(sendMsg);

        foreach (var soc in clientSockets)
        {
            Console.WriteLine("Sending '{0}' to: " + soc.RemoteEndPoint.ToString(), sendMsg);
            soc.BeginSend(sendBuffer, 0, sendBuffer.Length, 0, new AsyncCallback(SendCallBack), soc);
        }

        //sendMsg = "";
        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallBack), socket);
    }

    private void SendCallBack(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        socket.EndSend(result);
    }
    #endregion

    #region UDP


    #endregion

    public void EndServer()
    {
        tcpSocket.Shutdown(SocketShutdown.Both);
        tcpSocket.Close();

        clientSockets.Clear();
        clientEPs.Clear();
    }
    public string GetLocalIPv4() => Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
}
