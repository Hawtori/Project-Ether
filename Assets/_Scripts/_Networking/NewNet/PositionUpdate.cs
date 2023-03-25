using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using static UnityEditor.PlayerSettings;
using System.Diagnostics;

public class PositionUpdate : MonoBehaviour
{
    public static PositionUpdate Instance;

    public GameObject localPlayer, remotePlayer;

    private EndPoint serverEP;
    private EndPoint remoteEP;
    private Socket socket;

    private byte[] buffer = new byte[512];
    private string recvMsg;
    private string sendMsg;

    private float netRefreshRate = 0.4f;
    private bool flag = true;

    private Vector2 inputs;
    private Vector3 position;
    private Vector3 remotePosition;
    private Vector3 velocity;
    private Vector3 remoteVelocity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        serverEP = new IPEndPoint(IPAddress.Any, 0);
        remoteEP = new IPEndPoint(IPAddress.Parse(NetInfo.Instance.GetIP()), NetInfo.Instance.GetUDPPort());
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        Thread receiveUpdate = new Thread(ReceiveUpdates);
        receiveUpdate.Start();
        receiveUpdate.Name = "Receive Updates";

        socket.Connect(remoteEP);

        StartCoroutine(SendUpdates());
    }

    private void Update()
    {
        inputs = PlayerMovement.instance.GetMovement();
        if (inputs.magnitude > 0) flag = true;
    }

    private void FixedUpdate()
    {
        remotePlayer.GetComponent<Rigidbody>().MovePosition(remotePosition);
        remotePlayer.GetComponent<Rigidbody>().velocity = remoteVelocity;
    }

    private void ReceiveUpdates() // good now
    {
        while (true)
        {
            byte[] recbuff = new byte[512];
            int recv = socket.ReceiveFrom(recbuff, ref serverEP);

            recvMsg = Encoding.ASCII.GetString(recbuff);

            string[] msg = recvMsg.Split(',');
            float[] pos = new float[3];
            float[] vel = new float[3];
            
            for(int i = 0; i < 3; i++)
                pos[i] = float.Parse(msg[i]);
            for (int i = 0, j = 3; i < 3; i++)
                vel[i] = float.Parse(msg[j]);

            remotePosition = new Vector3(pos[0], pos[1], pos[2]);
            remoteVelocity = new Vector3(vel[0], vel[1], vel[2]);
        }
    }

    IEnumerator SendUpdates() // should be good
    {
        while (true)
        {
            yield return new WaitForSeconds(netRefreshRate);
            if (!flag) continue;

            buffer = new byte[512];
            sendMsg = "";
            Rigidbody rb = localPlayer.GetComponent<Rigidbody>();

            sendMsg += localPlayer.transform.position.x + ", " + localPlayer.transform.position.y + ", " + localPlayer.transform.position.z;
            sendMsg += ", " + rb.velocity.x + ", " + rb.velocity.y + ", " + rb.velocity.z;

            buffer = Encoding.ASCII.GetBytes(sendMsg);

            socket.SendTo(buffer, remoteEP);
            flag = false;
        }
    }

}
