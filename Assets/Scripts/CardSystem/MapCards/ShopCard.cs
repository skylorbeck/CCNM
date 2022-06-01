using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShopCard", menuName = "Cards/MapCard/ShopCard")]
public class ShopCard : MapCard
{
    public new MapCardType mapCardType { get; protected set; } = MapCardType.Shop;

    [field:SerializeField] public ShopType shopType { get; private set; } = ShopType.General ;
    public enum ShopType
    {
        General,//items, food
        Weapon,//armor, weapons
        Magic,//consumables, magic items/armor/weapons. Cost something extra? One choice?
    }
}