using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brain : ScriptableObject
{
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int maxHealth { get; private set; } = 100;
    [field: SerializeField] public int startingShield { get; private set; } = 100;
    [field: SerializeField] public AbilityObject[] abilities { get; private set; } = new AbilityObject[0];
    
    [field: SerializeField] public int baseDamage { get; private set; } = 1;
    [field: SerializeField] public int baseShield { get; private set; } = 1;
    [field: SerializeField] public int baseHeal { get; private set; } = 1;
    
    public AbilityObject GetRandomAbility()
    {
        int randomIndex = Random.Range(0,abilities.Length);
        return abilities[randomIndex];
    }
    
    public AbilityObject GetAbility(int index)
    {
        return abilities[index];
    }
    
    public void AddAbility(AbilityObject ability)
    {
        List<AbilityObject> newAbilities = new List<AbilityObject>(abilities);
        newAbilities.Add(ability);
        abilities = newAbilities.ToArray();
    }
    
    public void ClearAbilities()
    {
        abilities = new AbilityObject[0];
    }
}
