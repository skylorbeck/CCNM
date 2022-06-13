using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EquipmentRegistry", menuName = "Data/EquipmentRegistry")]
public class EquipmentRegistry : ScriptableObject,  ISerializationCallbackReceiver
{
    public ItemCard[] itemCards;
    public List<string> keys = new List<string> ();
    public List<int> values = new List<int>();
    public Dictionary<string, int>  CardDictionary = new Dictionary<string, int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        for (var index = 0; index < itemCards.Length; index++)
        {
            values.Add(index);
            keys.Add(itemCards[index].name);
        }
    }

    public void OnAfterDeserialize()
    {
        CardDictionary = new Dictionary<string, int>();

        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
            CardDictionary.Add(keys[i], values[i]);
        
    }
    public ItemCard GetCard(string cardTitle)
    {
        if (CardDictionary.ContainsKey(cardTitle))
        {
            return itemCards[CardDictionary[cardTitle]];
        }
        else
        {
            Debug.LogError("Card not found in registry: " + cardTitle);
            return null;
        }
    }
    public ItemCard GetRandomCard()
    {
        return itemCards[UnityEngine.Random.Range(0, itemCards.Length)];
    }
}
