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