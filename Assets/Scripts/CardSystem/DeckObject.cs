using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Deck", menuName = "Cards/Deck")]
public class DeckObject : ScriptableObject
{
    [field:SerializeField] public Sprite icon { get;private set; }
    [field: SerializeField] public Color[] colors { get; private set; } = new Color[]{Color.white,Color.white};
    [field: SerializeField] public int bossAt { get; private set; } =  5;
    [field: SerializeField] public int[] miniBossAt { get; private set; } = new[] { 3 };
    [field: SerializeField] public int[] shopAt { get; private set; } = new[] { 4 };
    [field: SerializeField] public int[] eventAt { get; private set; } = new[] { 1,2 };
    [field: SerializeField] public string[] eventNames { get; private set; }
    [field:SerializeField] public BossCard bossCard { get;private set; }
    [field:SerializeField] public MiniBossCard[] miniBossCard { get;private set; }
    [field:SerializeField] public MinionCard[] minionCards { get;private set; }
    [field:SerializeField] public ShopCard[] shopCards { get;private set; }
    [field: Header("Rewards")]
    [field:SerializeField] public ItemCard[] itemCards { get;private set; }
    [field:SerializeField] public ConsumableCard[] consumableCards { get;private set; }
    [field:SerializeField] public Relic[] relics { get;private set; }

[Obsolete("Use a more specific method like DrawBossCard()")]
    public MapCard DrawRandomCard()
    {
        List<MapCard> cards = new List<MapCard>();
        cards.AddRange(minionCards);
        cards.AddRange(shopCards);
        return cards[Random.Range(0, cards.Count)];
    }
    
    public MapCard DrawBossCard()
    {
        return bossCard;
    }
    
    public MapCard DrawMiniBossCard()
    {
        return miniBossCard[Random.Range(0, miniBossCard.Length)];
    }
    
    public MapCard DrawMinionCard()
    {
        return minionCards[Random.Range(0, minionCards.Length)];
    }
    
    
    public MapCard DrawShopCard()
    {
        return shopCards[Random.Range(0, shopCards.Length)];
    }
    
    public ItemCard DrawItemCard()
    {
        return itemCards[Random.Range(0, itemCards.Length)];
    }
    
    public ConsumableCard DrawConsumableCard()
    {
        return consumableCards[Random.Range(0, consumableCards.Length)];
    }
}
