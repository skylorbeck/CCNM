using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    private MapCard selectedCard;
    
    [SerializeField] Battlefield battlefield;

    public async void DealCards()
    {
        await Task.Delay(1000);
        selectedCard = battlefield.deck.DrawMinionCard();
        await Task.Delay(1000);
        SetupAndLoad();
    }

    public virtual void SetupAndLoad()
    {
        switch (selectedCard.mapCardType)
        {
            case MapCard.MapCardType.Shop:
                GameManager.Instance.LoadSceneAdditive("Shop","MapScreen");
                break;
            case MapCard.MapCardType.Boss:
                BossCard bossCard = selectedCard as BossCard;
                battlefield.InsertEnemies(bossCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.MiniBoss:
                MiniBossCard miniBossCard = selectedCard as MiniBossCard;
                battlefield.InsertEnemies(miniBossCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.Minion:
                MinionCard minionCard = selectedCard as MinionCard;
                battlefield.InsertEnemies(minionCard!.enemies);
                GameManager.Instance.LoadSceneAdditive("Fight","MapScreen");
                break;
            case MapCard.MapCardType.Event:
                GameManager.Instance.LoadSceneAdditive("Event","MapScreen");
                break;
        }
    }
}
