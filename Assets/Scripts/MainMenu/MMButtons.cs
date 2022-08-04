using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMButtons : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] GenericMenuV1 menu;

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
        GameManager.Instance.eventSystem.SetSelectedGameObject(button.gameObject);
        MusicManager.Instance.PlayTrack(GameManager.Instance.musicRegistry.GetMusic(2));
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
        DOTween.To(()=>menu.transform.localPosition, x=>menu.transform.localPosition = x, new Vector3(0,0,0), 0.5f);
            button.interactable = true;
    }
}
