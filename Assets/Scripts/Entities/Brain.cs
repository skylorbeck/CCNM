using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brain : ScriptableObject
{
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public AbilityObject[] abilities { get; private set; } = new AbilityObject[0];
    

    
    
    #region persistentData
    [field:SerializeField] public float currentHealth { get; private set; } = 1;
    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
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
    #endregion

    #region statGetters
    public virtual float GetStrength()
    {
        return strength;
    }
    public virtual float GetDexterity()
    {
        return dexterity;
    }
    public virtual float GetVitality()
    {
        return vitality;
    }
    public virtual float GetSpeed()
    {
        return speed;
    }
    public virtual float GetSkill()
    {
        return skill;
    }
    public virtual float GetLuck()
    {
        return luck;
    }
    public virtual float GetGrit()
    {
        return grit;
    }
    public virtual float GetResolve()
    {
        return resolve;
    }
    
    public virtual float GetIntelligence()
    {
        return intelligence;
    }
    
    public virtual float GetCharisma()
    {
        return charisma;
    }
    #endregion

    #region computedStatGetters
    public virtual float GetDamage()
    {
        return GetStrength();
    }

    public virtual float GetShieldMax()
    {
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
