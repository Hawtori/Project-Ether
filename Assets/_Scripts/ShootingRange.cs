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
    private List<GameObject> targets;

    [SerializeField]
    private List<Vector3> targetPositions;
    private GameObject cube;
    private bool spawnTargets = false;

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
        totalTargets = targets.Count;
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        targetPositions = new List<Vector3>();
    }

    private void CreateList()
    {
        targetPositions.Clear();
        for (int i = 0; i < 15; i++)
        {
            Vector3 pos = new Vector3(Random.Range(0f, 20f), Random.Range(0f, 20f), 0);
            targetPositions.Add(pos);
        }
        totalTargets = targetPositions.Count;
    }

    private void Start()
    {
        //flyweight to create targets
        if(totalTargets == 0)
        {
            spawnTargets = true;

            cube.AddComponent<BoxCollider>().size = Vector3.one * 1.5f;
            cube.layer = 7;
            cube.transform.parent = transform;
            cube.transform.localScale = new Vector3(3.7716f, 3.7716f, 1);

            CreateList();
        }
    }

    private void Update()
    {
        if (spawnTargets)
        {
            if (targetsHit == totalTargets) { targetsHit = 0; CreateList(); }
            cube.transform.localPosition = targetPositions[targetsHit];
            cube.GetComponent<Renderer>().material.color = Color.white;

        }
        else
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
        } 

        if(targetsHit > 0)
            startTimer = true;
        else
        {
            if (timer < highscore) highscore = timer;
            if (highscore == float.MaxValue)
            highText.text = "Highscore: 0";
            else
            highText.text = "Highscore: " + highscore.ToString();
            startTimer = false;
            timer = 0f;
            targetsHit = 0;
        }

        if (startTimer)
        {
            timer += Time.deltaTime;
            timerText.text = "Timer: " + timer.ToString();
        }
    }

    public void IncreaseHit()
    {
        if (!spawnTargets)
        {
            targetsHit = 0;
            foreach (GameObject target in targets)
            {
                if (target.GetComponent<Renderer>().material.color == Color.yellow) targetsHit++;
            }
        }
        else 
            targetsHit++; startTimer = true; 
        
    }
}
