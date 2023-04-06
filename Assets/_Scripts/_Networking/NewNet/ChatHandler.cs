using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ChatHandler : MonoBehaviour
{
    public static ChatHandler instance;

    public TMP_InputField input;
    public TMP_Text chatHistory;
    public Client client;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    // on send click
    public void SendMessage()
    {
        client.SendMessageTCP(input.text);
    }

    public void ReceiveMessage(string msg)
    {
        chatHistory.text = "CHAT\n" + msg;
    }
}
