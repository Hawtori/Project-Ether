// Click connect, take in input from name and ip fields
// save those to netinfo
// connect to server using given ip 
// change code text to "Connected to server: {server ip}
// send tcp message {0, name}
// receive messge {0, name}
// receive message {5, "Start"} -> go next scene
// receive message {1/2, msg} -> display in chat || work on this later

// disable start button and client list text 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System.Text;

public class Client : MonoBehaviour
{
    public static Client Instance;

    private IPAddress ip;
    private EndPoint serverEP;

    private Socket client;

    private byte[] receiveBuffer = new byte[512];

    public TMP_InputField serverInput, nameInput;
    public TMP_Text roomTxt, clientsText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    // on click join
    public void ConnectToServer()
    {
        NetInfo.Instance.SetName(nameInput.text);
        NetInfo.Instance.SetIP(serverInput.text);

        ip = IPAddress.Parse(serverInput.text);

        serverEP = new IPEndPoint(ip, NetInfo.Instance.GetTCPPort());

        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(serverEP);

        string msg = "0\\" + NetInfo.Instance.GetName();
        client.Send(Encoding.ASCII.GetBytes(msg));

        client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), client);

        roomTxt.text = "Conencted to server: " + client.RemoteEndPoint + " as " + client.LocalEndPoint;
    }

    // on click create
    public void ConnectAsHost()
    {
        NetInfo.Instance.SetClient(1);
        Invoke("ConnectAsHostDelayed", 0.5f);
    }

    private void ConnectAsHostDelayed()
    {
        // because i was too lazy to change it from buttom click, forgive me

        ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == AddressFamily.InterNetwork); // local ipv4
        NetInfo.Instance.SetName(Dns.GetHostName());

        serverEP = new IPEndPoint(ip, NetInfo.Instance.GetTCPPort());

        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(serverEP);

        string msg = "0\\" + NetInfo.Instance.GetName();
        client.Send(Encoding.ASCII.GetBytes(msg));

        client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), client);

        roomTxt.text = "Connected to server: " + ip.ToString();
    }

    private void ReceiveCallBack(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int recv = socket.EndReceive(result);

        string msg = Encoding.ASCII.GetString(receiveBuffer);
        string[] split = msg.Split('\\');

        //this.msg = "FROM CLIENT " + split[1];
        Debug.Log("FROM CLIENT Received message: " + split[1]);
        //Dbuggr.instance.AddText("FROM CLIENT Received message: " + split[1]);

        // game start indication
        if (split[0] == "5") SceneManager.LoadScene(3);
            //Dbuggr.instance.AddText("Received good to go to start game");
        // client joining message
        if (split[0] == "0") clientsText.text += split[1] + '\n';

        // text messages for chat
        if (split[0] == "1" || split[0] == "2" || split[0] == "3")
        {
            // send to chat handler
            // chathandler.instance.addText(split[1]);
        }

        // enemy damaged
        if (split[0] == "!")
        {
            // damage enemy
            // enemylist.damage enemy at int.parse(split[1]) for float.parse(split[2])
        }

        // game state changed 
        if (split[0] == "@")
        {
            // change somethinga bout game 
        }

        receiveBuffer = new byte[512];
        client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), client);
    }

    public void SendMessageTCP(string msg)
    {
        byte[] send = Encoding.ASCII.GetBytes(msg);
        client.Send(send);

    }
}
