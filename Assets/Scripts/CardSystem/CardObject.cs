using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObject : ScriptableObject
{
    [field:SerializeField] public Sprite icon { get;private set; }
    [field:SerializeField] public string cardTitle{ get;private set; }
    [field:SerializeField] public string cardDescription{ get;private set; }
    [field:SerializeField] public int cardCost{ get;private set; }//price to buy

    [field:SerializeField] public CardType cardType { get; protected set; }

    public enum CardType
    {
        Item, //equippable
        Consumable, //one time use
        Map, //level generator
    }
}