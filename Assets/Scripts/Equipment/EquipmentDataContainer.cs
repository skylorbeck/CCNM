using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EquipmentDataContainer
{
    [field:SerializeField] public ItemCard itemCore { get; private set; }
    [field:SerializeField] public AbilityObject ability{ get; protected set; }
    [field:SerializeField] public Quality quality { get; private set; }
    [field:SerializeField] public int level { get; private set; }
    [field:SerializeField] public Stats[] stats { get; private set; }
    [field:SerializeField] public int[] statValue { get; private set; }
    [field:SerializeField] public bool indestructible { get; private set; }

    public Guid guid { get; private set; }
    public void SetIndestructible(bool value)
    {
        indestructible = value;
    }
    
    public void InsertItem(ItemCard item)
    {
        itemCore = item;
    }
    
    public void InsertAbility(AbilityObject ability)
    {
        this.ability = ability;
    }
    
    public void SetStatValue(int[] value)
    {
        statValue = value;
    }
    public void SetStats(Stats[] stats)
    {
        this.stats = stats;
    }

    public int GetItemCoreIndex()
    {
        return GameManager.Instance.equipmentRegistries[(int)itemCore.itemType].GetCardIndex(itemCore.name);
    }
    
    public int GetAbilityIndex()
    {
        return GameManager.Instance.abilityRegistry.GetAbilityIndex(ability.title);
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
        allStats.Remove(Stats.None);
        
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
            int statMulti = 1;
            switch (stats[i])
            {
                case Stats.Strength:
                    statMulti = 5;
                    break;
                case Stats.Dexterity:
                    statMulti = 5;
                    break;
                case Stats.Speed:
                    statMulti = 5;
                    break;
                case Stats.Skill:
                    statMulti = 5;
                    break;
                case Stats.Vitality:
                    statMulti = 10;
                    break;
                case Stats.Luck:
                    statMulti = 2;
                    break;
                // case Stats.Willpower:
                //     statMulti = 5;
                //     break;
                case Stats.Grit:
                    statMulti = 5;
                    break;
                case Stats.Resolve:
                    statMulti = 5;
                    break;
            }

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

        /*float abilityChance = 0.5f;
        if (quality == Quality.Noteworthy)
            abilityChance = 0.75f;
        if (quality == Quality.Typical)
        {
            abilityChance = 1f;
        }

        if (Random.value < abilityChance)
        {
            ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
        }*/
        ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
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
    public enum Stats
    {
        None,
        Strength,//damage
        Dexterity,//dodge chance
        Vitality,//health
        Speed,//crit chance
        Skill,//crit damage
        Luck,//loot quality
        Grit,//shield capacity
        Resolve,//shield recharge rate
        Intelligence,//ego boost
        Charisma,//credit boost
        Sagacity,//status damage
        // Wisdom,
        // Willpower,
    }

    public EquipmentDataContainer()
    {
    }
    public EquipmentDataContainer(SavableDataContainer savableDataContainer)
    {
        guid = savableDataContainer.guid;
        itemCore = GameManager.Instance.equipmentRegistries[savableDataContainer.equipmentRegistryIndex].GetCard(savableDataContainer.itemCoreIndex);
        ability = GameManager.Instance.abilityRegistry.GetAbility(savableDataContainer.abilityIndex);
        quality = savableDataContainer.quality;
        level = savableDataContainer.level;
        stats = savableDataContainer.stats;
        statValue = savableDataContainer.statValue;
        indestructible = savableDataContainer.indestructible;
    }
}

[Serializable]
public class SavableDataContainer
{
    public Guid guid;
    public int equipmentRegistryIndex;
    public int itemCoreIndex;
    public int abilityIndex;
    public int level;
    public int[] statValue;
    public bool indestructible;
    public EquipmentDataContainer.Quality quality;
    public EquipmentDataContainer.Stats[] stats;
    public SavableDataContainer(EquipmentDataContainer data)
    {
        guid = data.guid;
        equipmentRegistryIndex = (int)data.itemCore.itemType;
        itemCoreIndex = data.GetItemCoreIndex();
        abilityIndex = data.GetAbilityIndex();
        level = data.level;
        statValue = data.statValue;
        indestructible = data.indestructible;
        quality = data.quality;
        stats = data.stats;
    }
}