using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMButtons : MonoBehaviour
{

    public void NewGame()
    {
        GameManager.Instance.LoadSceneAdditive("RunSettings",false,"MainMenu");
    }
    
    public void Hotel()
    {
        GameManager.Instance.LoadSceneAdditive("Hotel",false,"MainMenu");
    }
    
    public void Settings()
    {
        GameManager.Instance.LoadSceneAdditive("Settings",false,"MainMenu");
    }
    
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
        Application.Quit();
    }
}
