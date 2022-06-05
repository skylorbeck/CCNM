using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCard", menuName = "Cards/ItemCard")]
public class ItemCard : CardObject
{
    [field: Header("ItemCard")]

    [field:SerializeField] public AbilityObject ability{ get; protected set; }

    public new CardType cardType { get; protected set; } = CardType.Item;
    
    [field: SerializeField] public Quality quality { get; set; }
    
    [field: SerializeField] public Stats stat1 { get; set; } = Stats.None;
    [field: SerializeField] public float stat1Value { get; set; } = 0;
    [field: SerializeField] public Stats stat2 { get; set; } = Stats.None;
    [field: SerializeField] public float stat2Value { get; set; } = 0;
    [field: SerializeField] public Stats stat3 { get; set; } = Stats.None;
    [field: SerializeField] public float stat3Value { get; set; } = 0;
    [field: SerializeField] public Stats stat4 { get; set; } = Stats.None;
    [field: SerializeField] public float stat4Value { get; set; } = 0;
    [field: SerializeField] public Stats stat5 { get; set; } = Stats.None;
    [field: SerializeField] public float stat5Value { get; set; } = 0;

    public enum Stats
    {
        None,
        Damage,
        Shield,
        CriticalChance,
        CriticalDamage,
        Health,
    }

    public enum Quality
    {
        Typical, //white
        Noteworthy, //green
        Remarkable, //blue
        Choice, //purple
        Signature, //orange
        Fabled, //yellow
        Curator, //red
    }
    public enum ItemType
    {
        Weapon, //offensive abilities?
        Shield, //offensive/defensive abilities?
        Helmet, //just for stats?
        Chest,  //defensive abilities?
        Gloves, //just for stats?
        Pants,  //just for stats?
        Boots,  //just for stats?
        Ring,   //magic abilities?
        Amulet, //magic abilities?
    }
    
}