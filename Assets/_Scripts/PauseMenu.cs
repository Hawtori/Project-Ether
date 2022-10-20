using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject InGame;
    public GameObject player;

    public Slider xSens, ySens;
    public TMP_Text txt_xSens, txt_ySens;

    private bool Paused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Paused = !Paused;
        
        if (Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Menu.SetActive(true);
            InGame.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Menu.SetActive(false);
            InGame.SetActive(true);
            Time.timeScale = 1;
        }
    }

    public void ChangeXSens()
    {
        txt_xSens.text = xSens.value.ToString();
        player.GetComponent<PlayerMovement>().xSens = xSens.value;
    }
    
    public void ChangeYSens()
    {
        txt_ySens.text = ySens.value.ToString();
        player.GetComponent<PlayerMovement>().ySens = ySens.value;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
