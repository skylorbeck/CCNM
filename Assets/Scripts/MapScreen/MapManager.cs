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
    [SerializeField] private Button startButton;
    [SerializeField] private SpriteRenderer[] curtainsBack;
    [SerializeField] private SpriteRenderer[] curtains;

    async void Start()
    {
        await Task.Delay(10);
        playerStatDisplay.FadeInOut();

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



        if (GameManager.Instance.battlefield.runStarted)
        {
            GameManager.Instance.uiStateObject.Ping("Saved game");
            deckManager.gameObject.SetActive(false);
            cardDealer.transform.localPosition = new Vector3(0,-1,-1);
            deckManager.transform.localPosition = new Vector3(-100, 0, 0);
            cardDealer.InsertDeck(GameManager.Instance.deck);
            InsertDeck();
            cardDealer.GenerateCards();
            selectDeckButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            totalCardsText.text = GameManager.Instance.battlefield.totalHands + "/" +
                                  GameManager.Instance.deck
                                      .bossAt;
            await Task.Delay(1500);
            cardDealer.DealCards();
        }
        else
        {
            GameManager.Instance.uiStateObject.Ping("Select a Deck to Play!");
            startButton.interactable = GameManager.Instance.battlefield.deckChosen;
            cardDealer.DisableButtons();
            cardDealer.transform.localPosition = new Vector3(0, -100, -1);
            deckManager.transform.localPosition = Vector3.zero;
            deckManager.InitalizeDecks();
            totalCardsText.text = "";
            if (GameManager.Instance.battlefield.deckChosen)
            {
                deckManager.SetSelectedDeck(GameManager.Instance.deck);
                InsertDeck();
            }
        }

        GameManager.Instance.inputReader.Back += Back;
    }

    public void Back()
    {
        if (GameManager.Instance.battlefield.runStarted)
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
                if (GameManager.Instance.battlefield.runStarted) //might need a deckchosen check here
                {
                    cardDealer.EnableButtons();
                }
            }
        } else
        {
            GameManager.Instance.LoadSceneAdditive("RunSettings", "MapScreen");
        }
    }

    public void Quit()
    {
        if (GameManager.Instance.battlefield.runStarted)
        {
            GameManager.Instance.saveManager.SaveRun();
        } else if (GameManager.Instance.battlefield.deckChosen)
        {
            GameManager.Instance.saveManager.SaveMeta();
        }
        Back();
        GameManager.Instance.LoadSceneAdditive("MainMenu", "MapScreen");
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back -= Back;
    }

    public void InsertDeck()
    {
        for (var i = 0; i < curtains.Length; i++)
        {
            var i1 = i;
            DOTween.To(
                () => curtains[i1].color,
                x => curtains[i1].color = x,
                GameManager.Instance.deck.colors[0],
                0.5f);
        }
        for (var i = 0; i < curtainsBack.Length; i++)
        {
            var i1 = i;
            DOTween.To(
                () => curtainsBack[i1].color,
                x => curtainsBack[i1].color = x,
                GameManager.Instance.deck.colors[1],
                0.5f);
        }
        totalCardsText.text = GameManager.Instance.battlefield.totalHands + "/" +
                              GameManager.Instance.deck
                                  .bossAt;
    }

    public async void StartGame()
    {
        selectDeckButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        DOTween.To(() => deckManager.transform.localPosition, x => deckManager.transform.localPosition = x,
            new Vector3(-100, 0, 0), 5f);
        DOTween.To(() => cardDealer.transform.localPosition, x => cardDealer.transform.localPosition = x,
            new Vector3(0, -1, -1), 1f);
        cardDealer.InsertDeck(GameManager.Instance.deck);
        GameManager.Instance.battlefield.ClearBattlefield();
        GameManager.Instance.battlefield.StartRun();
        cardDealer.GenerateCards();
        await Task.Delay(1250);
        cardDealer.DealCards();
    }
    
    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}