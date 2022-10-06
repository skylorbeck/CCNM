using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
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
    [SerializeField] private Vector3[] positions;
    [SerializeField] private EventCard eventCard;
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

        if(GameManager.Instance.battlefield.totalHands % GameManager.Instance.deck.shopEvery==0)
        {
            mapCards.Add(GameManager.Instance.deck.DrawShopCard());
        }

        if (GameManager.Instance.battlefield.totalHands % GameManager.Instance.deck.eventEvery==0)
        {
            mapCards.Add(eventCard);
        }

        if (GameManager.Instance.battlefield.totalHands % GameManager.Instance.deck.miniBossEvery==0)
        {
            mapCards.Add(GameManager.Instance.deck.DrawMiniBossCard());
        }

        for (int index = mapCards.Count; index < 3; index++)
        {
            mapCards.Add(GameManager.Instance.deck.DrawMinionCard());
        }

        mapCards.Sort((a, b) => Random.Range(-1, 2));

        if (GameManager.Instance.battlefield.totalHands == GameManager.Instance.battlefield.maximumHands)
        {
            mapCards.Clear();
            mapCards.Add(GameManager.Instance.deck.DrawBossCard());
        }

        for (var i = 0; i < mapCards.Count; i++)
        {
            shells[i].InsertCard(mapCards[i], i == 0);
            shells[i].SetTargetPosition(positions[i]);
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
        GameManager.Instance.uiStateObject.Ping("Pick A Card!");
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
        deckPreviewer.transform.DOLocalMove(new Vector3(0,-4f,0), 0.5f);
        GameManager.Instance.eventSystem.SetSelectedGameObject(buttons[0].gameObject);

    }

    public virtual void SetupAndLoad(int selectedCard)
    {
        MapCard mapCard = shells[selectedCard].card as MapCard;
        switch (mapCard!.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                // ShopCard shopCard = mapCard as ShopCard;
                //todo different shop based on shop type stored in shopcard
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