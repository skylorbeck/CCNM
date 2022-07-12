using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RelicRegistry", menuName = "Data/RelicRegistry")]
public class RelicRegistry : ScriptableObject,  ISerializationCallbackReceiver
{
    public Relic[] relics;
    public List<string> keys = new List<string> ();
    public List<int> values = new List<int>();
    public Dictionary<string, int>  RelicDictionary = new Dictionary<string, int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        for (var index = 0; index < relics.Length; index++)
        {
            values.Add(index);
            keys.Add(relics[index].name);
        }
    }

    public void OnAfterDeserialize()
    {
        RelicDictionary = new Dictionary<string, int>();

        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
            RelicDictionary.Add(keys[i], values[i]);
        
    }
    public Relic GetRelic(string relicTitle)
    {
        if (RelicDictionary.ContainsKey(relicTitle))
        {
            return relics[RelicDictionary[relicTitle]];
        }
        else
        {
            Debug.LogError("Relic not found in registry: " + relicTitle);
            return null;
        }
    }
    
    public Relic GetRelic(int relicIndex)
    {
        if (relicIndex < relics.Length)
        {
            return relics[relicIndex];
        }
        else
        {
            Debug.LogError("Relic not found in registry: " + relicIndex);
            return null;
        }
    }
    
    public Relic GetRandomRelic()
    {
        return relics[UnityEngine.Random.Range(0, relics.Length)];
    }
    
    public int GetRelicIndex(string relicTitle)
    {
        if (RelicDictionary.ContainsKey(relicTitle))
        {
            return RelicDictionary[relicTitle];
        }
        else
        {
            Debug.LogError("Relic not found in registry: " + relicTitle);
            return -1;
        }
    }
}
