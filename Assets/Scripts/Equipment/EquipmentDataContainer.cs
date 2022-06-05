using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EquipmentDataContainer
{
    [field: SerializeField] public ItemCard itemCore { get; private set; }
    [field:SerializeField] public AbilityObject ability{ get; protected set; }
    [field: SerializeField] public Quality quality { get; private set; }
 
    [field: Header("Stats")]
    [field: SerializeField] public Stats stat1 { get; private set; } = Stats.None;
    [field: SerializeField] public int stat1Value { get; private set; } = 0;
    [field: SerializeField] public Stats stat2 { get; private set; } = Stats.None;
    [field: SerializeField] public int stat2Value { get; private set; } = 0;
    [field: SerializeField] public Stats stat3 { get; private set; } = Stats.None;
    [field: SerializeField] public int stat3Value { get; private set; } = 0;
    [field: SerializeField] public Stats stat4 { get; private set; } = Stats.None;
    [field: SerializeField] public int stat4Value { get; private set; } = 0;
    [field: SerializeField] public Stats stat5 { get; private set; } = Stats.None;
    [field: SerializeField] public int stat5Value { get; private set; } = 0;
  
    
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

    public void GenerateData()//todo replace this entire thing, it's disgusting
    {
        quality = (Quality) Random.Range(0, Enum.GetNames(typeof(Quality)).Length);
        switch (quality)
        {
            case Quality.Curator:
                stat5 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat5Value = 100;
                stat4 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat4Value = 100;
                stat3 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat3Value = 100;
                stat2 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat2Value = 100;
                stat1 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat1Value = 100;
                ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                break;
            case Quality.Fabled:
                stat5 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat5Value = Random.Range(0, 100);
                stat4 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat4Value = Random.Range(0, 100);
                stat3 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat3Value = Random.Range(0, 100);
                stat2 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat2Value = Random.Range(0, 100);
                stat1 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat1Value = Random.Range(0, 100);
                ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                break;
            case Quality.Signature:
                stat5 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat5Value = Random.Range(0, 100);
                stat4 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat4Value = Random.Range(0, 100);
                stat3 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat3Value = Random.Range(0, 100);
                stat2 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat2Value = Random.Range(0, 100);
                if (Random.Range(0f,1f)>0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                    stat1 = Stats.None;
                    stat1Value = 0;
                }
                else
                {
                    stat1 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    stat1Value = Random.Range(0, 100);
                }
                break;
            case Quality.Choice:
                stat4 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat4Value = Random.Range(0, 100);
                stat3 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat3Value = Random.Range(0, 100);
                stat2 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat2Value = Random.Range(0, 100);
                if (Random.Range(0f,1f)>0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                    stat1 = Stats.None;
                    stat1Value = 0;
                }
                else
                {
                    stat1 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    stat1Value = Random.Range(0, 100);
                }
                break;
            case Quality.Remarkable:
                stat3 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat3Value = Random.Range(0, 100);
                stat2 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat2Value = Random.Range(0, 100);
                if (Random.Range(0f,1f)>0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                    stat1 = Stats.None;
                    stat1Value = 0;
                }
                else
                {
                    stat1 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    stat1Value = Random.Range(0, 100);
                }
                break;
            case Quality.Noteworthy:
                stat2 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat2Value = Random.Range(0, 100);
                if (Random.Range(0f,1f)>0.5f)
                {
                    ability = GameManager.Instance.abilityRegistry.GetRandomAbility();
                    stat1 = Stats.None;
                    stat1Value = 0;
                }
                else
                {
                    stat1 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                    stat1Value = Random.Range(0, 100);
                }
                break;
            case Quality.Typical:
                stat1 = (Stats) Random.Range(1, Enum.GetNames(typeof(Stats)).Length);
                stat1Value = Random.Range(0, 100);
                break;
        }
    }
    
    public enum Quality
    {
        Typical, //white 1 stat or ability
        Noteworthy, //green 1 stat, 1 stat or ability
        Remarkable, //blue 2 stats, 1 stat or ability
        Choice, //purple 3 stats, 1 stat or ability
        Signature, //orange 4 stats, 1 stat or ability
        Fabled, //yellow 5 stats, 1 ability
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