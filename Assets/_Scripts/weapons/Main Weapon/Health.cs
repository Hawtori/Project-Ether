using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float health;

    private bool takingDamage = false;

    private Color initColor;

    private void Start()
    {
        initColor = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        if (health <= 0) Die();
    }

    public void TakeDamage(float d)
    {
        if (takingDamage) return;
        takingDamage = true;
        health -= d;
        GetComponent<Renderer>().material.color = Color.red;
        Invoke("ResetDamage", 0.15f);
    }

    private void ResetDamage()
    {
        takingDamage = false;
        GetComponent<Renderer>().material.color = initColor;
    }

    private void Die()
    {
        EnemyPool._instance.ReturnEnemy(gameObject);
    }
}
