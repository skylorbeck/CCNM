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
    [SerializeField] private Image cursor;
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
        GameManager.Instance.inputReader.PadAny += NavUpdate;
        GameManager.Instance.inputReader.ClickEvent += NavUpdateMouse;
        await Task.Delay(10);
        
        for (var i = 0; i < playerObject.equippedCards.Length; i++)
        {
     
            if (playerObject.equippedSlots[i])
            {
                previewObjects[i].SetEquipped(playerObject.equippedCards[i]);
            }
            else
            {
                previewObjects[i].Clear();
            }

        }
        //todo delete this
        for (var index = 0; index < playerObject.equipmentDataContainers.Length; index++)
        {
            EquipmentList equipmentList = playerObject.equipmentDataContainers[index];
            if (equipmentList.container.Count <= amountToTest)
            {
                int amount = amountToTest -equipmentList.container.Count;
                for (int j = 0; j < amount; j++)
                {
                    EquipmentDataContainer dataContainer = new EquipmentDataContainer();
                    dataContainer.InsertItem(
                        GameManager.Instance.equipmentRegistries[index].itemCards[
                            Random.Range(0, GameManager.Instance.equipmentRegistries[index].itemCards.Length)]);
                    dataContainer.GenerateData();
                    equipmentList.container.Add(dataContainer);
                }
            }
        }

        SelectHand(0);
    }

    public void OnDestroy()
    {
        GameManager.Instance.FixedHalfSecond -= SizeSelector;
        GameManager.Instance.FixedSecond -= SizeSelector;
        GameManager.Instance.inputReader.PadAny -= NavUpdate;
        GameManager.Instance.inputReader.ClickEvent -= NavUpdateMouse;
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
            if (GameManager.Instance.eventSystem.currentSelectedGameObject!=cardSlider.gameObject)
            {
                GameManager.Instance.eventSystem.SetSelectedGameObject(ButtonRight.gameObject);
            }
        }
        else
        {
            ButtonLeft.interactable = true;
        }

        if (Math.Abs(cardSlider.value - cards.Count) < 1.1f)
        {
            ButtonRight.interactable = false;
            if (GameManager.Instance.eventSystem.currentSelectedGameObject!=cardSlider.gameObject)
            {
                GameManager.Instance.eventSystem.SetSelectedGameObject(ButtonLeft.gameObject);
            }
        }
        else
        {
            ButtonRight.interactable = true;
        }
    }
    
    public void SelectCard()
    {
        EquipmentDataContainer equipmentData = cards[(int) cardSlider.value].EquipmentData;
        playerObject.Equip(equipmentData);
        previewObjects[currentHandIndex].SetEquipped(equipmentData);
    }
    
    public async void SelectHand(int index)
    {
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
        int cardsToAdd = playerObject.equipmentDataContainers[index].container.Count;
        for (int i = 0; i < cardsToAdd; i++)
        {
            EquipmentCardShell cardShell = cardPool.Get();
            cardShell.transform.localPosition = new Vector3(i * 0.5f, -3, i);
            EquipmentDataContainer dataContainer = playerObject.equipmentDataContainers[index].container[i];
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
    }
    
    private void NavUpdateMouse()
    {
        DisablePointer();
    }
    private async void NavUpdate()
    {
        if (!cursor.enabled)
        {
            EnablePointer();
        }

        await Task.Delay(50);
        cursor.transform.position = GameManager.Instance.eventSystem.currentSelectedGameObject.transform.position;

    }

    private void DisablePointer()
    {
        cursor.enabled = false;
    }

    private void EnablePointer()
    {
        cursor.enabled = true;
    }

}