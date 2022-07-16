using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMButtons : MonoBehaviour
{
    public void NewGame()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("RunSettings","MainMenu");
    }

    public void ResumeGame()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("MapScreen", "MainMenu");
    }

    public void Hotel()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Hotel","MainMenu");
    }
    
    public void Settings()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Settings","MainMenu");
    }
    
    private void Start()
    {
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.uiStateObject.HideTopBar();
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
        SoundManager.Instance.PlayUiBack();
        Application.Quit();
    }

    public void EnableButtons()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
    }
}
