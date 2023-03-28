using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBlanksMenu : MonoBehaviour
{
    public GameObject startButton, clientList;

    private void OnEnable()
    {
        if (NetInfo.Instance == null) Invoke(nameof(Enable), 1f);
        else Enable();
    }

    private void Enable()
    {
        if (NetInfo.Instance.GetClientNum() == 0)
        {
            //disable start button and update text
            Debug.Log("Client 0, disable stuff");
            startButton.SetActive(false);
            clientList.SetActive(false);
        }
    }
}
