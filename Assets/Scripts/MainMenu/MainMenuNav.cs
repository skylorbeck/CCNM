using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenuNav : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] MMButtons mmButtons;
    [SerializeField] MenuState menuState = MenuState.Main;
    [SerializeField] GameObject[] menuImages;
    async void Start()
    {
        await Task.Delay(10);
        GameManager.Instance.eventSystem.SetSelectedGameObject(button.gameObject);
        GameManager.Instance.inputReader.PadLeft += DecreaseState;
        GameManager.Instance.inputReader.PadRight += IncreaseState;
        UpdateButton();
        MusicManager.Instance.PlayTrack(GameManager.Instance.musicRegistry.GetMusic(2));
    }

    public void OnDestroy()
    {
        GameManager.Instance.inputReader.PadLeft -= DecreaseState;
        GameManager.Instance.inputReader.PadRight -= IncreaseState;
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
            case MenuState.Main:
                if (GameManager.Instance.battlefield.deckChosen)
                {
                    buttonText.text = "Continue";
                    button.onClick.AddListener(mmButtons.ResumeGame);
                } else
                {
                    buttonText.text = "Start";
                    button.onClick.AddListener(mmButtons.NewGame);
                }
                break;
            case MenuState.Hotel:
                buttonText.text = "Hotel";
                button.onClick.AddListener(mmButtons.Hotel);
                break;
            case MenuState.Options:
                buttonText.text = "Options";
                button.onClick.AddListener(mmButtons.Settings);
                break;
            case MenuState.Credits:
                buttonText.text = "Credits";
                // button.onClick.AddListener(mmButtons.Credits);//todo
                break;
            case MenuState.Quit:
                buttonText.text = "Quit";
                button.onClick.AddListener(Application.Quit);
                break;
        }
    }
    
   public enum MenuState
    {
        Main,
        Hotel,
        Options,
        Credits,
        Quit
    }
    
    public void SetMenuState(MenuState state)
    {
        menuState = state;
        UpdateButton();
    }

    public void IncreaseState()
    {
        menuState++;
        SoundManager.Instance.PlayUiClick();
        
        if (menuState > MenuState.Quit)
        {
            menuState = MenuState.Main;
        }
        UpdateButton();
    }

    public void DecreaseState()
    {
        menuState--;
        SoundManager.Instance.PlayUiClick();

        if (menuState < MenuState.Main)
        {
            menuState = MenuState.Quit;
        }
        UpdateButton();
    }


    void Update()
    {
        for (var i = 0; i < menuImages.Length; i++)
        {
            menuImages[i].transform.localPosition = Vector3.Lerp(menuImages[i].transform.localPosition, new Vector3(i*100-((int)menuState*100), 0, 0),Time.deltaTime*10);
        }
    }

    void FixedUpdate()
    {
        
    }
}
