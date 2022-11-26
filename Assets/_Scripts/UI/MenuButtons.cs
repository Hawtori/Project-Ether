using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    public GameObject mainMenu, startMenu, optionsMenu;

    public Slider xSens, ySens, volume;
    public TMP_Text txt_xSens, txt_ySens, txt_volume;

    public void MainScreen()
    {
        mainMenu.SetActive(true);
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
    
    public void StartScreen()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    
    public void OptionsScreen()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    
    public void CreditsScreen()
    {
        Debug.Log("No credits yet ");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitted game");
        Application.Quit();
    }

    public void JoinGame()
    {
        //change this so we show them a screen where they can join an existing game
        SceneManager.LoadScene(3);
    }
    
    public void CreateGame()
    {
        //show a screen where they can create a game
    }
    
    public void CreateLevel()
    {
        //map editor scene
    }

    public void ChangeXSens()
    {
        txt_xSens.text = xSens.value.ToString();
        //set player's sens 
    }

    public void ChangeYSens()
    {
        txt_ySens.text = ySens.value.ToString();
        //set player's sens 
    }

    public void ChangeVolume()
    {
        txt_volume.text = volume.value.ToString();
    }

    public void TutorialLevel()
    {
        SceneManager.LoadScene(1);
    }
}
