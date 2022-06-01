using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCard", menuName = "Cards/ItemCard")]
public class ItemCard : CardObject
{
    [field: Header("ItemCard")]
    [field:SerializeField] public ItemType itemType{ get; protected set; }

    [field:SerializeField] public AbilityObject ability{ get; protected set; }
    [field:SerializeField] public int defense{ get; protected set; }
    [field:SerializeField] public int attack{ get; protected set; }
    [field:SerializeField] public int health{ get; protected set; }

    public new CardType cardType { get; protected set; } = CardType.Item;
    
    public enum ItemType
    {
        Weapon,
        Shield,
        Helmet,
        Chest,
        Gloves,
        Pants,
        Boots,
        Ring,
        Amulet,
    }
}