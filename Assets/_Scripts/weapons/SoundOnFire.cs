using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class SoundOnFire : MonoBehaviour
{

    public AK.Wwise.Event gunShot;

    Weapon _weapon = null;

    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    private void OnEnable()
    {
        _weapon.Fire += OnFire;
        _weapon.Fire += OnEmpty;
    }

    private void OnDisable()
    {
        _weapon.Fire -= OnFire;
        _weapon.Fire -= OnEmpty;
    }

    void OnFire()
    {
        if (gunShot != null)
        {
            gunShot.Post(gameObject);
        }
    }

    void OnEmpty()
    {
        if (gunShot != null)
        {
            gunShot.Post(gameObject);
        }
    }


}
