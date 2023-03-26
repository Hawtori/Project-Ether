using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // start, options, credit, join, create
    public GameObject[] menus;

    public Slider xSens, ySens, volume;
    public TMP_Text xSensTxt, ySensTxt, volumeTxt;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("xSens")) return;
        xSens.value = PlayerPrefs.GetFloat("xSens");
        ySens.value = PlayerPrefs.GetFloat("ySens");
        volume.value = PlayerPrefs.GetFloat("volume");

        xSensTxt.text = xSens.value.ToString("#.##");
        ySensTxt.text = ySens.value.ToString("#.##");
        volumeTxt.text = volume.value.ToString("#.##");
    }

    public void SetScreen(int index)
    {
        for(int i = 0; i < menus.Length; i++) menus[i].SetActive(false);
        menus[index].SetActive(true);
    }

    public void StartGame()
    {
        // change scene to game
        SceneManager.LoadScene(3);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeSensX()
    {
        xSensTxt.text = xSens.value.ToString("#.##");
        PlayerPrefs.SetFloat("xSens", xSens.value);
    }

    public void ChangeSensY()
    {
        ySensTxt.text = ySens.value.ToString("#.##");
        PlayerPrefs.SetFloat("ySens", ySens.value);
    }

    public void ChangeVolume()
    {
        volumeTxt.text = volume.value.ToString("#.##");
        PlayerPrefs.SetFloat("volume", volume.value);
    }
}
