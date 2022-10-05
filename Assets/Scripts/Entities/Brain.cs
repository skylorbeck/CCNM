using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    public virtual int GetStrength()//damage
    {
        return strength;
    }
    public virtual int GetDexterity()//dodge chance
    {
        return dexterity;
    }
    public virtual int GetVitality()//health max
    {
        return vitality;
    }
    public virtual int GetSpeed()//crit chance
    {
        return speed;
    }
    public virtual int GetSkill()//crit damage
    {
        return skill;
    }
    public virtual int GetLuck()//loot luck
    {
        return luck;
    }
    public virtual int GetCap()//shield max
    {
        return grit;
    }
    public virtual int GetCharge()//shield rate (amt per recharge)
    {
        return resolve;
    }
    
    public virtual int GetIntelligence()//ego boost
    {
        return intelligence;
    }
    
    public virtual int GetCharisma()//credit boost
    {
        return charisma;
    }
    
    public virtual int GetWisdom()//status damage
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
                return GetCap();
            case EquipmentDataContainer.Stats.Chg:
                return GetCharge();
            case EquipmentDataContainer.Stats.Int:
                return GetIntelligence();
            case EquipmentDataContainer.Stats.Cha:
                return GetCharisma();
            case EquipmentDataContainer.Stats.Wis:
                return GetWisdom();
        }
    }
    #endregion

    #region unModifiedComputedStatGetters
    public virtual int GetUnmodifiedDamage(int temp =0)
    {
        return (GetStrength()+temp)*5;
    }
    public virtual int GetShieldMaxUnmodified(int temp =0)
    {
        return (GetCap()+temp)*5;
    }
    public virtual int GetHealthMaxUnmodified(int temp =0)
    {
        return (GetVitality()+temp)*5;
    } 
    public virtual int GetHealUnmodified(int temp =0)
    {
        return (GetVitality()+temp);
    }
    public virtual int GetShieldRateUnmodified(int temp =0)
    {
        return (GetCharge()+temp)*5;
    }
    public virtual float GetCritChanceUnmodified(int temp =0)
    {
        return (GetSpeed()+temp)*0.05f;
    }

    public virtual float GetCritDamageUnmodified(int temp =0)
    {
        return (GetSkill()+temp)*0.05f;
    }

    public virtual float GetDodgeChanceUnmodified(int temp =0)
    {
        return (GetDexterity()+temp)*0.01f;
    }
    
    public virtual int GetLootLuckUnmodified(int temp =0)
    {
        return (GetLuck()+temp);
    }

    public virtual int GetEgoBoostUnmodified(int temp =0)
    {
        return (GetIntelligence()+temp);
    }

    public virtual int GetCreditBoostUnmodified(int temp =0)
    {
        return (GetCharisma()+temp);
    }
    
    public virtual int GetStatusDamageUnmodified(int temp =0)
    {
        return (GetWisdom()+temp)*5;
    }
    
    #endregion
    
    #region computedStatGetters
    public virtual int GetDamage()
    {
        int dam = GetUnmodifiedDamage();
        foreach (Relic relic in relics)
        {
            if (relic.modifyDamage)
            {
                dam = Mathf.RoundToInt(dam * relic.modifyDamagePercent);
            }
        }

        return dam;
    }

   
    public virtual int GetShieldMax()
    {
        int shieldMax = GetShieldMaxUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic.modifyShield)
            {
                shieldMax = Mathf.RoundToInt(shieldMax * relic.modifyShieldPercent);
            }
        }

        return shieldMax;
    }

    public virtual int GetShieldRate()
    {
        int shieldRate = GetShieldRateUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic.modifyShieldRegen)
            {
                shieldRate = Mathf.RoundToInt(shieldRate * relic.modifyShieldRegenPercent);
            }
        }
        return shieldRate;
    }

    public virtual float GetCritChance()
    {
        float critChance = GetCritChanceUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic.modifyCritChance)
            {
                critChance = critChance * relic.modifyCritChancePercent;
            }
        }
        return critChance;
    }

    public virtual float GetCritDamage()
    {
        float critDamage = GetCritDamageUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic.modifyCritDamage)
            {
                critDamage = critDamage * relic.modifyCritDamagePercent;
            }
        }
        return critDamage;
    }

    public virtual float GetDodgeChance()
    {
        float dodgeChance = GetDodgeChanceUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic.modifyDodgeChance)
            {
                dodgeChance = dodgeChance * relic.modifyDodgeChancePercent;
            }
        }
        return dodgeChance;
    }
    public virtual int GetHealthMax()
    {
        int healthMax = GetHealthMaxUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic.modifyHealth)
            {
                healthMax =Mathf.RoundToInt(healthMax * relic.modifyHealthPercent);
            }
        }

        return healthMax;
    }
    public virtual int GetHeal()
    {
        return GetHealUnmodified();
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
        int statusDamage = GetStatusDamageUnmodified();
        foreach (Relic relic in relics)
        {
            if (relic.modifyStatusDamage)
            {
                statusDamage =Mathf.RoundToInt(statusDamage * relic.modifyStatusDamagePercent);
            }
        }

        return statusDamage;
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
                SetGrit(GetCap() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Chg:
                SetResolve(GetCharge() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Int:
                SetIntelligence(GetIntelligence() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Cha:
                SetCharisma(GetCharisma() + levelDifference);
                break;
            case EquipmentDataContainer.Stats.Wis:
                SetSagacity(GetWisdom() + levelDifference);
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

    public void AddRelic(Relic newRelic)
    {
        foreach (Relic relic in relics)
        {
            if (relic.name.Equals(newRelic.name))
            {
                return;
            }
        }
        
        List<Relic> newRelics = new List<Relic>(relics);
        newRelics.Add(newRelic);
        relics = newRelics.ToArray();
    }
}
