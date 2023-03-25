using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.PackageManager;
using UnityEngine;

public class NetInfo : MonoBehaviour
{
    public static NetInfo Instance { get; private set; }

    private int UDP_Port = 8889;
    private int TCP_Port = 8888;

    private int clientNum = 0;
    private string clientName = "";

    private string serverIP;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(Instance);

        DontDestroyOnLoad(this.gameObject);
    }

    // getters and setters
    public string GetIP() => serverIP;
    public int GetTCPPort() => TCP_Port;
    public int GetUDPPort() => UDP_Port;
    public int GetClientNum() => clientNum;
    public string GetName() => clientName;
    public void SetIP(string ip) => serverIP = ip;
    public void SetClient(int n) => clientNum = n;
    public void SetName(string name) => clientName = name;

}
