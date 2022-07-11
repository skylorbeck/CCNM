using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AbilityRegistry", menuName = "Data/AbilityRegistry")]
public class AbilityRegistry : ScriptableObject,  ISerializationCallbackReceiver
{
    public AbilityObject[] abilityObjects;
    public List<string> keys = new List<string> ();
    public List<int> values = new List<int>();
    public Dictionary<string, int>  Dictionary = new Dictionary<string, int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        for (var index = 0; index < abilityObjects.Length; index++)
        {
            values.Add(index);
            keys.Add(abilityObjects[index].title);
        }
    }

    public void OnAfterDeserialize()
    {
        Dictionary = new Dictionary<string, int>();

        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
            Dictionary.Add(keys[i], values[i]);
        
    }
    public AbilityObject GetAbility(string cardTitle)
    {
        if (Dictionary.ContainsKey(cardTitle))
        {
            return abilityObjects[Dictionary[cardTitle]];
        }
        else
        {
            Debug.LogError("Ability not found in registry: " + cardTitle);
            return null;
        }
    }

    public AbilityObject GetRandomAbility()
    {
        return abilityObjects[UnityEngine.Random.Range(0, abilityObjects.Length)];
    }
    
    public int GetAbilityIndex(string cardTitle)
    {
        if (Dictionary.ContainsKey(cardTitle))
        {
            return Dictionary[cardTitle];
        }
        else
        {
            Debug.LogError("Ability not found in registry: " + cardTitle);
            return -1;
        }
    }
    
    public AbilityObject GetAbility(int index)
    {
        return abilityObjects[index];
    }
}
