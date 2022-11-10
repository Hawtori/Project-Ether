using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempSceneSwap : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) SceneManager.LoadScene(1);
    }
}
