using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [DllImport("LoadGame.dll")]
    private static extern void LoadGameData();

    [DllImport("LoadGame.dll")]
    private static extern int GetID();

    [DllImport("LoadGame.dll")]
    private static extern int GetPosition();

    [DllImport("LoadGame.dll")]
    private static extern void SetFilePath(string path);

    public string filePath;
    public GameObject environment;
    private List<int> id = new();
    private List<Vector3> pos = new();
    // Start is called before the first frame update
    void Start()
    {
        SetFilePath(filePath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F6))
        {
            LoadGameData();
            //code that would update the position of all objects to what was in the saved game data file
        }
    }
}
