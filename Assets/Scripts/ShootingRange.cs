using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// singleton design pattern
/// </summary>
public class ShootingRange : MonoBehaviour
{
    public static ShootingRange _instance { get; set; }

    [SerializeField]
    private TMP_Text timerText;
    [SerializeField]
    private TMP_Text highText;
    [SerializeField]
    private GameObject[] targets;

    private int targetsHit = 0;
    private int totalTargets;

    private bool startTimer = false;

    private float timer = float.MaxValue;
    private float highscore = float.MaxValue;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        totalTargets = targets.Length;
    }

    private void Update()
    {
        if(targetsHit == totalTargets)
        {
            foreach (GameObject target in targets)
            {
                target.GetComponent<Renderer>().material.color = Color.white;
                targetsHit = 0;
                if (timer < highscore) highscore = timer;
            }
        }

        if(targetsHit > 0)
        {
            startTimer = true;
        }
        else
        {
            if(highscore == float.MaxValue)
            highText.text = "Highscore: 0";
            else
            highText.text = "Highscore: " + highscore.ToString();
            startTimer = false;
            timer = 0f;
        }

        if (startTimer)
        {
            timer += Time.deltaTime;
            timerText.text = "Timer: " + timer.ToString();
        }
    }

    public void IncreaseHit()
    {
        targetsHit = 0;
        foreach (GameObject target in targets)
        {
            if (target.GetComponent<Renderer>().material.color == Color.yellow) targetsHit++;
        }
    }
}
