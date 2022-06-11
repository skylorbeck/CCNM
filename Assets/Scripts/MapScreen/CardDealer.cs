using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardDealer : MonoBehaviour
{
    [SerializeField] Battlefield battlefield;
    [SerializeField] TextMeshProUGUI totalCardsText;
    [SerializeField] private CardShell[] shells;
    [SerializeField] private Button[] buttons;
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
    }

    public virtual void SetupAndLoad(int selectedCard)
    {
        MapCard mapCard = shells[selectedCard].card as MapCard;
        battlefield.TotalHandsPlus();
        switch (mapCard!.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                GameManager.Instance.LoadSceneAdditive("ShopScreen",false,"MapScreen");
                break;
            case MapCard.MapCardType.Boss:
            case MapCard.MapCardType.MiniBoss:
            case MapCard.MapCardType.Minion:
                FightCard fightCard = mapCard as FightCard;
                battlefield.InsertEnemies(fightCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight",false,"MapScreen");
                break;
            case MapCard.MapCardType.Event:
                GameManager.Instance.LoadSceneAdditive("EventScreen",false,"MapScreen");
                break;
        }

        battlefield.randomState = Random.state;
    }

    public virtual void Equipment()
    {
        GameManager.Instance.LoadSceneAdditive("Equipment",false,"MapScreen");
    }
}
