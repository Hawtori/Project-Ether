using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

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
    private Vector3 remotePosition;
    private Vector3 remoteRotation;
    private Vector3 remoteVelocity;
    private int remoteHealth = 4;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        if(NetInfo.Instance == null)
        {
            Destroy(this);
            return;
        } 

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
        inputs = PlayerMovement.Instance.GetMovement();
        if (inputs.magnitude > 0) flag = true;

        // prediction
        remotePosition += remoteVelocity * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        remotePlayer.transform.position = remotePosition;
        remotePlayer.transform.rotation = Quaternion.Euler(remoteRotation);
        //remotePlayer.GetComponent<Rigidbody>().MovePosition(remotePosition);
        //remotePlayer.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(remoteRotation));
        //remotePlayer.GetComponent<Rigidbody>().velocity = remoteVelocity;
        remotePlayer.GetComponent<Health>().SetHealth(remoteHealth);
    }

    private void ReceiveUpdates()
    {
        while (true)
        {
            byte[] recbuff = new byte[512];
            _ = socket.ReceiveFrom(recbuff, ref serverEP);

            recvMsg = Encoding.ASCII.GetString(recbuff);

            string[] msg = recvMsg.Split('^');
            string[] bPos = msg[0].Split(',');
            string[] bRot = msg[1].Split(',');
            string[] bVel = msg[2].Split(',');
<<<<<<< Updated upstream
                Debug.LogWarning(" > " + msg[3][0]);
            try
            {
                remoteHealth = float.Parse(msg[3]);
            }
            catch (Exception e)
            {
                remoteHealth = 1;
            }
=======
            remoteHealth = int.Parse(msg[3]);
>>>>>>> Stashed changes

            float[] pos = new float[3];
            float[] rot = new float[3];
            float[] vel = new float[3];
            
            for(int i = 0; i < 3; i++)
            {
                pos[i] = float.Parse(bPos[i]);
                rot[i] = float.Parse(bRot[i]);
                vel[i] = float.Parse(bVel[i]);
            }

            remotePosition = new Vector3(pos[0], pos[1], pos[2]);
            remoteRotation = new Vector3(rot[0], rot[1], rot[2]);
            remoteVelocity = new Vector3(vel[0], vel[1], vel[2]);

            Debug.Log("Received: " + remotePosition + " : " + remoteRotation);
        }
    }

    IEnumerator SendUpdates()
    {
        while (true)
        {
            yield return new WaitForSeconds(netRefreshRate);
            if (!flag) continue;

            buffer = new byte[512];
            Rigidbody rb = localPlayer.GetComponent<Rigidbody>();

            sendMsg = localPlayer.transform.position.x + "," + localPlayer.transform.position.y + "," + localPlayer.transform.position.z;         // position
            sendMsg += "^" + localPlayer.transform.rotation.x + "," + localPlayer.transform.rotation.y + "," + localPlayer.transform.rotation.z;  // rotation
            sendMsg += "^" + rb.velocity.x + "," + rb.velocity.y + "," + rb.velocity.z;                                                           // velocity
            sendMsg += "^" + localPlayer.gameObject.GetComponent<hurtplayer>().hitPoint.ToString();                                               // health

            buffer = Encoding.ASCII.GetBytes(sendMsg);

            Debug.Log("Sending: " + sendMsg);

            socket.SendTo(buffer, remoteEP);
            flag = false;
        }
    }

}
