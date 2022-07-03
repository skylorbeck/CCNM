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

    public void SetIndestructible(bool value)
    {
        indestructible = value;
    }
    
    public void InsertItem(ItemCard item)
    {
        itemCore = item;
    }

    public int GetItemCoreIndex()
    {
        GameManager.Instance.equipmentRegistries[(int)itemCore.itemType].CardDictionary.TryGetValue(itemCore.cardTitle, out int index);
        return index;
    }
    
    public int GetAbilityIndex()
    {
        GameManager.Instance.abilityRegistry.Dictionary.TryGetValue(ability.title, out int index);
        return index;
    }

    public void GenerateDataOfLevel(int ofLevel)
    {
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
                case Quality.Curator:
                    qualityMulti = 0.3f;
                    break;
            }

            statValue[i] = itemCore.guaranteeStats.Contains(stats[i])
                ? level * statMulti 
                : Random.Range(level, level * statMulti);
            statValue[i] += (int)Math.Ceiling(qualityMulti * statValue[i]);
        }

        float abilityChance = 0.5f;
        if (quality == Quality.Noteworthy)
            abilityChance = 0.75f;
        if (quality == Quality.Typical)
        {
            abilityChance = 1f;
        }

        if (Random.value < abilityChance)
        {
            ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
        }
    }

    public enum Quality
    {
        Typical, //white 2 stats, 100% ability
        Noteworthy, //green 3 stats, 75% ability
        Remarkable, //blue 4 stats, 50% ability
        Choice, //purple 5 stats, 50% ability
        Signature, //yellow 5 stats, 1 max stats, 50% ability
        Fabled, //orange 5 stats, 2 max stats, 50% ability
        Curator, //red 5 stats, 3 max stats, 50% ability
    }
    public enum Stats
    {
        None,
        Strength,
        Dexterity,
        Speed,
        Skill,
        Vitality,
        Luck,
        Sagacity,
        Intelligence,
        Wisdom,
        Charisma,
        Willpower,
        Faith,
        }
}