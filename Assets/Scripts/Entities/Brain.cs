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
    [field: SerializeField] public AbilityGem[] abilities { get; private set; } = new AbilityGem[0];
    [field: SerializeField] public int credits { get; protected set; }
    [field: SerializeField] public int ego { get; protected set; }
    [field: SerializeField] public int level { get; protected set; } = 0;
    #region persistentData
    [field:SerializeField] public int currentHealth { get; private set; } = 1;
    [field:SerializeField] public Relic[] relics{ get; private set; }

    public void SetCurrentHealth(int health)
    {
        currentHealth = health;
    }
    
    [Obsolete("Use ModifyCurrentHealth inside the Shell instead")]
    public void ModifyCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > GetHealthMax())
        {
            SetCurrentHealth(GetHealthMax());
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
    public int GetUnmodifiedStatValue(EquipmentDataContainer.Stats desiredStat)
    {
        switch (desiredStat)
        {
            default:
            case EquipmentDataContainer.Stats.None:
                return 0;
            case EquipmentDataContainer.Stats.Str:
                return GetStrength();
            case EquipmentDataContainer.Stats.Spd:
                return GetSpeed();
            case EquipmentDataContainer.Stats.Vit:
                return GetVitality();
            case EquipmentDataContainer.Stats.Dex:
                return GetDexterity();
            case EquipmentDataContainer.Stats.Skl:
                return GetSkill();
            case EquipmentDataContainer.Stats.Lck:
                return GetLuck();
            case EquipmentDataContainer.Stats.Cap:
                return GetGrit();
            case EquipmentDataContainer.Stats.Chg:
                return GetResolve();
            case EquipmentDataContainer.Stats.Int:
                return GetIntelligence();
            case EquipmentDataContainer.Stats.Cha:
                return GetCharisma();
            case EquipmentDataContainer.Stats.Wis:
                return GetSagacity();
        }
    }
    #endregion

    #region unModifiedStatGetters

    public virtual int GetUnmodifiedDamage()
    {
        return GetStrength();
    }
    public virtual int GetShieldMaxUnmodified()
    {
        return GetGrit();
    }
    public virtual int GetHealthMaxUnmodified()
    {
        return GetVitality();
    }
    public virtual int GetShieldRateUnmodified()
    {
        return GetResolve();
    }
    public virtual int GetCritChanceUnmodified()
    {
        return GetSpeed();
    }

    public virtual int GetCritDamageUnmodified()
    {
        return GetSkill();
    }

    public virtual int GetDodgeChanceUnmodified()
    {
        return GetDexterity();
    }
    
    public virtual int GetLootLuckUnmodified()
    {
        return GetLuck();
    }

    public virtual int GetEgoBoostUnmodified()
    {
        return GetIntelligence();
    }

    public virtual int GetCreditBoostUnmodified()
    {
        return GetCharisma();
    }
    
    public virtual int GetStatusDamageUnmodified()
    {
        return GetSagacity();
    }
    #endregion
    
    #region computedStatGetters
    public virtual int GetDamage()
    {
        return GetUnmodifiedDamage();
    }

    public virtual int GetShieldMax()
    {
        int shieldMax = GetShieldMaxUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic is RelicTradeHpForShield trade)
            {
                shieldMax += trade.GetShieldBonus(GetHealthMaxUnmodified());
            }
        }

        return shieldMax;
    }

    public virtual int GetShieldRate()
    {
        return GetShieldRateUnmodified();
    }

    public virtual int GetCritChance()
    {
        return GetCritChanceUnmodified();
    }

    public virtual int GetCritDamage()
    {
        return GetCritDamageUnmodified();
    }

    public virtual int GetDodgeChance()
    {
        return GetDodgeChanceUnmodified();
    }
    public virtual int GetHealthMax()
    {
        int healthMax = GetHealthMaxUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic is RelicTradeHpForShield trade)
            {
                healthMax -= trade.GetHpPenalty(GetHealthMaxUnmodified());
            }
        }

        return healthMax;
    } 
  
    public virtual int GetLootLuck()
    {
        return GetLootLuckUnmodified();
    }

    public virtual int GetEgoBoost()
    {
        return GetEgoBoostUnmodified();
    }

    public virtual float GetCreditBoost()
    {
        return 0.01f* GetCreditBoostUnmodified();
    }
    
    public virtual int GetStatusDamage()
    {
        return GetStatusDamageUnmodified();
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
    
    public virtual void SetSagacity(int value)
    {
        sagacity = value;
    }

    public void LevelUpStat(EquipmentDataContainer.Stats controllerStat, int levelDifference)
    {
        switch (controllerStat)
        {
            default:
            case EquipmentDataContainer.Stats.None:
                break;
            case EquipmentDataContainer.Stats.Str:
                SetStrength(GetStrength() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Dex:
                SetDexterity(GetDexterity() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Vit:
                SetVitality(GetVitality() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Spd:
                SetSpeed(GetSpeed() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Skl:
                SetSkill(GetSkill() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Lck:
                SetLuck(GetLuck() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Cap:
                SetGrit(GetGrit() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Chg:
                SetResolve(GetResolve() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Int:
                SetIntelligence(GetIntelligence() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Cha:
                SetCharisma(GetCharisma() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Wis:
                SetSagacity(GetSagacity() + levelDifference);
                break;
        }
        level += levelDifference;
    }

    #endregion
    public AbilityGem GetRandomAbility()
    {
        
        int randomIndex = Random.Range(0,abilities.Length);
        
        return abilities[randomIndex];
    }
    
    public AbilityGem GetAbility(int index)
    {
        return abilities[index];
    }
    
    public void AddAbility(AbilityGem ability)
    {
        if (ability.abilityIndex==-1)
        {
            return;
        }
        List<AbilityGem> newAbilities = new List<AbilityGem>(abilities);
        newAbilities.Add(ability);
        abilities = newAbilities.ToArray();
    }
    
    public void ClearAbilities()
    {
        abilities = new AbilityGem[0];
    }
    
    public void ClearRelics()
    {
        relics = new Relic[0];
    }
    
    public void AddRelic(Relic relic)
    {
        if (relics.Contains(relic))
        {
            return;
        }
        List<Relic> newRelics = new List<Relic>(relics);
        newRelics.Add(relic);
        relics = newRelics.ToArray();
    }
}
