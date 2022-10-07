using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera camera;
    public Gun gun;
    public Transform nuzzle;
    public float damage;
    public float fireRate;
    public float moveSpeed;

    private float shootTimer = 0f;
    private RaycastHit hit;
    private int shootableLayer = 7;
    private int bitMask;

    [SerializeField]
    private Vector3 rot;

    private void Start()
    {
        gun = new Gun(damage, fireRate, moveSpeed);
        bitMask = 1 << shootableLayer;
    }

    private void Update()
    {
        if (transform.parent == null) return;

        transform.localRotation = Quaternion.Euler(rot);

        if (shootTimer < gun.GetFireRate()) shootTimer += Time.deltaTime;
        GetInputs();
        if (shootTimer >= gun.GetFireRate()) GetComponent<Renderer>().material.color = Color.black;
        else GetComponent<Renderer>().material.color = Color.white;

        GameObject.Find("Player").GetComponent<PlayerMovement>().speedMultiplier = gun.GetMoveSpeed();
    }

    private void GetInputs()
    {
        if (Input.GetMouseButton(0)) Shoot();
        
    }

    private void Shoot()
    {        
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        Vector3 dir = ray.direction;// - nuzzle.transform.position;

        if (shootTimer >= gun.GetFireRate())
        {//local transform y going positive
            if(Physics.Raycast(nuzzle.transform.position, dir, out hit, Mathf.Infinity, bitMask))
            {
                hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                ShootingRange._instance.IncreaseHit();
            }
            shootTimer = 0f;
        } 
    }
}

