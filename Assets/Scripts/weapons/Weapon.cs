using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    //Bullet 
    public GameObject bullet;
    public float shootForce, upwardForce;

    //Gun
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magSize;
    
    private int bulletsLeft;

    private bool shooting, readyToShoot, reloading;

    public Camera cam;
    public Transform nuzzle;

    public GameObject nuzzleFlash;
    public TextMeshProUGUI ammoDisplay;

    private bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = true;
    }

    private void Update()
    {
        Inputs();

        if (ammoDisplay != null)
            ammoDisplay.SetText(bulletsLeft + " / " + magSize);
    }

    private void Inputs()
    {
        shooting = Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading) Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 target;
        if (Physics.Raycast(ray, out hit)) target = hit.point;
        else target = ray.GetPoint(80); //somewhere far away

        Vector3 dir = target - nuzzle.position;

        //spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 targetDir = dir + new Vector3(x, y, 0);

        GameObject currBullet = Instantiate(bullet, nuzzle.position, Quaternion.identity);
        currBullet.transform.forward = targetDir.normalized;

        //add forces
        currBullet.GetComponent<Rigidbody>().AddForce(targetDir.normalized * shootForce, ForceMode.Impulse);

        if (nuzzleFlash != null) Instantiate(nuzzleFlash, nuzzle.position, Quaternion.identity);

        bulletsLeft--;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        GetComponent<Renderer>().material.color = Color.black;
        reloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    private void ReloadFinish()
    {
        GetComponent<Renderer>().material.color = Color.white;
        bulletsLeft = magSize;
        reloading = false;
    }

    
}

