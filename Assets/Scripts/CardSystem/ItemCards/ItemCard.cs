using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemCard", menuName = "Cards/ItemCard")]
[Serializable]
public class ItemCard : CardObject
{
    [field: Header("ItemCard")]
    [field:SerializeField] public ItemType itemType { get; protected set; }
    [field:SerializeField] public AbilityObject[] possibleAbilities { get; protected set; }
    [field:SerializeField] public EquipmentDataContainer.Stats[] guaranteeStats { get; protected set; }
    
    
    public AbilityObject GetRandomAbility()
    {
        return possibleAbilities[UnityEngine.Random.Range(0, possibleAbilities.Length)];
    }
   
    public enum ItemType
    {
        Armor, //defensive
        Weapon, //offensive
        Accessory,
    }
}