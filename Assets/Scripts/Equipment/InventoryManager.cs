using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Slider cardSlider;
    [SerializeField] private Button ButtonLeft;
    [SerializeField] private Button ButtonRight;
    [SerializeField] private Button[] EquipmentButtons;
    [SerializeField] private EquipmentCardShell cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform selector;
    [SerializeField] private bool selectorBig;
    [SerializeField] private PlayerBrain playerObject;
    [SerializeField] private EquippedPreview[] previewObjects;
    
    private ObjectPool<EquipmentCardShell> cardPool;

    public int currentHandIndex = 0;
    public int amountToTest = 10;
    public List<EquipmentCardShell> cards;
    public float cardDistance = 2;
    private async void Start()
    {
        cards = new List<EquipmentCardShell>();
        cardPool = new ObjectPool<EquipmentCardShell>(
            () =>
            {
                EquipmentCardShell card = Instantiate(cardPrefab, cardContainer);
                return card;
            },
            card =>
            {
                card.gameObject.SetActive(true);
            },
            card =>
            {
                card.gameObject.SetActive(false);
            },
            card => {
                Destroy(card);
            },
            true, 10, 100
        );
        GameManager.Instance.FixedHalfSecond += SizeSelector;
        GameManager.Instance.FixedSecond += SizeSelector;
        await Task.Delay(10);
        if (GameManager.Instance.battlefield.deckChosen)
        {
            playerObject = GameManager.Instance.battlefield.player;
        } else
        {
            playerObject = GameManager.Instance.metaPlayer;
        }
        
        for (var i = 0; i < playerObject.equippedSlots.Length; i++)
        {
            if (playerObject.equippedSlots[i]>=0
                && playerObject.EquippedCardExists(i))
            {
                previewObjects[i].SetEquipped(playerObject.GetEquippedCard(i));
            }
            else
            {
                playerObject.equippedSlots[i] = -1;
                if (i<3)
                {
                    previewObjects[i].SetEquipped(playerObject.defaultEquipment[i]);
                }
                else
                {
                    previewObjects[i].Clear();
                }
            }
        }
        GameManager.Instance.inputReader.Back+=Back;
        GameManager.Instance.eventSystem.SetSelectedGameObject(cardSlider.gameObject);
        
        // for (var index = 0; index < playerObject.playerInventory.Length; index++)
        // {
        //     EquipmentList equipmentList = playerObject.playerInventory[index];
        //     if (equipmentList.container.Count <= amountToTest)
        //     {
        //         int amount = 10+index -equipmentList.container.Count;
        //         for (int j = 0; j < amount; j++)
        //         {
        //             equipmentList.container.Add(GameManager.Instance.lootManager.GetItemCard(index));
        //         }
        //     }
        // }

        SelectHand(0);
    }

    public void OnDestroy()
    {
        GameManager.Instance.FixedHalfSecond -= SizeSelector;
        GameManager.Instance.FixedSecond -= SizeSelector;
        GameManager.Instance.inputReader.Back -= Back;
    }

    private void SizeSelector()
    {
        selectorBig = !selectorBig;
        if (selectorBig)
            {
                selector.localScale = Vector3.one * 0.35f;
            }
            else
            {
                selector.localScale = Vector3.one * 0.3f;
            }
    }

    public void Update()
    {
            for (var i = 0; i < cards.Count; i++)
            {
                EquipmentCardShell card = cards[i];
                Vector3 localPosition = card.transform.localPosition;
                float x = Math.Abs(cardSlider.value - i);
                localPosition = Vector3.Lerp(localPosition,
                    new Vector3((-cardSlider.value + i) * cardDistance, x > 1.5f ? -4.5f : x < 0.1f ? -2.75f : -3.75f,
                        0), Time.deltaTime * 10);
                localPosition.z = Math.Abs(localPosition.x) * 10;
                card.transform.localPosition = localPosition;
                card.Selected = localPosition.x < 0.25f && localPosition.x > -0.25f;
            }
    }
    
    public void SliderUp()
    {
        cardSlider.value += 1;
    }
    
    public void SliderDown()
    {
        cardSlider.value -= 1;
    }

    public void CheckButtons()
    {
        if (cardSlider.value==0)
        {
            ButtonLeft.interactable = false;
        }
        else
        {
            ButtonLeft.interactable = true;
        }

        if (Math.Abs(cardSlider.value - cards.Count) < 1.1f)
        {
            ButtonRight.interactable = false;
        }
        else
        {
            ButtonRight.interactable = true;
        }
    }
    
    public void SelectCard()
    {
        if (cards.Count==0)
        {
            return;
        }

        if (currentHandIndex<3)
        {
            if (cardSlider.value==0)
            {
                UnequipCard();
            }
            else
            {
                playerObject.Equip(currentHandIndex,(int)cardSlider.value-1);
                previewObjects[currentHandIndex].SetEquipped(playerObject.GetEquippedCard(currentHandIndex));
            }
        }
        else
        {
            playerObject.Equip(currentHandIndex,(int)cardSlider.value);
            previewObjects[currentHandIndex].SetEquipped(playerObject.GetEquippedCard(currentHandIndex));
        }
    }
    
    public void UnequipCard()
    {
        playerObject.Unequip(currentHandIndex);
        if (currentHandIndex<3)
        {
            previewObjects[currentHandIndex].SetEquipped(playerObject.defaultEquipment[currentHandIndex]);
        }
        else
        {
            previewObjects[currentHandIndex].Clear();
        }
    }
    
    public async void SelectHand(int index)
    {
        GameObject selected = GameManager.Instance.eventSystem.currentSelectedGameObject;
        currentHandIndex = index;
        selector.localPosition = previewObjects[currentHandIndex].transform.localPosition;

        foreach (Button button in EquipmentButtons)
        {
            button.interactable = false;
        }
        await MoveCardsDown();
        foreach (EquipmentCardShell card in cards)
        {
            cardPool.Release(card);
        }
        cards.Clear();
        int cardsToAdd = playerObject.playerInventory[index].container.Count;
        if (index<3)
        {
            EquipmentCardShell cardShell = cardPool.Get();
            cardShell.transform.localPosition = new Vector3(0, -9, 0);
            EquipmentDataContainer dataContainer = playerObject.defaultEquipment[index];
            cardShell.InsertItem(dataContainer);
            cards.Add(cardShell);
        }
        for (int i = 0; i < cardsToAdd; i++)
        {
            EquipmentCardShell cardShell = cardPool.Get();
            cardShell.transform.localPosition = new Vector3(i * 0.5f, -9, i);
            EquipmentDataContainer dataContainer = playerObject.playerInventory[index].container[i];
            cardShell.InsertItem(dataContainer);
            cards.Add(cardShell);
        }
        cardSlider.maxValue = cards.Count-1;
        cardSlider.value = 0;
        CheckButtons();
        await MoveCardsUp();
        foreach (Button button in EquipmentButtons)
        {
            button.interactable = true;
        }
        GameManager.Instance.eventSystem.SetSelectedGameObject(selected);
    }

    public async Task MoveCardsDown()
    {
        while (cardContainer.localPosition.y > -9)
        {
            cardContainer.localPosition = Vector3.Lerp(cardContainer.localPosition, new Vector3(0, -10, 0), Time.deltaTime * 10);
            await Task.Delay(10);
        }
    }
    
    public async Task MoveCardsUp()
    {
        while (cardContainer.localPosition.y <0f )
        {
            cardContainer.localPosition = Vector3.Lerp(cardContainer.localPosition, new Vector3(0, 1, 0), Time.deltaTime * 10);
            await Task.Delay(10);
        }
        cardContainer.localPosition = Vector3.zero;
    }

    public void Back()
    {
        if (GameManager.Instance.battlefield.deckChosen)
        {
            GameManager.Instance.LoadSceneAdditive("MapScreen","Equipment");
        }
        else
        {
            GameManager.Instance.LoadSceneAdditive("Hotel","Equipment");
        }
    }
}