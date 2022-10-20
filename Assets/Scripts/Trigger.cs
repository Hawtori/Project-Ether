using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject crosshair;
    public GameObject text;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        other.transform.position = Vector3.zero + Vector3.up * 2f;
        ShowText();
    }

    private void ShowText()
    {
        text.SetActive(true);
        crosshair.SetActive(false);
        Time.timeScale = 0f;
        Invoke("HideText", 0.5f);

    }

    private void HideText()
    {
     
        Time.timeScale = 1f;
        text.SetActive(false);
        crosshair.SetActive(true);
    }
}
