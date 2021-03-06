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


    [SerializeField] private CardShell[] shells;
    [SerializeField] private Button[] buttons;
    [SerializeField] private DeckPreviewer deckPreviewer;

    async void Start()
    {



    }
    public void InsertDeck(DeckObject deck)
    {
        deckPreviewer.InsertDeck(deck);
    }

    public void GenerateCards()
    {
        List<MapCard> mapCards = new List<MapCard>();

        if (GameManager.Instance.battlefield.deck.shopAt.Contains(GameManager.Instance.battlefield.totalHands))
        {
            mapCards.Add(GameManager.Instance.battlefield.deck.DrawShopCard());
        }

        if (GameManager.Instance.battlefield.deck.eventAt.Contains(GameManager.Instance.battlefield.totalHands))
        {
            mapCards.Add(GameManager.Instance.battlefield.deck.DrawEventCard());
        }

        if (GameManager.Instance.battlefield.deck.MiniBossAt.Contains(GameManager.Instance.battlefield.totalHands))
        {
            mapCards.Add(GameManager.Instance.battlefield.deck.DrawMiniBossCard());
        }

        for (int index = mapCards.Count; index < 3; index++)
        {
            mapCards.Add(GameManager.Instance.battlefield.deck.DrawMinionCard());
        }

        mapCards.Sort((a, b) => Random.Range(-1, 2));

        if (GameManager.Instance.battlefield.totalHands == GameManager.Instance.battlefield.deck.BossAt)
        {
            mapCards.Clear();
            mapCards.Add(GameManager.Instance.battlefield.deck.DrawBossCard());
        }

        for (var i = 0; i < mapCards.Count; i++)
        {
            shells[i].InsertCard(mapCards[i], i == 0);
        }

        foreach (CardShell shell in shells)
        {
            if (!shell.hasBrain)
            {
                shell.gameObject.SetActive(false);
            }
        }
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

        GameManager.Instance.eventSystem.SetSelectedGameObject(buttons[0].gameObject);

    }

    public virtual void SetupAndLoad(int selectedCard)
    {
        MapCard mapCard = shells[selectedCard].card as MapCard;
        switch (mapCard!.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                GameManager.Instance.LoadSceneAdditive("ShopScreen", "MapScreen");
                break;
            case MapCard.MapCardType.Boss:
            case MapCard.MapCardType.MiniBoss:
            case MapCard.MapCardType.Minion:
                FightCard fightCard = mapCard as FightCard;
                GameManager.Instance.battlefield.InsertEnemies(fightCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight", "MapScreen");
                break;
            case MapCard.MapCardType.Event:
                // GameManager.Instance.GameManager.Instance.battlefield.SetEvent(((EventCard)mapCard).eventObject);//todo figure out event clusters
                GameManager.Instance.LoadSceneAdditive("EventScreen", "MapScreen");
                break;
        }

        GameManager.Instance.battlefield.randomState = Random.state;
    }

    public virtual void Equipment()
    {
        GameManager.Instance.LoadSceneAdditive("Equipment", "MapScreen");
    }

    public void ToggleButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = !button.interactable;
        }
    }
    
    public void DisableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }
    
    public void EnableButtons()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }
}