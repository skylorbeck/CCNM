using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DeckRegistry", menuName = "Data/DeckRegistry")]
public class DeckRegistry : ScriptableObject,  ISerializationCallbackReceiver
{
    public DeckObject[] deckObjects;
    public List<string> keys = new List<string> ();
    public List<int> values = new List<int>();
    public Dictionary<string, int>  Dictionary = new Dictionary<string, int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        for (var index = 0; index < deckObjects.Length; index++)
        {
            values.Add(index);
            keys.Add(deckObjects[index].name);
        }
    }

    public void OnAfterDeserialize()
    {
        Dictionary = new Dictionary<string, int>();

        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
            Dictionary.Add(keys[i], values[i]);
        
    }
    public DeckObject GetDeck(string cardName)
    {
        if (Dictionary.ContainsKey(cardName))
        {
            return deckObjects[Dictionary[cardName]];
        }
        else
        {
            Debug.LogError("Deck not found in registry: " + cardName);
            return null;
        }
    }

    public DeckObject GetRandomDeck()
    {
        return deckObjects[UnityEngine.Random.Range(0, deckObjects.Length)];
    }
    
    public int GetDeckIndex(string cardName)
    {
        if (Dictionary.ContainsKey(cardName))
        {
            return Dictionary[cardName];
        }
        else
        {
            Debug.LogError("Deck not found in registry: " + cardName);
            return -1;
        }
    }
    
    public DeckObject GetDeck(int index)
    {
        return deckObjects[index];
    }
}
