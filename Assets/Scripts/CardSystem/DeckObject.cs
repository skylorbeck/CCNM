using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Deck", menuName = "Cards/Deck")]
public class DeckObject : ScriptableObject
{
    [field:SerializeField] public Sprite icon { get;private set; }
    [field: SerializeField] public Color[] colors { get; private set; } = new Color[]{Color.white,Color.white};
    [field: SerializeField] public int BossAt { get; private set; } =  5;
    [field: SerializeField] public int[] MiniBossAt { get; private set; } = new[] { 3 };
    [field: SerializeField] public int[] shopAt { get; private set; } = new[] { 4 };
    [field: SerializeField] public int[] eventAt { get; private set; } = new[] { 1,2 };
    [field:SerializeField] public BossCard BossCard { get;private set; }
    [field:SerializeField] public MiniBossCard[] MiniBossCard { get;private set; }
    [field:SerializeField] public MinionCard[] MinionCards { get;private set; }
    [field:SerializeField] public EventCard[] EventCards { get;private set; }
    [field:SerializeField] public ShopCard[] ShopCards { get;private set; }
    [field: Header("Rewards")]
    [field:SerializeField] public ItemCard[] ItemCards { get;private set; }
    [field:SerializeField] public ConsumableCard[] ConsumableCards { get;private set; }


    public MapCard DrawRandomCard()
    {
        List<MapCard> cards = new List<MapCard>();
        cards.AddRange(MinionCards);
        cards.AddRange(EventCards);
        cards.AddRange(ShopCards);
        return cards[Random.Range(0, cards.Count)];
    }
    
    public MapCard DrawBossCard()
    {
        return BossCard;
    }
    
    public MapCard DrawMiniBossCard()
    {
        return MiniBossCard[Random.Range(0, MiniBossCard.Length)];
    }
    
    public MapCard DrawMinionCard()
    {
        return MinionCards[Random.Range(0, MinionCards.Length)];
    }
    
    public MapCard DrawEventCard()
    {
        return EventCards[Random.Range(0, EventCards.Length)];
    }
    
    public MapCard DrawShopCard()
    {
        return ShopCards[Random.Range(0, ShopCards.Length)];
    }
    
    public ItemCard DrawItemCard()
    {
        return ItemCards[Random.Range(0, ItemCards.Length)];
    }
    
    public ConsumableCard DrawConsumableCard()
    {
        return ConsumableCards[Random.Range(0, ConsumableCards.Length)];
    }
}
