using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ChatHandler : MonoBehaviour
{
    public static ChatHandler instance;

    public TMP_InputField input;
    public TMP_Text chatHistory;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    // on send click
    public void SendMessage()
    {
        string msg = (NetInfo.Instance.GetClientNum()+1).ToString("0")+"^"; 
            msg += input.text;

        Client.Instance.SendMessageTCP(msg);

        input.text = "";
    }

    public void ReceiveMessage(string msg)
    {
        chatHistory.text = "CHAT\n" + msg;
    }
}
