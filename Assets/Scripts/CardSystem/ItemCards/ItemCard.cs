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
    
    public AbilityObject GetRandomAbility()
    {
        return possibleAbilities[UnityEngine.Random.Range(0, possibleAbilities.Length)];
    }
   
    public enum ItemType
    {
        Shield, //offensive/defensive abilities?
        Weapon, //offensive abilities?
        Ring,   //magic abilities?
        Pants,  //just for stats?
        Chest,  //defensive abilities?
        Helmet, //just for stats?
        Boots,  //just for stats?
        Gloves, //just for stats?
        Amulet, //magic abilities?
    }
}