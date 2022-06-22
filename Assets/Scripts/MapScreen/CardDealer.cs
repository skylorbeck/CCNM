using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardDealer : MonoBehaviour
{
    [SerializeField] Battlefield battlefield;
    [SerializeField] TextMeshProUGUI totalCardsText;
    [SerializeField] private CardShell[] shells;
    [SerializeField] private Button[] buttons;
    [SerializeField] private TextMeshProUGUI[] pauseText;
    [SerializeField] private GraphicRaycaster pauseRaycaster;
    [SerializeField] private StatDisplay playerStatDisplay;

    async void Start()
    {
        if (battlefield.randomState==null)
        {
            battlefield.randomState = Random.state;
        }
        else
        {
            Random.state = battlefield.randomState.Value;
        }
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(0, 0,true);
        }
        totalCardsText.text = battlefield.totalHands + "/" + battlefield.deck.BossAt;
        if (battlefield.totalHands == battlefield.deck.BossAt)
        {
            shells[0].InsertCard(battlefield.deck.DrawBossCard(), false);
            shells[1].gameObject.SetActive(false);
            shells[2].gameObject.SetActive(false);
        }
        else if (battlefield.totalHands == battlefield.deck.MiniBossAt)
        {
            shells[0].gameObject.SetActive(false);
            shells[1].InsertCard(battlefield.deck.DrawMiniBossCard(), false);
            shells[2].InsertCard(battlefield.deck.DrawMiniBossCard(), true);
        }
        else
        {
            for (var i = 0; i < shells.Length; i++)
            {
                CardShell shell = shells[i];
                shell.InsertCard(battlefield.deck.DrawRandomCard(), i == 0);
            }
        }
        await Task.Delay(1000);//this is to wait for the screen to load in before dealing the cards
        DealCards();
        GameManager.Instance.eventSystem.SetSelectedGameObject(buttons[0].gameObject);

    }

    public async void DealCards()
    {
        for (var i = 0; i < shells.Length; i++)
        {
            CardShell shell = shells[i];
            if (shell.hasBrain)
            {
                shell.Flip();
                await Task.Delay(250);
                buttons[i].interactable = true;
            }
        }
        GameManager.Instance.inputReader.Back+=Back;

    }

    public virtual void SetupAndLoad(int selectedCard)
    {
        MapCard mapCard = shells[selectedCard].card as MapCard;
        battlefield.TotalHandsPlus();
        switch (mapCard!.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                GameManager.Instance.LoadSceneAdditive("ShopScreen","MapScreen");
                break;
            case MapCard.MapCardType.Boss:
            case MapCard.MapCardType.MiniBoss:
            case MapCard.MapCardType.Minion:
                FightCard fightCard = mapCard as FightCard;
                battlefield.InsertEnemies(fightCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.Event:
                GameManager.Instance.LoadSceneAdditive("EventScreen","MapScreen");
                break;
        }

        battlefield.randomState = Random.state;
    }

    public virtual void Equipment()
    {
        GameManager.Instance.LoadSceneAdditive("Equipment","MapScreen");
    }
    
    public void Back()
    {
        GameManager.Instance.uiStateObject.TogglePause();
        
        foreach (Button button in buttons)
        {
            button.interactable = !button.interactable;
        }
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ?1 :0, 0.25f,true);
        }
        playerStatDisplay.FadeInOut();
        pauseRaycaster.enabled = GameManager.Instance.uiStateObject.isPaused;
        if (GameManager.Instance.uiStateObject.isPaused)
        {
            GameManager.Instance.eventSystem.SetSelectedGameObject(buttons[0].gameObject);
        }
        else
        { 
            GameManager.Instance.eventSystem.SetSelectedGameObject(buttons[4].gameObject);
        }
    }


    private void OnDestroy()
    {
        GameManager.Instance.inputReader.Back-=Back;
    }
    
    public void Quit()
    {
        Back();
        GameManager.Instance.LoadSceneAdditive("MainMenu","MapScreen");
    }
}
