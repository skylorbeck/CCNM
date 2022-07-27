using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            // Debug.Log("No random state");
            Random.InitState(DateTime.Now.Millisecond);
            battlefield.randomState = Random.state;
        }
        else
        {
            // Debug.Log("Using random state");
            Random.state = battlefield.randomState.Value;
        }
        
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(0, 0,true);
        }
        totalCardsText.text = battlefield.totalHands + "/" + battlefield.deck.BossAt;
        
        List<MapCard> mapCards = new List<MapCard>();
        
        if (battlefield.deck.shopAt.Contains(battlefield.totalHands))
        {
            mapCards.Add(battlefield.deck.DrawShopCard());
        }
        if (battlefield.deck.eventAt.Contains(battlefield.totalHands))
        {
            mapCards.Add(battlefield.deck.DrawEventCard());
        }
        if (battlefield.deck.MiniBossAt.Contains(battlefield.totalHands))
        {
            mapCards.Add(battlefield.deck.DrawMiniBossCard());
        }
        for (int index = mapCards.Count; index < 3; index++)
        {
            mapCards.Add(battlefield.deck.DrawMinionCard());
        }
        
mapCards.Sort((a, b) => Random.Range(-1, 1));
        
        if (battlefield.totalHands == battlefield.deck.BossAt)
        {
            mapCards.Clear();
            mapCards.Add(battlefield.deck.DrawBossCard());
        }
        
        for (var i = 0; i < mapCards.Count; i++)
        {
                shells[i].InsertCard(mapCards[i],i==0);
        }
        foreach (CardShell shell in shells)
        {
            if (!shell.hasBrain)
            {
                shell.gameObject.SetActive(false);
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
                // GameManager.Instance.battlefield.SetEvent(((EventCard)mapCard).eventObject);
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
        GameManager.Instance.saveManager.SaveRun();
        Back();
        GameManager.Instance.LoadSceneAdditive("MainMenu","MapScreen");
    }
}
