using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Deck", menuName = "Cards/Deck")]
public class DeckObject : ScriptableObject
{
    [field: SerializeField] public int level { get; private set; } = 1;

    [field:Header("Icons")]
    [field:SerializeField] public Sprite icon { get;private set; }
    [field:SerializeField] public Sprite deckBack { get;private set; }
    [field:SerializeField] public Sprite consumableWall { get;private set; }
    [field:SerializeField] public Sprite floor { get;private set; }
    [field:SerializeField] public Sprite wall { get;private set; }
    [field:SerializeField] public Sprite wheelCover { get;private set; }
    [field:Header("cardAt")]
    [field: SerializeField] public int miniBossEvery { get; private set; } = 3;
    [field: SerializeField] public int shopEvery { get; private set; } = 4;
    [field: SerializeField] public int eventEvery { get; private set; }= 2;
    [field: SerializeField] public string[] eventNames { get; private set; }
    [field:Header("EnemyRefs")]

    [field:SerializeField] public BossCard bossCard { get;private set; }
    [field:SerializeField] public MiniBossCard[] miniBossCard { get;private set; }
    [field:SerializeField] public MinionCard[] minionCards { get;private set; }
    [field:SerializeField] public ShopCard[] shopCards { get;private set; }
    [field: Header("Rewards")]
    [field:SerializeField] public ItemCard[] itemCards { get;private set; }
    [field:SerializeField] public ConsumableCard[] consumableCards { get;private set; }
    [field:SerializeField] public Relic[] relics { get;private set; }
    
    [field: Header("Music")]
    [field:SerializeField] public int[] combat { get;private set; }
    [field:SerializeField] public int[] map { get;private set; }

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
