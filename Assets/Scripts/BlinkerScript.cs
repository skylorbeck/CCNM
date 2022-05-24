using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkerScript : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.FixedHalfSecond+=EnableDisable;    
    }

    void OnDestroy()
    {
        GameManager.Instance.FixedHalfSecond-=EnableDisable;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    void EnableDisable()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
