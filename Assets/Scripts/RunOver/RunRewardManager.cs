using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RunRewardManager : MonoBehaviour
{
    public EquipmentCardShell cardPrefab;
    public Transform cardContainer;
    public List<EquipmentCardShell> cards = new List<EquipmentCardShell>();
    public Slider cardSlider;
    async void Start()
    {
        await Task.Delay(10);
        //todo penalty for death
        GameManager.Instance.metaPlayer.CopyEgo(GameManager.Instance.battlefield.player);
        // GameManager.Instance.metaPlayer.CopyCards(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCredits(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCardSouls(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCardPacks(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopyCapsules(GameManager.Instance.battlefield.player);
        GameManager.Instance.metaPlayer.CopySuperCapsules(GameManager.Instance.battlefield.player);
        if (GameManager.Instance.battlefield.player.currentHealth <= 0)
        {
            GameManager.Instance.metaPlayer.AddCardPack(1);//todo replace with algorithm based on score
        }
        
        for (var i = 0; i < GameManager.Instance.battlefield.player.playerInventory.Length; i++)
        {
            for (var j = 0; j < GameManager.Instance.battlefield.player.playerInventory[i].container.Count; j++)
            {
                EquipmentDataContainer data = GameManager.Instance.battlefield.player.playerInventory[i].container[j];
                if (!data.indestructible)
                {
                    var card = Instantiate(cardPrefab, cardContainer);
                    card.InsertItem(data);
                    card.ToggleShredMark();
                    cards.Add(card);
                }
            }
        }
        cards.Sort((card1, card2) => card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality)*-1);
        cards[0].SetShredMark(false);
        cardSlider.maxValue = cards.Count - 1;
 
    }

    void Update()
    {
        for (var j = 0; j < cards.Count; j++)
        {
            Transform cardTransform = cards[j].transform;
            Vector3 localPosition = cardTransform.localPosition;

            localPosition = Vector3.Lerp(localPosition,
                new Vector3(1.5f * (j - cardSlider.value), 0, 0),
                Time.deltaTime * 10);
            if (Math.Abs(j - cardSlider.value) < 0.1f)
            {
                cardTransform.localScale = Vector3.Lerp(cardTransform.localScale, Vector3.one, Time.deltaTime * 10);
                localPosition = new Vector3(localPosition.x, localPosition.y, 0);
            }
            else
            {
                cardTransform.localScale =
                    Vector3.Lerp(cardTransform.localScale, Vector3.one * 0.5f, Time.deltaTime * 10);
                localPosition = new Vector3(localPosition.x, localPosition.y, 1);
            }

            cardTransform.localPosition = localPosition;

        }
    }

    void FixedUpdate()
    {
        
    }

    public async void MoveOn()
    {
        int soulsGained = 0;
        foreach (EquipmentCardShell card in cards)
        {
            if (card.MarkedForShred)
            {
                soulsGained+= GameManager.Instance.metaPlayer.ShredCard(card.EquipmentData);
            } else
            {
                GameManager.Instance.metaPlayer.AddCardToInventory(card.EquipmentData);
            }
        }
        TextPopController.Instance.PopPositive("You gained " + soulsGained + " souls!",Vector3.zero, false);
        await Task.Delay(1000);
        GameManager.Instance.battlefield.ClearBattlefield();
        GameManager.Instance.saveManager.SaveRun();
        GameManager.Instance.LoadSceneAdditive("MainMenu","RunOver");
    }

    public void SelectCardToKeep()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            cards[i].SetShredMark(Math.Abs(i - cardSlider.value) > 0.1f);
        }
    }
}
