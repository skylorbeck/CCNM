using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private CardDealer cardDealer;
    [SerializeField] private DeckManager deckManager;

    [SerializeField] TextMeshProUGUI totalCardsText;
    [SerializeField] private TextMeshProUGUI[] pauseText;
    [SerializeField] private GraphicRaycaster pauseRaycaster;
    [SerializeField] private StatDisplay playerStatDisplay;
    [SerializeField] private Button[] cardButtons;
    [SerializeField] private Button selectDeckButton;

    async void Start()
    {
        await Task.Delay(10);

        if (GameManager.Instance.battlefield.randomState == null)
        {
            // Debug.Log("No random state");
            Random.InitState(DateTime.Now.Millisecond);
            GameManager.Instance.battlefield.randomState = Random.state;
        }
        else
        {
            // Debug.Log("Using random state");
            Random.state = GameManager.Instance.battlefield.randomState.Value;
        }

        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(0, 0, true);
        }


        playerStatDisplay.FadeInOut();

        if (GameManager.Instance.battlefield.runStarted)
        {
            deckManager.gameObject.SetActive(false);
            cardDealer.transform.localPosition = Vector3.zero;
            deckManager.transform.localPosition = new Vector3(-100, 0, 0);
            cardDealer.GenerateCards();
            await Task.Delay(10);
            cardDealer.DealCards();
            selectDeckButton.gameObject.SetActive(false);
            totalCardsText.text = GameManager.Instance.battlefield.totalHands + "/" +
                                  GameManager.Instance.battlefield.deck
                                      .BossAt;
        }
        else
        {
            cardDealer.DisableButtons();
            cardDealer.transform.localPosition = new Vector3(0, -100, 0);
            deckManager.transform.localPosition = Vector3.zero;
            deckManager.InitalizeDecks();
            totalCardsText.text = "";
        }

        GameManager.Instance.inputReader.Back += Back;
    }

    public void Back()
    {
        GameManager.Instance.uiStateObject.TogglePause();

        foreach (Button button in cardButtons)
        {
            button.interactable = !button.interactable;
        }

        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ? 1 : 0, 0.25f, true);
        }

        playerStatDisplay.FadeInOut();
        pauseRaycaster.enabled = GameManager.Instance.uiStateObject.isPaused;
        if (GameManager.Instance.uiStateObject.isPaused)
        {
            GameManager.Instance.eventSystem.SetSelectedGameObject(cardButtons[0].gameObject);
            cardDealer.DisableButtons();
        }
        else
        {
            GameManager.Instance.eventSystem.SetSelectedGameObject(cardButtons[1].gameObject);
            if (GameManager.Instance.battlefield.runStarted)//might need a deckchosen check here
            {
                cardDealer.EnableButtons();
            }
        }
    }

    public void Quit()
    {
        GameManager.Instance.saveManager.SaveRun();
        Back();
        GameManager.Instance.LoadSceneAdditive("MainMenu", "MapScreen");
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public async void InsertDeck()
    {
        selectDeckButton.gameObject.SetActive(false);
        deckManager.SelectDeck();
        DOTween.To(() => deckManager.transform.localPosition, x => deckManager.transform.localPosition = x,
            new Vector3(-100, 0, 0), 5f);
        DOTween.To(() => cardDealer.transform.localPosition, x => cardDealer.transform.localPosition = x,
            new Vector3(0, 0, 0), 1f);
        cardDealer.InsertDeck(GameManager.Instance.battlefield.deck);
        GameManager.Instance.battlefield.StartRun();
        cardDealer.GenerateCards();
        await Task.Delay(1000);
        cardDealer.DealCards();
    }
    
   
    
    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}