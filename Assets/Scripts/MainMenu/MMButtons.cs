using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMButtons : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] GenericMenuV1 menu;
    [SerializeField] GenericMenuEntry continueEntry;

    public void NewGame()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.battlefield.Reset();
        GameManager.Instance.runSettings.Reset();
        // GameManager.Instance.LoadSceneAdditive("RunSettings","MainMenu");//todo put this back in when the run settings are done
        GameManager.Instance.LoadSceneAdditive("MapScreen", "MainMenu");
    }

    public void ResumeGame()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("MapScreen", "MainMenu");
    }

    public void MainMenu()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("MainMenu","MainMenu");
    }
    
    public void Settings()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Settings","MainMenu");
    }
    public void Equipment()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Equipment","MainMenu");
    }
    public void Leveling()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Leveling","MainMenu");
    }
    public void Shredding()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("CardShredding","MainMenu");
    }
    public void CardPacks()
    {
        if (GameManager.Instance.metaPlayer.totalEquipment>GameManager.Instance.metaPlayer.maximumEquipmentSlots-5)
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("Your inventory is full!",Vector3.zero,false);
            button.interactable = true;
        }
        else
        {
            SoundManager.Instance.PlayUiAccept();
            GameManager.Instance.LoadSceneAdditive("CardPacks","MainMenu");
        }
    }
    public void Capsules()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Capsules","MainMenu");
    }
    public void Gems()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Gems","MainMenu");
    }
    public void VIP()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Store","MainMenu");
    }
    private async void Start()
    {
        // await Task.Delay(10);
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.uiStateObject.HideTopBar();
        // GameManager.Instance.eventSystem.SetSelectedGameObject(button.gameObject);
        MusicManager.Instance.PlayTrack(2);
        await Task.Delay(10);
        if (GameManager.Instance.battlefield.runStarted)
        {
            menu.AddEntry(continueEntry,2);
            menu.SetSelected(2);
        } else menu.SetSelected(1);
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
        DOTween.To(()=>menu.transform.localPosition, x=>menu.transform.localPosition = x, new Vector3(0,-0.5f,0), 0.5f);
            button.interactable = true;
    }
}
