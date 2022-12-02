using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    public GameObject mainMenu, startMenu, optionsMenu, tutorial;

    public Slider xSens, ySens, volume;
    public TMP_Text txt_xSens, txt_ySens, txt_volume;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("xSens")) return;
        xSens.value = PlayerPrefs.GetFloat("xSens");
        ySens.value = PlayerPrefs.GetFloat("ySens");

        txt_xSens.text = xSens.value.ToString();
        txt_ySens.text = ySens.value.ToString();
    }

    public void MainScreen()
    {
        mainMenu.SetActive(true);
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        tutorial.SetActive(false);
    }
    
    public void StartScreen()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        tutorial.SetActive(false);
    }
    
    public void OptionsScreen()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        tutorial.SetActive(false);
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
        SceneManager.LoadScene(3);
    }

    public void CreateLevel()
    {
        //map editor scene
        SceneManager.LoadScene(1);
    }

    public void ChangeXSens()
    {
        txt_xSens.text = xSens.value.ToString();
        //set player's sens 
        PlayerPrefs.SetFloat("xSens", xSens.value);
    }

    public void ChangeYSens()
    {
        txt_ySens.text = ySens.value.ToString();
        //set player's sens 
        PlayerPrefs.SetFloat("ySens", ySens.value);

    }

    public void ChangeVolume()
    {
        txt_volume.text = volume.value.ToString();
    }

    public void TutorialLevel()
    {
        mainMenu.SetActive(false);
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        tutorial.SetActive(true);
    }
}
