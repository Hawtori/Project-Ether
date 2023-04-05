using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempArmsAnim : MonoBehaviour
{
    public Animator anim;

    private int equip;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            equip = 0;
            anim.SetBool("KnifeEquip", true);
            Invoke("ResetAnim", 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            equip = 1;
            anim.SetBool("PistolEquip", true);
            Invoke("ResetAnim", 0.5f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(equip == 0)
            anim.SetBool("KnifeShoot", true);
            
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(equip == 0)
            anim.SetBool("KnifeShoot", false);
        }

    }

    private void ResetAnim()
    {
        anim.SetBool("KnifeEquip", false);
        anim.SetBool("KnifeShoot", false);
        anim.SetBool("PistolEquip", false);
    }
}
