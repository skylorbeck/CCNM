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
    [field: SerializeField] public AbilitySO[] abilities { get; private set; } = new AbilitySO[0];
    
    public AbilitySO GetRandomAbility()
    {
        int randomIndex = Random.Range(0,abilities.Length);
        return abilities[randomIndex];
    }
}
