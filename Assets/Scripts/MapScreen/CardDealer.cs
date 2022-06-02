using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CardDealer : MonoBehaviour
{
    [SerializeField] Battlefield battlefield;
    [SerializeField] private CardShell[] shells;
    [SerializeField] private Button[] buttons;

    async void Start()
    {
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
                GameManager.Instance.LoadSceneAdditive("Shop","MapScreen");
                break;
            case MapCard.MapCardType.Boss:
            case MapCard.MapCardType.MiniBoss:
            case MapCard.MapCardType.Minion:
                FightCard fightCard = mapCard as FightCard;
                battlefield.InsertEnemies(fightCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.Event:
                GameManager.Instance.LoadSceneAdditive("Event","MapScreen");
                break;
        }
    }
}
