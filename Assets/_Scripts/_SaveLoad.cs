using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

using System.IO;
using System.Linq;

public class _SaveLoad : MonoBehaviour
{

    [DllImport("Project_Ether_DLL")] private static extern int GetID();

    [DllImport("Project_Ether_DLL")] private static extern void SetID(int id);

    [DllImport("Project_Ether_DLL")] private static extern Vector3 GetPosition();

    [DllImport("Project_Ether_DLL")] private static extern void SetPosition(float x, float y, float z);

    [DllImport("Project_Ether_DLL")] private static extern void SaveToFile(int id, float x, float y, float z);

    [DllImport("Project_Ether_DLL")] private static extern void StartWriting(string filename);

    [DllImport("Project_Ether_DLL")] private static extern void EndWriting();




    //[DllImport("Plugin")] private static extern void


    string m_path, fn;
    public GameObject wow;


    // Start is called before the first frame update
    void Start()
    {
        SetPosition(1, 1, 1);
        SetID(1000);

        //m_path = Application.dataPath;

       // fn = m_path + "/Saves/WoahFile.txt";
         fn = Application.persistentDataPath + "/SavePlayerPosition.txt";
        
    }

    void savePositionToFile(float x, float y, float z)
    {
        Debug.Log(fn);

        StartWriting(fn);
        SaveToFile(20, x, y, z);
        EndWriting();
    }

    void playerSaveLoad()
    {
        //Save
        if (Input.GetKeyDown(KeyCode.N))
        {
            //Change to player position
            savePositionToFile(wow.transform.position.x, wow.transform.position.y, wow.transform.position.z);
        }

        //Load

        if (Input.GetKeyDown(KeyCode.M))
        {


            List<string> fileLines = File.ReadAllLines(fn).ToList();

            foreach (string line in fileLines)
            {
                Debug.Log(line);
            }

            //Change to player position
            wow.transform.position = new Vector3(float.Parse(fileLines[1]), float.Parse(fileLines[2]), float.Parse(fileLines[3]));

            ;
        }
    }

    // Update is called once per frame
    void Update()
    {

        playerSaveLoad();

    }
}
