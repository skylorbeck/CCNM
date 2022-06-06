using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EquipmentDataContainer
{
    [field: SerializeField] public ItemCard itemCore { get; private set; }
    [field:SerializeField] public AbilityObject ability{ get; protected set; }
    [field: SerializeField] public Quality quality { get; private set; }

    [field: Header("Stats")]
    [field: SerializeField]
    public Stats[] stats { get; private set; }

    [field: SerializeField] public int[] statValue { get; private set; }

    public void InsertItem(ItemCard item)
    {
        itemCore = item;
    }

    public int GetItemCoreIndex()
    {
        GameManager.Instance.equipmentRegistry.CardDictionary.TryGetValue(itemCore.cardTitle, out int index);
        return index;
    }
    
    public int GetAbilityIndex()
    {
        GameManager.Instance.abilityRegistry.Dictionary.TryGetValue(ability.title, out int index);
        return index;
    }

    public void GenerateData() //todo replace this entire thing, it's disgusting
    {
        quality = (Quality)Random.Range(0, Enum.GetNames(typeof(Quality)).Length);
        stats = new Stats[5];
        statValue = new int[5];
        switch (quality)
        {
            case Quality.Curator:
                for (var index = 0; index < stats.Length; index++)
                {
                    stats[index] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[index] = 100;
                }

                ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                break;
            case Quality.Fabled:
                for (var index = 0; index < stats.Length; index++)
                {
                    stats[index] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[index] = Random.Range(0, 100);
                }

                ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                break;
            case Quality.Signature:
                for (var index = 0; index < stats.Length - 1; index++)
                {
                    stats[index] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[index] = Random.Range(0, 100);
                }

                if (Random.Range(0f, 1f) > 0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                }
                else
                {
                    stats[stats.Length - 1] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[stats.Length - 1] = Random.Range(0, 100);
                }
                break;
            case Quality.Choice:
                for (var index = 0; index < stats.Length - 2; index++)
                {
                    stats[index] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[index] = Random.Range(0, 100);
                }

                if (Random.Range(0f, 1f) > 0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                }
                else
                {
                    stats[stats.Length - 2] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[stats.Length - 2] = Random.Range(0, 100);
                }
                break;
            case Quality.Remarkable:
                for (var index = 0; index < stats.Length - 3; index++)
                {
                    stats[index] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[index] = Random.Range(0, 100);
                }

                if (Random.Range(0f, 1f) > 0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                }
                else
                {
                    stats[stats.Length - 3] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[stats.Length - 3] = Random.Range(0, 100);
                }

                break;
            case Quality.Noteworthy:
                for (var index = 0; index < stats.Length - 4; index++)
                {
                    stats[index] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[index] = Random.Range(0, 100);
                }

                if (Random.Range(0f, 1f) > 0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                }
                else
                {
                    stats[stats.Length - 4] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    statValue[stats.Length - 4] = Random.Range(0, 100);
                }


                break;
            case Quality.Typical:
                stats[0] = (Stats)Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                statValue[0] = Random.Range(0, 100);
                break;
        }
    }

    public enum Quality
    {
        Typical, //white 1 stat or ability
        Noteworthy, //green 1 stat, 1 stat or ability
        Remarkable, //blue 2 stats, 1 stat or ability
        Choice, //purple 3 stats, 1 stat or ability
        Signature, //yellow 4 stats, 1 stat or ability
        Fabled, //orange 5 stats, 1 ability
        Curator, //red 5 stats, 1 ability, max stats
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