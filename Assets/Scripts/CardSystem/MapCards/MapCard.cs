using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapCard : CardObject
{
    [field:SerializeField] public MapCardType mapCardType { get; protected set; }
    
    public enum MapCardType
    {
        Shop,
        Boss,
        MiniBoss,
        Minion,
        Event,
    }
}