using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDrop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // loop through weapons 
            other.GetComponentInParent<WeaponInventory>().ResetAmmo();
            Destroy(gameObject);
        }
    }
}
