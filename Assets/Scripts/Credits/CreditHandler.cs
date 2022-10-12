using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditHandler : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.uiStateObject.ShowTopBar();
        GameManager.Instance.uiStateObject.Ping("Credits and Special Thanks");
        GameManager.Instance.inputReader.Back+=Back;
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back-=Back;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu", "Credits");
    }
}