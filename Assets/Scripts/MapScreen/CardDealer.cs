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
        for (var i = 0; i < shells.Length; i++)
        {
            CardShell shell = shells[i];
            shell.InsertCard(battlefield.deck.DrawRandomCard(),i==0);
        }
        DealCards();
    }

    public async void DealCards()
    {
        await Task.Delay(1000);
        foreach (CardShell shell in shells)
        {
            shell.Flip();
            await Task.Delay(250);
        }
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public virtual void SetupAndLoad(int selectedCard)
    {
        MapCard mapCard = shells[selectedCard].card as MapCard;
        switch (mapCard!.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                GameManager.Instance.LoadSceneAdditive("Shop","MapScreen");
                break;
            case MapCard.MapCardType.Boss:
                BossCard bossCard = mapCard as BossCard;
                battlefield.InsertEnemies(bossCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.MiniBoss:
                MiniBossCard miniBossCard = mapCard as MiniBossCard;
                battlefield.InsertEnemies(miniBossCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.Minion:
                MinionCard minionCard = mapCard as MinionCard;
                battlefield.InsertEnemies(minionCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.Event:
                GameManager.Instance.LoadSceneAdditive("Event","MapScreen");
                break;
        }
    }
}
