using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class ReloadOnEmpty : MonoBehaviour
{

    public AK.Wwise.Event emptyGun;

    Weapon _weapon = null;

  
    void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }
    private void OnEnable()
    {
        _weapon.Rel += OnReload;
    }

    private void OnDisable()
    {
        _weapon.Rel -= OnReload;

    }

    void OnReload()
    {
        if (emptyGun != null)
        {
            emptyGun.Post(gameObject);
        }
    }
}
