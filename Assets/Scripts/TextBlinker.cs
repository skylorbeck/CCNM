using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TextBlinker : MonoBehaviour
{
    TextMeshProUGUI text;

    protected void OnEnable()
    {
        text = GetComponent<TextMeshProUGUI>();

        GameManager.Instance.FixedSecond+=EnableDisable;    
    }

    protected void OnDisable()
    {
        GameManager.Instance.FixedSecond-=EnableDisable;
    }

    protected void EnableDisable()
    {
        text.enabled = !text.enabled;

    }
}
