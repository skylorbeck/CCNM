using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotelManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);

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
