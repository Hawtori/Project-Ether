using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun 
{
    private float damage;
    private float fireRate;
    private float moveSpeed;
    
    public Gun(float damage, float fireRate, float moveSpeed)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        this.moveSpeed = moveSpeed;
    }

    public float GetDamage()
    {
        return this.damage;
    }

    public float GetFireRate()
    {
        return this.fireRate;
    }

    public float GetMoveSpeed()
    {
        return this.moveSpeed;
    }

}
