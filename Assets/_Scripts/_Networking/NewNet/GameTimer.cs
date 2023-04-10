using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;
using TMPro;

public class GameTimer : MonoBehaviour
{
    // we only want one timer
    public static GameTimer Instance;

    public bool endGame = false;
    public GameObject endScreen;
    public TMP_Text leaderboardTxt;

    private bool invoked = false;

    private float timer = 0f;
    private List<string> leaderboardDates = new List<string>();
    private List<float> leaderboardTimes = new List<float>();


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        Debug.Log(Application.persistentDataPath);
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.H)) endGame = true;

        if (!endGame)
        {
            timer += Time.deltaTime;
        }
        // else => get timer, save to text file, read everything from text file and show it on the end game window
        else
        {
            if (invoked) return;
            else EndGame();
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0f;
        invoked = true;
        endScreen.SetActive(true);
        // read from text file
        string path = Application.persistentDataPath + @"\leaderboard.txt";

        if (!File.Exists(path)) File.Create(path).Close();

        //using (StreamWriter sw = new StreamWriter(path))
        //{
        //    string line = DateTime.Today.ToString("d") + ":" + timer;
        //    Debug.Log("LINE TO ADD: " + line);
        //    sw.WriteLine(line + "\n");
        //    sw.Close();
        //}

        Debug.Log("Entries: " + leaderboardTimes.Count);

        leaderboardDates.Add(DateTime.Today.ToString("d"));
        leaderboardTimes.Add(timer);
        
        using (StreamReader sr = new StreamReader(path))
        {
            string line;
            while((line = sr.ReadLine()) != null) 
            {
                if (line == "") continue;
                Debug.Log("LINE READ: " + line);
                leaderboardDates.Add(line.Split(':')[0]);
                leaderboardTimes.Add(float.Parse(line.Split(':')[1]));
            }
            sr.Close();
        }       

        // sort the times {bubble sort}
        for(int i = 0; i < leaderboardTimes.Count; i++)
        {
            for(int j = 0; j < leaderboardTimes.Count - i - 1; j++)
            {
                if (leaderboardTimes[j] > leaderboardTimes[j + 1])
                {
                    float temp = leaderboardTimes[j];
                    leaderboardTimes[j] = leaderboardTimes[j + 1];
                    leaderboardTimes[j + 1] = temp;
        
                    string t = leaderboardDates[j];
                    leaderboardDates[j] = leaderboardDates[j+1];
                    leaderboardDates[j+ 1] = t;
                }
            }
        }

        // display times on window, only top 10
        using (StreamWriter sw = new StreamWriter(path))
        {
            for (int i = 0; i < leaderboardTimes.Count && i < 10; i++)
            {
                TimeSpan time = TimeSpan.FromSeconds(leaderboardTimes[i]);
                leaderboardTxt.text += "\n" + leaderboardDates[i] + " : " + string.Format("{0:D2}m-{1:D2}s", time.Minutes, time.Seconds);

                // save everyting to text file
                //File.WriteAllText(path, "");

                sw.WriteLine(leaderboardDates[i] + " : " + leaderboardTimes[i]);

            }
            sw.Close();
        }
        // save format:
        // date : time to completion
    }

}
