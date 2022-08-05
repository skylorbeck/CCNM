using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HotelManager : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] GenericMenuV1 menu;


    private async void Start()
    {
        await Task.Delay(10);
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        GameManager.Instance.uiStateObject.ShowTopBar();
        GameManager.Instance.uiStateObject.Ping("The Hotel");
        GetComponent<Animator>().SetTrigger("FadeIn");
        MusicManager.Instance.PlayTrack("Hotel");
      
    }

    public void Update()
    {
    }

    public void EnableButtons()
    {
        button.interactable = true;
    }
    


    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void Back()
    {
        SoundManager.Instance.PlayUiBack();
        GameManager.Instance.LoadSceneAdditive("MainMenu","Hotel");
    }
    public void Equipment()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Equipment","Hotel");
    }
    public void Leveling()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Leveling","Hotel");
    }
    public void Shredding()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("CardShredding","Hotel");
    }
    public void CardPacks()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("CardPacks","Hotel");
    }
    public void Capsules()
    {
        SoundManager.Instance.PlayUiAccept();
        GameManager.Instance.LoadSceneAdditive("Capsules","Hotel");
    }
}
