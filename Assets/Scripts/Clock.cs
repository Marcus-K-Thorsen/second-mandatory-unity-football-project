using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{

    public TMP_Text clocktext;

    // Update is called once per frame
    void Update()
    {
        clocktext.text = DateTime.Now.ToShortTimeString();
    }
}
