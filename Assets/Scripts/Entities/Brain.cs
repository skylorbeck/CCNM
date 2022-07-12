using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Brain : ScriptableObject
{
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public AbilityObject[] abilities { get; private set; } = new AbilityObject[0];
    
    #region persistentData
    [field:SerializeField] public int currentHealth { get; private set; } = 1;
    [field:SerializeField] public Relic[] relics{ get; private set; }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }
    
    public void ModifyCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > GetMaxHealth())
        {
            SetCurrentHealth((int)GetMaxHealth());
        }
    }
    #endregion
    
    #region stats
    [SerializeField] private int strength;
    [SerializeField] private int dexterity;
    [SerializeField] private int vitality;
    [SerializeField] private int speed;
    [SerializeField] private int skill;
    [SerializeField] private int luck;
    [SerializeField] private int grit;
    [SerializeField] private int resolve;
    [SerializeField] private int intelligence;
    [SerializeField] private int charisma;
    [SerializeField] private int sagacity;
    #endregion

    #region statGetters
    public virtual int GetStrength()
    {
        return strength;
    }
    public virtual int GetDexterity()
    {
        return dexterity;
    }
    public virtual int GetVitality()
    {
        return vitality;
    }
    public virtual int GetSpeed()
    {
        return speed;
    }
    public virtual int GetSkill()
    {
        return skill;
    }
    public virtual int GetLuck()
    {
        return luck;
    }
    public virtual int GetGrit()
    {
        return grit;
    }
    public virtual int GetResolve()
    {
        return resolve;
    }
    
    public virtual int GetIntelligence()
    {
        return intelligence;
    }
    
    public virtual int GetCharisma()
    {
        return charisma;
    }
    
    public virtual int GetSagacity()
    {
        return sagacity;
    }
    
    #endregion

    #region computedStatGetters
    public virtual float GetDamage()
    {
        return GetStrength();
    }

    public virtual float GetShieldMax()
    {
        foreach (Relic relic in relics)
        {
            if (relic is RelicTradeHpForShield trade)
            {
                trade.AdjustShield((int)GetMaxHealth());
            }
        }

        return GetGrit();
    }

    public virtual float GetShieldRate()
    {
        return GetResolve();
    }

    public virtual float GetCritChance()
    {
        return GetSpeed();
    }

    public virtual float GetCritDamage()
    {
        return GetSkill();
    }

    public virtual float GetDodgeChance()
    {
        return GetDexterity();
    }
    public virtual float GetMaxHealth()
    {
        return GetVitality();
    }
    public virtual float GetLootLuck()
    {
        return GetLuck();
    }

    public virtual float GetEgoBoost()
    {
        return GetIntelligence();
    }

    public virtual float GetCreditBoost()
    {
        return GetCharisma();
    }
    
    public virtual float GetStatusDamage()
    {
        return GetSagacity();
    }
    #endregion
    
    #region statSetters
    public virtual void SetStrength(int value)
    {
        strength = value;
    }
    public virtual void SetDexterity(int value)
    {
        dexterity = value;
    }
    public virtual void SetVitality(int value)
    {
        vitality = value;
    }
    public virtual void SetSpeed(int value)
    {
        speed = value;
    }
    public virtual void SetSkill(int value)
    {
        skill = value;
    }
    public virtual void SetLuck(int value)
    {
        luck = value;
    }
    public virtual void SetGrit(int value)
    {
        grit = value;
    }
    public virtual void SetResolve(int value)
    {
        resolve = value;
    }
    public virtual void SetIntelligence(int value)
    {
        intelligence = value;
    }
    public virtual void SetCharisma(int value)
    {
        charisma = value;
    }
    #endregion
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
