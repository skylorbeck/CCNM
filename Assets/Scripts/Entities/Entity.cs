using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : ScriptableObject
{
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public int health { get; private set; } = 100;
    [field: SerializeField] public int maxHealth { get; private set; } = 100;
    [field: SerializeField] public int shield { get; private set; } = 100;
    [field: SerializeField] public int maxShield { get; private set; } = 100;
    [field: SerializeField] public AbilitySO[] abilities { get; private set; } = new AbilitySO[0];
    [field: SerializeField] public StatusEffect[] statusEffects { get; private set; } = new StatusEffect[0];


    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        
    }
    
    public void Damage(int damage)
    {
        if (shield > 0)
        {
            shield -= damage;
            if (shield < 0)
            {
                health += shield;
                shield = 0;
            }
        }
        else
        {
            health -= damage;
        }
    }
    
    public void Heal(int heal)
    {
        health += heal;
    }
    
    public void Shield(int amount)
    {
        shield += amount;
    }
    
    public void AddStatusEffect(StatusEffect statusEffect)
    {
        statusEffects = statusEffects.Append(statusEffect).ToArray();
    }
    
    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffects = statusEffects.Where(se => se != statusEffect).ToArray();
    }
    
    public AbilitySO GetRandomAbility()
    {
        int randomIndex = Random.Range(0,abilities.Length);
        return abilities[randomIndex];
    }
    
}
