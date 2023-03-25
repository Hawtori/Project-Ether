using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Net;
using System.Net.Sockets;
using System;

public class JoinGame : MonoBehaviour
{
    private Socket client;
    private EndPoint serverEP;

    public TMP_Text codeTxt;

    private void OnEnable()
    {
        serverEP = new IPEndPoint(IPAddress.Parse(NetInfo.Instance.GetIP()), NetInfo.Instance.GetUDPPort());
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(serverEP);

        codeTxt.text = "IP: " + NetInfo.Instance.GetIP();
    }
}
