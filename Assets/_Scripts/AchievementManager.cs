using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public TextMeshProUGUI jump1, shoot1;

    private int jumpTotal = 0;
    private int shootTotal = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpTotal++;
        }
        if (jumpTotal >= 5)
        {
            jump1.text = "Complete!";
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shootTotal++;
        }
        if (shootTotal >= 10)
        {
            shoot1.text = "Complete!";
        }
    }
}
