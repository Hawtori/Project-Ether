using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    public GameObject[] guns;
    public Transform leftPosition, rightPosition;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropGun(0);
        }


        if(guns.Length > 2)
        {
            DropGun(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(leftPosition.transform.childCount > 1)
            {
                leftPosition.GetChild(0).gameObject.SetActive(true);
                leftPosition.GetChild(1).gameObject.SetActive(false);
            }
            else if (rightPosition.transform.childCount > 1)
            {
                rightPosition.GetChild(0).gameObject.SetActive(true);
                rightPosition.GetChild(1).gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (leftPosition.transform.childCount > 1)
            {
                leftPosition.GetChild(1).gameObject.SetActive(true);
                leftPosition.GetChild(0).gameObject.SetActive(false);
            }
            else if (rightPosition.transform.childCount > 1)
            {
                rightPosition.GetChild(1).gameObject.SetActive(true);
                rightPosition.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void DropGun(int index)
    {
        Debug.Log("Dropped gun");
        if(leftPosition.transform.childCount > 0)
        {
            Transform child = leftPosition.transform.GetChild(index);
            child.parent = null;
            child.GetComponent<Rigidbody>().useGravity = true;
        }
        if (rightPosition.transform.childCount > 0)
        {
            Transform child = rightPosition.transform.GetChild(index);
            child.parent = null;
            child.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
