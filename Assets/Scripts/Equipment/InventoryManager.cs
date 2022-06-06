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
    public int amountToTest = 10;
    public List<EquipmentCardShell> cards = new List<EquipmentCardShell>();
    public float cardDistance = 2;
    public float cardYOffset = 3.5f;
    private async void Start()
    {
        await Task.Delay(1000);
        for (int i = 0; i < amountToTest; i++)
        {
            EquipmentCardShell cardShell = Instantiate(cardPrefab, transform);
            EquipmentDataContainer dataContainer = new EquipmentDataContainer();
            dataContainer.InsertItem(GameManager.Instance.equipmentRegistry.itemCards[Random.Range(0, GameManager.Instance.equipmentRegistry.itemCards.Length)]);
            dataContainer.GenerateData();
            cardShell.transform.localPosition = new Vector3(i *0.5f, -3, i);
            cardShell.InsertItem(dataContainer);
            cards.Add(cardShell);
        }

        cardSlider.maxValue = cards.Count-1;
        CheckButtons();
    }
    
    public void Update()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            EquipmentCardShell card = cards[i];
            Vector3 localPosition = card.transform.localPosition;
            float x = Math.Abs(cardSlider.value - i);
            localPosition = Vector3.Lerp(localPosition, new Vector3((-cardSlider.value + i)*cardDistance, x >1.5f? -4.5f: x < 0.1f?-2.75f:-3.75f, 0), Time.deltaTime * 10);
            localPosition.z = Math.Abs(localPosition.x) * 10;
            // localPosition = Vector3.Lerp(localPosition, new Vector3((-cardSlider.value + i)*cardDistance, (float)(-cardYOffset+Math.Cos(localPosition.x)), Math.Abs(localPosition.x)*10), Time.deltaTime * 10);
            card.transform.localPosition = localPosition;
            card.Selected = localPosition.x<0.25f && localPosition.x>-0.25f;
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
        //todo
    }
}