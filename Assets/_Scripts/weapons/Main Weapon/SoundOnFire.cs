using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class SoundOnFire : MonoBehaviour
{

    public AK.Wwise.Event GunShot;

    Weapon _weapon = null;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        _weapon.Fire += OnFire;
    }

    private void OnDisable()
    {
        _weapon.Fire -= OnFire;

    }

    void OnFire()
    {
        if (GunShot != null)
        {
            GunShot.Post(gameObject);
        }
    }


}
