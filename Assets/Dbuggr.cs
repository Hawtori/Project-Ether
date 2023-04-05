using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Dbuggr : MonoBehaviour
{
    public static Dbuggr instance;

    public TMP_Text text;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void AddText(string t) => text.text = t;
}
