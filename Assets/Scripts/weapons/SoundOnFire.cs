using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class SoundOnFire : MonoBehaviour
{
    [SerializeField] AudioClip _gunShotSound = null;
    [SerializeField] AudioClip _emptyMagSound = null;
    [SerializeField] Transform _locationToPlay = null;

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
        if (_gunShotSound != null && _locationToPlay != null)
        {
            AudioSource.PlayClipAtPoint
                (_gunShotSound, _locationToPlay.position);
        }
    }

    void OnEmpty()
    {
        if (_gunShotSound != null && _locationToPlay != null)
        {
            AudioSource.PlayClipAtPoint
              (_emptyMagSound, _locationToPlay.position);
        }
    }


}
