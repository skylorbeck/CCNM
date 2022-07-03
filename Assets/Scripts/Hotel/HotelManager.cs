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
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] MenuState menuState = MenuState.Equipment;
    [SerializeField] GameObject[] menuImages;
    private async void Start()
    {
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        GameManager.Instance.uiStateObject.ShowTopBar();
        GameManager.Instance.uiStateObject.Ping("The Hotel");
        GameManager.Instance.inputReader.PadLeft += DecreaseState;
        GameManager.Instance.inputReader.PadRight += IncreaseState;
        await Task.Delay(100);
        GetComponent<Animator>().SetTrigger("FadeIn");
        UpdateButton();
    }

    public void Update()
    {
        for (var i = 0; i < menuImages.Length; i++)
        {
            menuImages[i].transform.localPosition = Vector3.Lerp(menuImages[i].transform.localPosition, new Vector3(i*100-((int)menuState*100), 0, 0),Time.deltaTime*10);
        }
    }

    public void EnableButtons()
    {
        button.interactable = true;
    }
    public enum MenuState
    {
        Equipment,
        Shredding,
        Packs,
        Capsules,
        Quit
    }
    public void UpdateButton()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            button.interactable = false;
        });
        switch (menuState)
        {
            case MenuState.Equipment:
                    buttonText.text = "Equipment";
                    button.onClick.AddListener(Equipment);
                break;
            case MenuState.Shredding:
                buttonText.text = "Shredder";
                button.onClick.AddListener(CardShredding);
                break;
            case MenuState.Packs:
                buttonText.text = "Card Packs";
                button.onClick.AddListener(CardPacks);
                break;
            case MenuState.Capsules:
                buttonText.text = "Capsules";
                button.onClick.AddListener(Capsules);
                break;
            case MenuState.Quit:
                buttonText.text = "Back";
                button.onClick.AddListener(Back);
                break;
        }
    }
    public void SetMenuState(MenuState state)
    {
        menuState = state;
        UpdateButton();
    }

    public void IncreaseState()
    {
        menuState++;
        if (menuState > MenuState.Quit)
        {
            menuState = MenuState.Equipment;
        }
        UpdateButton();
    }

    public void DecreaseState()
    {
        menuState--;
        if (menuState < MenuState.Equipment)
        {
            menuState = MenuState.Quit;
        }
        UpdateButton();
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
        GameManager.Instance.inputReader.PadLeft -= DecreaseState;
        GameManager.Instance.inputReader.PadRight -= IncreaseState;
    }

    public void Back()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu","Hotel");
    }
    public void Equipment()
    {
        GameManager.Instance.LoadSceneAdditive("Equipment","Hotel");
    }
    public void CardShredding()
    {
        GameManager.Instance.LoadSceneAdditive("CardShredding","Hotel");
    }
    
    public void CardPacks()
    {
        GameManager.Instance.LoadSceneAdditive("CardPacks","Hotel");
    }
    
    public void Capsules()
    {
        GameManager.Instance.LoadSceneAdditive("Capsules","Hotel");
    }
}
