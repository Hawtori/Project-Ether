using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Weapon : MonoBehaviour
{
    //Bullet 
    public GameObject bullet;
    public float shootForce, upwardForce;

    //Gun
    public float timeBetweenShooting, spread, reloadTime;
    public int magSize;

    private float spreadIncreaseY = 0, resetSpreadTime = 0;
    private float bulletsShot = 0;

    //Gun Firing Observer Event
    public event Action Fire = delegate { };

    //Gun Reloading Observer Event
    public event Action Rel = delegate { };

    private int bulletsLeft;

    private bool shooting, readyToShoot, reloading;

    public Camera cam;
    public Transform nuzzle;

    public GameObject nuzzleFlash;
    public TextMeshProUGUI ammoDisplay;

    private bool allowInvoke = true;

    private float LerpF(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

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

        ChangeSpread();
    }

    private void Inputs()
    {
        shooting = Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && !reloading)
        {
            Rel.Invoke();
            Reload();
            return;
        }

        if (/*readyToShoot && shooting &&*/ !reloading && bulletsLeft <= 0)
        {
            Rel.Invoke();
            Reload();
            return;
        }

        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Fire.Invoke();
            Shoot();
            return;
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        //RaycastHit hit;

        Vector3 target = ray.GetPoint(80);

        //if (Physics.Raycast(ray, out hit)) target = hit.point;
        //else target = ray.GetPoint(80); //somewhere far away

        Vector3 dir = target - nuzzle.position;

        //spread
        float x = UnityEngine.Random.Range(-spread * Mathf.Clamp(bulletsShot, 0, 2), spread * Mathf.Clamp(bulletsShot, 0, 2));
        float y = UnityEngine.Random.Range(-spread * Mathf.Clamp(bulletsShot, 0, 2), spread * Mathf.Clamp(bulletsShot, 0, 2));

        Vector3 targetDir = dir + new Vector3(x, y, 0);

        GameObject currBullet = Instantiate(bullet, nuzzle.position, Quaternion.identity);
        currBullet.transform.forward = targetDir.normalized;

        //add forces
        currBullet.GetComponent<Rigidbody>().AddForce(targetDir.normalized * shootForce, ForceMode.Impulse);

        if (nuzzleFlash != null) Instantiate(nuzzleFlash, nuzzle.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

    }

    private void ChangeSpread()
    {
        if (shooting && !reloading) { spreadIncreaseY += 0.05f; resetSpreadTime += Time.deltaTime; }
        else { spreadIncreaseY -= 0.025f; resetSpreadTime -= Time.deltaTime; bulletsShot-= Time.deltaTime; }


        spreadIncreaseY = Mathf.Clamp(spreadIncreaseY, 0, 18f);
        resetSpreadTime = Mathf.Clamp(resetSpreadTime, 0, 1f);
        float xRot = PlayerMovement._instance.xRotation;
        PlayerMovement._instance.xRotationRecoil = spreadIncreaseY;
        //PlayerMovement._instance.xRotation = LerpF(xRot-spreadIncreaseY, xRot, resetSpreadTime);
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

