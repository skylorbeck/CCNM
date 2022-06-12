using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMButtons : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.Instance.LoadSceneAdditive("RunSettings","MainMenu");
    }

    public void ResumeGame()
    {
        List<string> scenes = new List<string>(GameManager.Instance.lastScenesUnloaded);
        if (scenes.Contains("MapScreen"))
        {
            GameManager.Instance.battlefield.deckChosen = true;
            GameManager.Instance.LoadSceneAdditive("MapScreen","MainMenu");
        } else if (scenes.Contains("Fight"))
        {
            GameManager.Instance.battlefield.deckChosen = true;
            GameManager.Instance.LoadSceneAdditive("Fight","MainMenu");
        }
    }
    public void Hotel()
    {
        GameManager.Instance.LoadSceneAdditive("Hotel","MainMenu");
    }
    
    public void Settings()
    {
        GameManager.Instance.LoadSceneAdditive("Settings","MainMenu");
    }
    
    private void Start()
    {
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        GameManager.Instance.uiStateObject.HideTopBar();
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
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
