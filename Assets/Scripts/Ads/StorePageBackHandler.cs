using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePageBackHandler : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.uiStateObject.ShowTopBar();
        GameManager.Instance.uiStateObject.Ping("VIPerks");
        GameManager.Instance.inputReader.Back+=Back;
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back-=Back;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu", "Store");
    }

}