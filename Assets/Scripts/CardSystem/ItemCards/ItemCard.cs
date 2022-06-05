using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCard", menuName = "Cards/ItemCard")]
public class ItemCard : CardObject
{
    [field: Header("ItemCard")]
    [field:SerializeField] public ItemType itemType { get; protected set; }
   
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