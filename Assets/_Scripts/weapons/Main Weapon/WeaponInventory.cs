using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public Camera cam;
    public Transform playerTransform;
    public Transform gunPosition;
    public int maxGuns;

    [SerializeField]
    private List<GameObject> guns;
    public int activeGunIndex = 0;

    public PlayerMovement player;

    public Animator armsAnim;

    private void Start()
    {
        //guns = new List<GameObject>();
    }

    private void Update()
    {
        GetInputs();    
    }   
    
    private void GetInputs() {
        if (Input.GetKeyDown(KeyCode.G)) DropGun();
        if (Input.GetKeyDown(KeyCode.F)) PickUpGun();
        //change active gun
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetActiveGun(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetActiveGun(1);
    }

    private void DropGun()
    {
        if (guns.Count < 2) return; // have atleast one gun

        // throw gun
        guns[activeGunIndex].GetComponent<Animator>().enabled = false;
        //guns[activeGunIndex].GetComponent<ReloadOnEmpty>().enabled = false;
        //guns[activeGunIndex].GetComponent<SoundOnFire>().enabled = false;
        guns[activeGunIndex].GetComponent<MeshCollider>().isTrigger = false;
        guns[activeGunIndex].GetComponent<Transform>().parent = null;
        guns[activeGunIndex].GetComponent<Rigidbody>().useGravity = true;
        guns[activeGunIndex].GetComponent<Rigidbody>().mass = 1f;
        guns[activeGunIndex].GetComponent<Rigidbody>().AddForce(playerTransform.forward * 10f + Vector3.up * 10f, ForceMode.Impulse);
        guns[activeGunIndex].GetComponent<Weapon>().playerReference = null;
        guns[activeGunIndex].GetComponent<Weapon>().isActive = false;
        guns.RemoveAt(activeGunIndex);
        SetActiveGun(guns.Count-1);
    }

    private void PickUpGun()
    {
        //raycast to where camera is looking then pick up gun
        int gunLayer = 8, layerMask = 1;
        layerMask = layerMask << gunLayer;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if(Physics.SphereCast(ray, 3f, out hit, 5f, layerMask))
        {
            //detected a gun
            guns.Add(hit.transform.gameObject);
            SetActiveGun(guns.Count - 1);

            hit.transform.GetComponent<Rigidbody>().isKinematic = false;
            hit.transform.position = gunPosition.position;
            hit.transform.SetParent(gunPosition);
            hit.rigidbody.velocity = Vector3.zero;
            hit.rigidbody.useGravity = false;
            hit.transform.localRotation = Quaternion.identity;

            guns[activeGunIndex].GetComponent<MeshCollider>().isTrigger = true;
            guns[activeGunIndex].GetComponent<Weapon>().playerReference = player;
            guns[activeGunIndex].GetComponent<Weapon>().isActive = true;
            guns[activeGunIndex].GetComponent<Animator>().enabled = true;
            //guns[activeGunIndex].GetComponent<ReloadOnEmpty>().enabled = true;
            //guns[activeGunIndex].GetComponent<SoundOnFire>().enabled = true;

        }

        if (guns.Count > maxGuns)
        {
            activeGunIndex--;
            DropGun();
        }
    }

    private void SetActiveGun(int index)
    {
        activeGunIndex = index;

        for(int i = 0; i < guns.Count; i++)
        {
            guns[i].GetComponent<Weapon>().CancelReload();
            if (i == activeGunIndex)
            {
                guns[i].GetComponent<Animator>().speed = 1;
                guns[i].SetActive(true);
                //guns[i].transform.localPosition = Vector3.zero;
                //guns[i].transform.localRotation = Quaternion.identity;
                guns[i].GetComponent<Weapon>().enabled = true;
                string name = guns[i].name + "Equip";
                Invoke("ResetArms", 0.25f);
                armsAnim.SetBool(name, true);
                continue;
            }
            guns[i].GetComponent<Weapon>().enabled = false;
            guns[i].SetActive(false);
        }

    }

    public void ResetAmmo()
    {
        for(int i = 0; i < guns.Count; i++)
            guns[i].GetComponent <Weapon>().PickUpAmmo(210);
    }

    private void ResetArms()
    {
        armsAnim.SetBool("KnifeEquip", false);
        armsAnim.SetBool("KnifeShoot", false);
        armsAnim.SetBool("PistolEquip", false);
        armsAnim.SetBool("AKEquip", false);
    }

}
