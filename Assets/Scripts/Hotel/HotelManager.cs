using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotelManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.inputReader.Back+=Back;
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu",false,"Hotel");
    }
    public void Equipment()
    {
        GameManager.Instance.LoadSceneAdditive("Equipment",false,"Hotel");
    }
}
