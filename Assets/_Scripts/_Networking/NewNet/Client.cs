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
    public static Mutex _mutex = new Mutex();

    private IPAddress ip;
    private EndPoint serverEP;

    private Socket client;

    private byte[] receiveBuffer = new byte[512];

    public TMP_InputField serverInput, nameInput;
    public TMP_Text roomTxt, clientsText;

    private int indicator = -1;
    private string msg = "";

    private bool startGame = false;

    private Queue<Action> executionQ = new Queue<Action>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);
    }

    private void UpdateObsolete()
    {
        if (startGame) StartCoroutine(StartGame());

        if (indicator == -1) return;
        //Dbuggr.instance.AddText(indicator.ToString() + msg);
        switch (indicator)
        {
            case 0:
                // client joining message
                //Dbuggr.instance.AddText(indicator.ToString() + msg);
                indicator = -1;
                clientsText.text += msg + '\n';
                msg = "";
                break;
            case 1:
            case 2:
            case 3:
                // chat message
                indicator = -1;
                //chathandler.instance.addtext(indicator, msg);
                break;
            case 4:
                // set seed
                indicator = -1;
                Debug.Log("Setting seed " + int.Parse(msg));
                int seed;
                if (int.TryParse(msg, out seed))
                {
                    NetInfo.Instance.SetSeed(seed);
                }
                else
                {
                    Debug.Log("Setting seed to 0");
                    NetInfo.Instance.SetSeed(0);
                }
                //NetInfo.Instance.SetSeed(
                //    int.Parse(
                //        msg)); // if i don't split this up then it gives an error idk why 
                Debug.Log("Done setting seed");
                msg = "";
                break;
            case 5:
                // game start
                //Dbuggr.instance.AddText("Starting game");
                indicator = -1;
                msg = "";
                SceneManager.LoadScene(2);
                break;
        }

    }

    private void Update()
    {
        if (executionQ.Count < 1) return;
        executionQ.Dequeue().Invoke();
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

        string msg = "0^" + NetInfo.Instance.GetName();
        client.Send(Encoding.ASCII.GetBytes(msg));

        client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), client);

        roomTxt.text = "Conencted to server: " + client.RemoteEndPoint + " as " + client.LocalEndPoint;
    }

    // on click create
    public void ConnectAsHost()
    {
        NetInfo.Instance.SetClient(1);
        //Invoke("ConnectAsHostDelayed", 0.5f);
        StartCoroutine(ConnectAsHostDelayed());
    }

    private IEnumerator ConnectAsHostDelayed()
    {
        // because i was too lazy to change it from buttom click, forgive me
        yield return new WaitForSeconds(0.5f);
        ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == AddressFamily.InterNetwork); // local ipv4
        NetInfo.Instance.SetName(Dns.GetHostName());

        serverEP = new IPEndPoint(ip, NetInfo.Instance.GetTCPPort());

        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(serverEP);

        string msg = "0^" + NetInfo.Instance.GetName();
        client.Send(Encoding.ASCII.GetBytes(msg));

        client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), client);

        roomTxt.text = "Connected to server: " + ip.ToString();
    }

    private void ReceiveCallBack(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int recv = socket.EndReceive(result);

        Debug.Log("FROM CLIENT Received number : " + recv);

        string msg = Encoding.ASCII.GetString(receiveBuffer);
        if (!msg.Contains('^')) goto restartCallBack;
        string[] split = msg.Split('^');

        //this.msg = "FROM CLIENT " + split[1];
        Debug.Log("FROM CLIENT Received indicator: " + split[0] + " with message: " + split[1]);
        //Dbuggr.instance.AddText("FROM CLIENT Received message: " + split[1]);

        if (split[0] == "$")
        {
            NetInfo.Instance.SetClient(int.Parse(split[1]));
        }

        // game start indication
        if (split[0] == "5")
        {
            Debug.Log("Starting game");
            //startGame = true;
            executionQ.Enqueue(StartGameFunc);
            //UnityMainThread.Instance().Enqueue(() => SceneManager.LoadSceneAsync(2));
            Debug.Log("done Starting game");
            indicator = 5;
        }

        // client joining message
        if (split[0] == "0")
        {
            indicator = 0;
            this.msg = split[1];
            executionQ.Enqueue(AppendTextJoining);
        }

        // text messages for chat
        if (split[0] == "1" || split[0] == "2" || split[0] == "3")
        {
            // send to chat handler
            //indicator = int.Parse(split[0]);
            //this.msg = split[1];
            //executionQ.Enqueue(UpdateTextChat);
            // chathandler.instance.addText(split[1]);
        }

        // set game seed
        if (split[0] == "4")
        {
            indicator = 4;
            this.msg = split[1];
            executionQ.Enqueue(SetSeed);
        }
            //Debug.Log("Setting seed: " + split[1]);
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
restartCallBack:
        receiveBuffer = new byte[512];
        Debug.Log("FROM CLIENT Restarting receive");
        client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), client);
    }

    private void StartGameFunc()
    {
        Debug.Log("Starting game in the function");
        SceneManager.LoadScene(1);
    }

    private void AppendTextJoining()
    {
        clientsText.text += msg + '\n';
    }

    private void SetSeed()
    {
        int seed;
        if (int.TryParse(msg, out seed))
        {
            NetInfo.Instance.SetSeed(seed);
        }
        else
        {
            Debug.Log("Setting seed to 0");
            NetInfo.Instance.SetSeed(0);
        }
    }

    private void UpdateTextChat()
    {

    }

    private IEnumerator StartGame()
    {
        Debug.Log("Starting game corout");
        startGame = false;
        yield return null;
        SceneManager.LoadScene(2);
    }

    private IEnumerator AppentText(string m)
    {
        yield return null;
        clientsText.text += m + '\n';
    }

    private IEnumerator SetGameSeed(int n)
    {
        yield return null;
        NetInfo.Instance.SetSeed(n);
    }

    public void SendMessageTCP(string msg)
    {
        byte[] send = Encoding.ASCII.GetBytes(msg);
        client.Send(send);
    }

    private void OnApplicationQuit()
    {
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }
}
