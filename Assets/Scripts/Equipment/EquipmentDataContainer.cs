﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EquipmentDataContainer
{
    [field:SerializeField] public ItemCard itemCore { get; private set; }
    [field:SerializeField] public int gemSlots { get; private set; }//1-3. If less than 3, the rest are hidden and locked and empty.
    [field:SerializeField] public bool[] lockedSlots { get; private set; }
    // [field:SerializeField] public AbilityObject[] abilities { get; protected set; }
    [field:SerializeField] public AbilityGem[] abilities { get; protected set; }
    [field:SerializeField] public Quality quality { get; private set; }
    [field:SerializeField] public int level { get; private set; }
    [field:SerializeField] public Stats[] stats { get; private set; }
    [field:SerializeField] public int[] statValue { get; private set; }
    [field:SerializeField] public bool indestructible { get; private set; }

    public Guid guid { get; private set; }

    public EquipmentDataContainer(EquipmentDataContainer old)//clone
    {
        itemCore = old.itemCore;
        gemSlots = old.gemSlots;
        lockedSlots = old.lockedSlots;
        abilities = old.abilities;
        quality = old.quality;
        level = old.level;
        stats = old.stats;
        statValue = old.statValue;
        indestructible = old.indestructible;
        guid = old.guid;
        // guid = Guid.NewGuid();//do we want a new GUID? This would allow for the same item to be duplicated.
    }
    
    public void SetIndestructible(bool value)
    {
        indestructible = value;
    }
    
    public void InsertItem(ItemCard item)
    {
        itemCore = item;
    }
    
    public bool InsertAbility([AllowNull]AbilityGem ability,int slot)
    {
        if (slot >= gemSlots || slot < 0)
        {
            Debug.LogWarning("Slot is out of range");
            return false;
        }

        if (lockedSlots[slot])
        {
            Debug.LogWarning("Slot is locked");
            return false;
        }

        if (abilities[slot] != null && abilities[slot].abilityIndex!=-1)
        {
            Debug.LogWarning("Slot is not empty");
            return false;
        }

        if (ability!=null)
        {
            abilities[slot] =new AbilityGem(ability);
            abilities[slot].SetAmountOwned(1);
            lockedSlots[slot] = true;
        } else
        {
            abilities[slot] = new AbilityGem(-1);
            lockedSlots[slot] = false;
        }

        return true;
    }

    public bool RemoveAbility(int slot)
    { 
        if (slot >= gemSlots || slot < 0)
        {
            Debug.LogWarning("Slot is out of range");
            return false;
        }
        abilities[slot] = new AbilityGem(-1);
        lockedSlots[slot] = false;
        return true;
    }

    public void SetStatValue(int[] value)
    {
        statValue = value;
    }
    public void SetStats(Stats[] stats)
    {
        this.stats = stats;
    }
    public void SetGemSlots(int value)
    {
        gemSlots = value;
    }
    
    public void SetLockedSlots(bool[] value)
    {
        lockedSlots = value;
    }
    
    public void SetAbilities(AbilityGem[] value)
    {
        abilities = value;
    }

    public int GetItemCoreIndex()
    {
        return GameManager.Instance.equipmentRegistries[(int)itemCore.itemType].GetCardIndex(itemCore.name);
    }
    
    public int GetAbilityIndex(int abilityIndex)
    {
        return abilities[abilityIndex].abilityIndex;
    }

    public void GenerateDataOfLevel(int ofLevel)
    {
        guid = Guid.NewGuid();
        if (ofLevel < 1)
        {
            ofLevel = 1;
        }
        quality = GameManager.Instance.lootManager.GetRandomQuality();

        level = ofLevel;
        stats = new Stats[5];
        statValue = new int[5];
        List<Stats> allStats = new List<Stats>();
        List<Stats> pickedStats = new List<Stats>();
        allStats.AddRange(Enum.GetValues(typeof(Stats)).Cast<Stats>());
        allStats.Remove(Stats.None);//todo remove the bottom three?
        allStats.Remove(Stats.Int);
        allStats.Remove(Stats.Cha);
        allStats.Remove(Stats.Lck);
        
        for (int i = 0; i < itemCore.guaranteeStats.Length; i++)
        {
            pickedStats.Add(itemCore.guaranteeStats[i]);
            allStats.Remove(pickedStats[i]);
        }
        
       for (int i = pickedStats.Count; i < 5; i++)
        {
            if ((int)quality >= i - 1)
            {
                pickedStats.Add(allStats[Random.Range(0, allStats.Count)]);
                allStats.Remove(pickedStats[i]);
            }
            else
            {
                pickedStats.Add(Stats.None);
            }
        }
        stats = pickedStats.ToArray();
        for (var i = 0; i < stats.Length; i++)
        {
            //we used to multiply every stat by their own balance numbers, but that hurts my brain and makes it hard to balance.
            //Now you get a stat value between your level and 2x your level.
            int statMulti = 2;
            /*switch (stats[i])
            {
                case Stats.Str:
                    statMulti = 5;
                    break;
                case Stats.Spd:
                    statMulti = 5;
                    break;
                case Stats.Dex:
                    statMulti = 5;
                    break;
                case Stats.Skl:
                    statMulti = 5;
                    break;
                case Stats.Vit:
                    statMulti = 10;
                    break;
                case Stats.Lck:
                    statMulti = 2;
                    break;
                // case Stats.Willpower:
                //     statMulti = 5;
                //     break;
                case Stats.Cap:
                    statMulti = 5;
                    break;
                case Stats.Chg:
                    statMulti = 5;
                    break;
            }*/

            float qualityMulti = 0;
            switch (quality)
            {
                case Quality.Signature:
                    qualityMulti = 0.1f;
                    break;
                case Quality.Fabled:
                    qualityMulti = 0.2f;
                    break;
                case Quality.Exalted:
                    qualityMulti = 0.3f;
                    break;
            }

            statValue[i] = itemCore.guaranteeStats.Contains(stats[i])
                ? level * statMulti 
                : Random.Range(level, level * statMulti);
            statValue[i] += (int)Math.Ceiling(qualityMulti * statValue[i]);
        }

        lockedSlots = new bool[3];
        /*for (int i = 0; i < lockedSlots.Length; i++)
        {
            lockedSlots[i] = true;
        }*/
        abilities = new AbilityGem[lockedSlots.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = new AbilityGem(-1);
        }
        gemSlots = 0;
        
        switch (quality)
        {
            default:
            case Quality.Typical:
                gemSlots = 1;
                break;
            case Quality.Noteworthy:
                gemSlots = 1+Random.Range(0,2);
                //we used to randomize the locked slots here, but it's not necessary anymore because slots are locked when gems are inserted.
                
                /*for (int i = 0; i < gemSlots; i++)
                {
                    lockedSlots[i] = Random.Range(0,3) != 0;//66% chance of being locked
                }*/
                break;
            case Quality.Remarkable:
                gemSlots = 1+Random.Range(0,3);
                /*for (int i = 0; i < gemSlots; i++)
                {
                    lockedSlots[i] = Random.Range(0,2) == 0;//50% chance of being locked
                }*/
                break;
            case Quality.Choice:
                gemSlots = 1+Random.Range(0,3);
                /*for (int i = 0; i < gemSlots; i++)
                {
                    lockedSlots[i] = Random.Range(0,3) == 0;//33% chance of being locked
                }*/
                break;
            case Quality.Signature:
                gemSlots = 2+Random.Range(0,2);
                /*for (int i = 0; i < gemSlots; i++)
                {
                    lockedSlots[i] = Random.Range(0,4) == 0;//25% chance of being locked
                }*/
                break;
            case Quality.Fabled:
                gemSlots = 2+Random.Range(0,2);
                /*for (int i = 0; i < gemSlots; i++)
                {
                    lockedSlots[i] = Random.Range(0,4) == 0;//25% chance of being locked
                }*/
                break;
            case Quality.Exalted:
                gemSlots = 3;
                /*for (int i = 0; i < lockedSlots.Length; i++)
                {
                    lockedSlots[i] = false;

                }//all slots are unlocked*/
                break;
        }

        abilities[0] = new AbilityGem(itemCore.GetRandomAbility(), 0);
        abilities[0].SetAmountOwned(1);
        lockedSlots[0] = true;
        for (int i = 1; i < gemSlots; i++)
        {
            // abilities[i] = Random.Range(0, 3) != 0 ? null: GameManager.Instance.abilityRegistry.GetRandomAbility();
            if (Random.Range(0, 3) != 0)
            {
                abilities[i] = new AbilityGem(-1);//redundant but I refuse to remove it
                lockedSlots[i] = false;
            }
            else
            {
                abilities[i] = new AbilityGem(itemCore.GetRandomAbility(), 0);
                abilities[i].SetAmountOwned(1);
                lockedSlots[i] = true;
            }
        }
    }
    public AbilityGem[] GetAbilities()
    {
        return abilities;
    }
    public AbilityGem GetAbility(int index)
    {
        if (abilities != null && abilities.Length > index)
        {
            return abilities[index];
        }
        return null;
    }
    
    public enum Quality
    {
        Typical, //white 2 stats
        Noteworthy, //green 3 stats
        Remarkable, //blue 4 stats
        Choice, //purple 5 stats
        Signature, //yellow 5 stats, 1 max stats
        Fabled, //orange 5 stats, 2 max stats
        Exalted, //red 5 stats, 3 max stats
    }

    public enum SlotType
    {
        Offense,
        Defense,
        Utility,
    }
    public enum Stats
    {
        None,
        //combat
        Str,//damage
        Wis,//status damage
        Spd,//crit chance
        Skl,//crit damage
        Vit,//health
        Dex,//dodge chance
        Cap,//shield capacity
        Chg,//shield recharge rate

        //meta
        Int,//ego boost
        Cha,//credit boost
        Lck,//loot quality

        // Willpower,
    }

    public static String AttributeName(Stats stat)
    {
        string translatedStat = "";
        switch (stat)
        {
            case Stats.None:
                break;
            case Stats.Str:
                translatedStat = "Attack";
                break;
            case Stats.Dex:
                translatedStat = "Dodge";
                break;
            case Stats.Vit:
                translatedStat = "Health";
                break;
            case Stats.Spd:
                translatedStat = "Crit Chance";
                break;
            case Stats.Skl:
                translatedStat = "Crit Damage";
                break;
            case Stats.Cap:
                translatedStat = "Shield";
                break;
            case Stats.Chg:
                translatedStat = "Recharge";
                break;
            case Stats.Wis:
                translatedStat = "Status Damage";
                break;
            case Stats.Int:
                translatedStat = "Ego Boost";
                break;
            case Stats.Cha:
                translatedStat = "Credit Boost";
                break;
            case Stats.Lck:
                translatedStat = "Luck";
                break;
        }

        return translatedStat;
    }

    public EquipmentDataContainer()
    {
    }
    public EquipmentDataContainer(SavableDataContainer savableDataContainer)
    {
        guid = savableDataContainer.guid;
        itemCore = GameManager.Instance.equipmentRegistries[savableDataContainer.equipmentRegistryIndex].GetCard(savableDataContainer.itemCoreIndex);
        gemSlots = savableDataContainer.gemSlots;
        if (gemSlots<=0)
        {
            gemSlots = 1;
        }
        lockedSlots = savableDataContainer.lockedSlots;
        if (lockedSlots == null || lockedSlots.Length <= 2)
        {
            lockedSlots = new bool[3];
            for (var i = 0; i < lockedSlots.Length; i++)
            {
                lockedSlots[i] = true;
            }
        }

        abilities = savableDataContainer.abilityGems;
        quality = savableDataContainer.quality;
        level = savableDataContainer.level;
        stats = savableDataContainer.stats;
        statValue = savableDataContainer.statValue;
        indestructible = savableDataContainer.indestructible;
    }

    public int GetStatValue(Stats stat)
    {
        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] == stat)
            {
                return statValue[i];
            }
        }

        return 0;
    }
}

[Serializable]
public class SavableDataContainer
{
    public Guid guid;
    public int equipmentRegistryIndex;
    public int itemCoreIndex;
    public AbilityGem[] abilityGems;
    public int level;
    public int[] statValue;
    public bool indestructible;

    [OptionalField]
    public int gemSlots;
    [OptionalField]
    public bool[] lockedSlots;
    
    public EquipmentDataContainer.Quality quality;
    public EquipmentDataContainer.Stats[] stats;
    public SavableDataContainer(EquipmentDataContainer data)
    {
        guid = data.guid;
        equipmentRegistryIndex = (int)data.itemCore.itemType;
        itemCoreIndex = data.GetItemCoreIndex();
        abilityGems = data.GetAbilities();
        level = data.level;
        statValue = data.statValue;
        indestructible = data.indestructible;
        quality = data.quality;
        stats = data.stats;
        lockedSlots = data.lockedSlots;
        gemSlots = data.gemSlots;
    }
}