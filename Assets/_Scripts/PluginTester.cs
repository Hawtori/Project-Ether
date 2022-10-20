using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PluginTester : MonoBehaviour
{
    [DllImport("SaveGame.dll")]
    private static extern void SaveGameData();

    [DllImport("SaveGame.dll")]
    private static extern int GetID();

    [DllImport("SaveGame.dll")]
    private static extern void SetID(int id);

    [DllImport("SaveGame.dll")]
    private static extern int GetPosition();

    [DllImport("SaveGame.dll")]
    private static extern void SetPosition(float x, float y, float z);

    [DllImport("SaveGame.dll")]
    private static extern void SetFilePath(string path);

    public string filePath;
    public GameObject environment;
    private List<int> id = new();
    private List<Vector3> pos = new();
    void Start()
    {
        SetFilePath(filePath);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            SetFilePath(filePath);
            id.Add(1);
            pos.Add(environment.transform.position);
            id.ToArray();
            pos.ToArray();
            SetID(id[0]);
            SetPosition(pos[0].x, pos[0].y, pos[0].z);
            SaveGameData();
        }
    }
}
