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
            return relics[relicIndex];
    }

    public Relic GetRandomRelicWithoutDuplicates(Relic[] ownedRelics)
    {
        List<Relic> availableRelics = new List<Relic>();
       
        //compare the name of each owned relic to the list of all relics and remove any matches
        for (int i = 0; i < relics.Length; i++)
        {
            bool isOwned = false;
            for (int j = 0; j < ownedRelics.Length; j++)
            {
                if (relics[i].name == ownedRelics[j].name)
                {
                    isOwned = true;
                }
            }
            if (!isOwned)
            {
                availableRelics.Add(relics[i]);
            }
        }
        
        if (availableRelics.Count > 0)
        {
            return availableRelics[UnityEngine.Random.Range(0, availableRelics.Count)];
        }
        else
        {
            Debug.LogError("No available relics");
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
