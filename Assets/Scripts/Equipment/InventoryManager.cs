using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Slider cardSlider;
    [SerializeField] private EquipmentCardShell cardPrefab;
    public int amountToTest = 10;
    public List<EquipmentCardShell> cards = new List<EquipmentCardShell>();
    public float cardDistance = 2;
    public float cardYOffset = 3.5f;
    private void Start()
    {
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
    }
    
    public void Update()
    {
        for (var i = 0; i < cards.Count; i++)
        {
            EquipmentCardShell card = cards[i];
            Vector3 localPosition = card.transform.localPosition;
            localPosition = Vector3.Lerp(localPosition, new Vector3((-cardSlider.value*(cardDistance*cards.Count-cardDistance*1)) + i*cardDistance, (float)(-cardYOffset+Math.Cos(localPosition.x)), Math.Abs(localPosition.x)*10), Time.deltaTime * 50);
            card.transform.localPosition = localPosition;
            card.Selected = localPosition.x<0.5f && localPosition.x>-0.5f;
        }
    }
}