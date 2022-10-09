using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoublerToggle : MonoBehaviour
{
    private Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        OnPurchaseComplete();
        IAPManager.Instance.OnPurchaseComplete+= OnPurchaseComplete;
    }

    private void OnPurchaseComplete()
    {
        if (GameManager.Instance.metaPlayer.doublerOwned)
        {
            toggle.interactable = true;
            toggle.isOn = GameManager.Instance.metaPlayer.doublerOwned;
        } else {
            toggle.isOn = false;
            toggle.interactable = false;
        }
    }

    public void Toggle()
    {
        GameManager.Instance.metaPlayer.doublerActive = toggle.isOn;
    }

    public void OnDestroy()
    {
        IAPManager.Instance.OnPurchaseComplete-= OnPurchaseComplete;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
