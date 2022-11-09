using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Weapon : MonoBehaviour
{
    public bool isActive;

    //Bullet 
    public GameObject bullet;
    public float shootForce, upwardForce;

    //Gun
    public float recoilForce;
    public float equipTime;
    public float timeBetweenShooting, spread, reloadTime;
    public int magSize;
    public bool isAuto;

    private float spreadIncreaseDelta = 0.075f;
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

    //animations
    private Animator anim;

    private float LerpF(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    private void Awake()
    {
        bulletsLeft = magSize;
        readyToShoot = false;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke("CanShoot", equipTime);
    }

    private void OnEnable()
    {
        Invoke("CanShoot", equipTime);
    }

    private void Update()
    {
        if (!isActive) { Falling(); return; }
        Inputs();

        if (ammoDisplay != null)
            ammoDisplay.SetText(bulletsLeft + " / " + magSize);

        ChangeSpread();
    }

    private void Inputs()
    {
        if (isAuto)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

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
        #region changing spread
        if (!isAuto) { 
            spreadIncreaseY += recoilForce;
            resetSpreadTime += 0.1f;
            spreadIncreaseDelta += 1f;
            spreadIncreaseDelta = Mathf.Min(0.1f, spreadIncreaseDelta);

            spreadIncreaseY = Mathf.Clamp(spreadIncreaseY, 0, 18f);
            resetSpreadTime = Mathf.Clamp(resetSpreadTime, 0, 1f);
            //float xRot = PlayerMovement._instance.xRotation;
            PlayerMovement._instance.xRotationRecoil = spreadIncreaseY;
        }
    #endregion

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
        currBullet.GetComponent<Rigidbody>().AddForce(targetDir.normalized * shootForce + transform.up * upwardForce, ForceMode.Impulse);
        if(upwardForce > 0) currBullet.GetComponent<Rigidbody>().useGravity = true;


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
        if (!isAuto)
        {
            resetSpreadTime -= Time.deltaTime; 
            bulletsShot-= Time.deltaTime; 
            spreadIncreaseDelta -= Time.deltaTime; 
            spreadIncreaseDelta = Mathf.Max(0.075f, spreadIncreaseDelta);

            //spreadIncreaseY = -LerpF(spreadIncreaseY, 0, resetSpreadTime) ;
            spreadIncreaseY -= 0.1f;

            spreadIncreaseY = Mathf.Clamp(spreadIncreaseY, 0, 18f);
            resetSpreadTime = Mathf.Clamp(resetSpreadTime, 0, 1f);
            //float xRot = PlayerMovement._instance.xRotation;
            PlayerMovement._instance.xRotationRecoil = spreadIncreaseY;
            //PlayerMovement._instance.xRotation = LerpF(xRot-spreadIncreaseY, xRot, resetSpreadTime);
            return;
        }

        if (shooting && !reloading)
        {
            spreadIncreaseY += spreadIncreaseDelta;
            resetSpreadTime += Time.deltaTime;
            spreadIncreaseDelta += Time.deltaTime / 3f;
            spreadIncreaseDelta = Mathf.Min(0.1f, spreadIncreaseDelta);
        }
        else
        {
            spreadIncreaseY -= spreadIncreaseDelta / 1.5f;
            resetSpreadTime -= Time.deltaTime;
            bulletsShot -= Time.deltaTime;
            spreadIncreaseDelta -= Time.deltaTime;
            spreadIncreaseDelta = Mathf.Max(0.075f, spreadIncreaseDelta);
        }

        spreadIncreaseY = Mathf.Clamp(spreadIncreaseY, 0, 18f);
        resetSpreadTime = Mathf.Clamp(resetSpreadTime, 0, 1f);
        float xRot = PlayerMovement._instance.xRotation;
        PlayerMovement._instance.xRotationRecoil = spreadIncreaseY;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        GetComponent<Renderer>().material.color = Color.black;
        if(anim != null)
        anim.SetBool("Reload", true);
        reloading = true;
        Invoke("ReloadFinish", reloadTime);
    }

    public void CancelReload()
    {
        GetComponent<Renderer>().material.color = Color.white;
        if (anim != null) anim.SetBool("Reload", false);
        reloading = false;
        CancelInvoke("ReloadFinish");
    }

    private void ReloadFinish()
    {
        GetComponent<Renderer>().material.color = Color.white;
        if(anim != null)
        anim.SetBool("Reload", false);
        bulletsLeft = magSize;
        reloading = false;
    }

    private void CanShoot()
    {
        readyToShoot = true;
    }
    
    private void Falling()
    {
        if (GetComponent<Rigidbody>().isKinematic) return;

        int ground = 6, layerMask = 1;
        layerMask = layerMask << ground;
        
        if(Physics.Raycast(transform.position, Vector3.down, 1f, layerMask))
            GetComponent<Rigidbody>().isKinematic = true;
        
    }

}

