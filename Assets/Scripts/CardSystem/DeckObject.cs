using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Deck", menuName = "Cards/Deck")]
public class DeckObject : ScriptableObject
{
    [field:SerializeField] public Sprite icon { get;private set; }
    [field:SerializeField] public BossCard BossCard { get;private set; }
    [field:SerializeField] public MiniBossCard[] MiniBossCard { get;private set; }
    [field:SerializeField] public MinionCard[] MinionCards { get;private set; }
    [field:SerializeField] public EventCard[] EventCards { get;private set; }
    [field:SerializeField] public ShopCard[] ShopCards { get;private set; }
    [field: Header("Rewards")]
    [field:SerializeField] public ItemCard[] ItemCards { get;private set; }
    [field:SerializeField] public ConsumableCard[] ConsumableCards { get;private set; }
}
