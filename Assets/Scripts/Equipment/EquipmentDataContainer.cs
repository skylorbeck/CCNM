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
        // pickedStats.Sort((x, y) => y.CompareTo(x));
        stats = pickedStats.ToArray();
        for (var i = 0; i < stats.Length; i++)
        {
            switch (stats[i])
            {
                case Stats.None:
                    statValue[i] = 0;
                    break;
                case Stats.Damage:
                    statValue[i] = Random.Range(1, level + 1);
                    break;
                case Stats.Shield:
                    statValue[i] = Random.Range(1, 10) * level;
                    break;
                case Stats.CriticalChance:
                    statValue[i] = Random.Range(level, 101);
                    break;
                case Stats.CriticalDamage:
                    statValue[i] = Random.Range(level*10, 101*level);
                    break;
                case Stats.Health:
                    statValue[i] = 100 + Random.Range(1 * level, 101 * level);
                    break;
            }
        }
        int[] statIndexes = new int[5]{0,1,2,3,4};
        statIndexes = statIndexes.OrderBy(x => Random.value).ToArray();
        for (int i = 0; i < quality - Quality.Choice; i++)
        {
            // statValue[statIndexes[i]] = -420;
            switch (stats[statIndexes[i]])
            {
                case Stats.Damage:
                    statValue[0] =level + 1;
                    break;
                case Stats.Shield:
                    statValue[i] = 10* level;
                    break;
                case Stats.CriticalChance:
                    statValue[i] = 100;
                    break;
                case Stats.CriticalDamage:
                    statValue[i] = 101;
                    break;
                case Stats.Health:
                    statValue[i] = 100+101* level;
                    break;
            }
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
        Damage,
        Shield,
        CriticalChance,
        CriticalDamage,
        Health,
    }
}

