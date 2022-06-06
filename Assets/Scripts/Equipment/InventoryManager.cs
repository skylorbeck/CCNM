using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Slider cardSlider;
    [SerializeField] private Button ButtonLeft;
    [SerializeField] private Button ButtonRight;
    [SerializeField] private EquipmentCardShell cardPrefab;
    [SerializeField] private Transform[] cardContainers;
    [SerializeField] private int handIndex = 0;

    public int amountToTest = 10;
    public List<EquipmentCardShell>[] cards;
    public float cardDistance = 2;
    public float cardYOffset = 3.5f;
    private async void Start()
    {
        cards = new List<EquipmentCardShell>[cardContainers.Length];
        await Task.Delay(10);
        for (var index = 0; index < cardContainers.Length; index++)
        {
            cards[index] = new List<EquipmentCardShell>();
            Transform cardContainer = cardContainers[index];
            for (int i = 0; i < amountToTest; i++)
            {
                EquipmentCardShell cardShell = Instantiate(cardPrefab, cardContainer);
                EquipmentDataContainer dataContainer = new EquipmentDataContainer();
                dataContainer.InsertItem(
                    GameManager.Instance.equipmentRegistry.itemCards[
                        Random.Range(0, GameManager.Instance.equipmentRegistry.itemCards.Length)]);
                dataContainer.GenerateData();
                cardShell.transform.localPosition = new Vector3(i * 0.5f, -3, i);
                cardShell.InsertItem(dataContainer);
                
                cards[index].Add(cardShell);
            }
        }

        cardSlider.maxValue = cards[0].Count-1;
        CheckButtons();
    }
    
    public void Update()
    {
        for (var i = 0; i < cardContainers.Length; i++)
        {
            if (i==handIndex)
            {
                cardContainers[i].localPosition = Vector3.Lerp(cardContainers[i].localPosition, Vector3.zero, Time.deltaTime * 10);
            }
            else
            {
                cardContainers[i].localPosition = Vector3.Lerp(cardContainers[i].localPosition, new Vector3(0, -10, 0), Time.deltaTime * 10);
            }
        }

        if (cards[handIndex] != null)
            for (var i = 0; i < cards[handIndex].Count; i++)
            {
                EquipmentCardShell card = cards[handIndex][i];
                Vector3 localPosition = card.transform.localPosition;
                float x = Math.Abs(cardSlider.value - i);
                localPosition = Vector3.Lerp(localPosition,
                    new Vector3((-cardSlider.value + i) * cardDistance, x > 1.5f ? -4.5f : x < 0.1f ? -2.75f : -3.75f,
                        0), Time.deltaTime * 10);
                localPosition.z = Math.Abs(localPosition.x) * 10;
                // localPosition = Vector3.Lerp(localPosition, new Vector3((-cardSlider.value + i)*cardDistance, (float)(-cardYOffset+Math.Cos(localPosition.x)), Math.Abs(localPosition.x)*10), Time.deltaTime * 10);
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

        if (Math.Abs(cardSlider.value - cards[handIndex].Count) < 1.1f)
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
        //todo
    }
    
    public void SelectHand(int index)
    {
        handIndex = index;
        cardSlider.value = 0;
    }
}