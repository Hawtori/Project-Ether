using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class SpawnPieces : MonoBehaviour
{
    public static SpawnPieces _instance { get; set; }

    public GameObject[] pieces;
    public Vector3[] offset;
    public Transform level;

    private int pieceIndex = 0;
    private GameObject currentPiece;

    public bool confirm = false;
    private bool started = true;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(gameObject);
    }

    public void ChangeIndex(TMP_Dropdown value)
    {
        pieceIndex = value.value;
    }

    public void ConfirmPiece()
    {
        if (started)
        {
            ClearFile();
            started = false;
        }
        InstantiatePiece();
    }

    public void LoadLeve()
    {
        started = false;
        string path = Application.persistentDataPath + "/Level.txt";
        StreamReader read = new StreamReader(path);

        string line, tempPos, tempRot;

        int index; Vector3 pos; Quaternion rot;

        while (!read.EndOfStream)
        {
            line = read.ReadLine();
            tempPos = "";
            tempRot = "";

            index = int.Parse(line[0].ToString());
            int i = 3;
            while(line[i] != ')')
            {
                tempPos += line[i]; i++;
            }

            {
                var pt = tempPos.Split(","[0]);
                pos.x = float.Parse(pt[0]);
                pos.y = float.Parse(pt[1]);
                pos.z = float.Parse(pt[2]);
            }

            i += 3;
            while(line[i] != ')')
            {
                tempRot += line[i]; i++;
            }

            {
                var pt = tempRot.Split(","[0]);
                rot.x = float.Parse(pt[0]);
                rot.y = float.Parse(pt[1]);
                rot.z = float.Parse(pt[2]);
                rot.w = float.Parse(pt[3]);
            }

            Instantiate(pieces[index], pos, rot, level);


        } //end of read
    }

    public void Save()
    {
        Debug.Log("Saved!");
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || confirm)  PlacePiece();

        if (currentPiece == null)
            return;


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = 0;

        if (Input.GetKeyDown(KeyCode.R)) currentPiece.transform.RotateAround(mousePos - offset[pieceIndex], Vector3.up, 90f);

        currentPiece.transform.position = mousePos - offset[pieceIndex];

    }

    private void PlacePiece()
    {
        SaveLevel(pieceIndex, currentPiece.transform);
        confirm = false; currentPiece.GetComponent<CurrentPiece>().enabled = false; currentPiece = null;
    }

    private void InstantiatePiece()
    {
        Vector3 pos = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.y);
        currentPiece = Instantiate(pieces[pieceIndex], pos, Quaternion.identity, level);
    }

    private void SaveLevel(int index, Transform piece)
    {
        string path = Application.persistentDataPath + "/Level.txt";

        StreamWriter write = new StreamWriter(path, true);

        write.WriteLine(index + " " + piece.position + " " + piece.rotation);

        write.Close();
    }

    private void ClearFile()
    {
        string path = Application.persistentDataPath + "/Level.txt";

        StreamWriter write = new StreamWriter(path, false);

        write.Flush();

        write.Close();
    }

}
