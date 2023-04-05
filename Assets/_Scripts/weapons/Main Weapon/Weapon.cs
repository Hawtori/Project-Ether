using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Weapon : MonoBehaviour
{
    public bool isActive;
    public bool isKinfe;

    //Bullet 
    public GameObject bullet;
    public float shootForce, upwardForce;

    //Gun
    public float recoilForce;
    public float equipTime;
    public float timeBetweenShooting, spread, reloadTime;
    public int totalBullets;
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
    public Animator armsAnim;

    public PlayerMovement playerReference;

    private int maxBullets;

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
        maxBullets = totalBullets;
    }

    private void OnEnable()
    {
        //anim.speed = 1;
        Invoke("CanShoot", equipTime);
    }

    private void Update()
    {
        if (!isActive) { Falling(); return; }
        if(Time.timeScale != 0) Inputs();

        if (ammoDisplay != null)
            ammoDisplay.SetText(bulletsLeft + " / " + Mathf.Max(totalBullets, 0));

        ChangeSpread();
    }

    private void Inputs()
    {
        if (isAuto)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magSize && totalBullets > 0 && !reloading)
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
            //float xRot = playerReference.xRotation;
            playerReference.xRotationRecoil = spreadIncreaseY;
        }
    #endregion

        readyToShoot = false;

        if (isKinfe)
        {
            Invoke("Bullet", 0.334f);
            armsAnim.SetBool("KnifeShoot", true);
            Invoke("ResetArms", 0.25f);
        }
        else
            Bullet();
        
        if (anim != null)
        {
            anim.SetTrigger("Shot");
            //anim.speed = 1f;
            Invoke("ResetTrigger", 0.166f);
        }

        if (nuzzleFlash != null) Instantiate(nuzzleFlash, nuzzle.position, Quaternion.identity);

        if(!isKinfe)
        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

    }

    private void Bullet()
    {
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
        if (upwardForce > 0) currBullet.GetComponent<Rigidbody>().useGravity = true;
    }
        
    private void ResetTrigger()
    {
        //if(!isKinfe)
        //anim.speed = 0f;
        anim.ResetTrigger("Shot");
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
            //float xRot = playerReference.xRotation;
            playerReference.xRotationRecoil = spreadIncreaseY;
            //playerReference.xRotation = LerpF(xRot-spreadIncreaseY, xRot, resetSpreadTime);
            return;
        }

        if (shooting && !reloading)
        {
            spreadIncreaseY += spreadIncreaseDelta * recoilForce * Time.deltaTime * 2f;
            resetSpreadTime += Time.deltaTime;
            spreadIncreaseDelta += Time.deltaTime / 3f;
            spreadIncreaseDelta = Mathf.Min(0.1f, spreadIncreaseDelta);
        }
        else
        {
            spreadIncreaseY -= spreadIncreaseDelta / 3f * recoilForce * recoilForce * Time.deltaTime;
            resetSpreadTime -= Time.deltaTime;
            bulletsShot -= Time.deltaTime;
            spreadIncreaseDelta -= Time.deltaTime;
            spreadIncreaseDelta = Mathf.Max(0.075f, spreadIncreaseDelta);
        }

        spreadIncreaseY = Mathf.Clamp(spreadIncreaseY, 0, 18f);
        resetSpreadTime = Mathf.Clamp(resetSpreadTime, 0, 1f);
        float xRot = playerReference.xRotation;
        playerReference.xRotationRecoil = spreadIncreaseY;
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        if (isKinfe) return;
        //GetComponent<Renderer>().material.color = Color.black;
        if(anim != null)
        {
            anim.ResetTrigger("Shot");
            anim.SetTrigger("Reload");
            string name = gameObject.name + "Reload";
            armsAnim.SetBool(name, true);
            Invoke("EndArmsReload", reloadTime/2f);
        }
        reloading = true;
        Invoke("ReloadFinish", reloadTime);

    }

    public void CancelReload()
    {
        //GetComponent<Renderer>().material.color = Color.white;
        if (anim != null)
        {
            anim.ResetTrigger("Reload");
            string name = gameObject.name + "Reload";
            armsAnim.SetBool(name, false);
        }
        reloading = false;
        CancelInvoke("ReloadFinish");
    }

    public void PickUpAmmo(int ammo)
    {
        totalBullets += ammo;
        totalBullets = Mathf.Clamp(totalBullets, 0, maxBullets);
    }

    private void EndArmsReload()
    {
         string name = gameObject.name + "Reload";
            armsAnim.SetBool(name, false);
    }

    private void ReloadFinish()
    {
        //GetComponent<Renderer>().material.color = Color.white;
        if(anim != null)
        {
            anim.ResetTrigger("Reload");
            string name = gameObject.name + "Reload";
            armsAnim.SetBool(name, false);
            //if(!isKinfe)
            //anim.speed = 0;
        }
        totalBullets = Mathf.Clamp(totalBullets - magSize, 0 - magSize, 175);
        bulletsLeft = Mathf.Min(magSize, totalBullets + magSize);
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

    private void ResetArms()
    {
        armsAnim.SetBool("KnifeEquip", false);
        armsAnim.SetBool("KnifeShoot", false);
        armsAnim.SetBool("PistolEquip", false);
        armsAnim.SetBool("AKEquip", false);
        armsAnim.SetBool("AKReload", false);
    }

}

