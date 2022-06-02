using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battlefield", menuName = "Combat/Battlefield")]
public class Battlefield : ScriptableObject
{
    [field:SerializeField] public DeckObject deck{ get;private set; }
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }
    [field:SerializeField] public PlayerBrain player{ get;private set; }
    public bool deckChosen = false;
    [field: SerializeField] public int totalHands { get; private set; } = 0;
    
    public bool runOver => totalHands >= deck.BossAt;

    public void TotalHandsPlus()
    {
        totalHands++;
    }
    
    public void InsertEnemies(EnemyBrain[] enemies)
    {
        this.enemies = enemies;
    }
    public void InsertPlayer(PlayerBrain player)
    {
        this.player = player;
    }
    public void InsertDeck(DeckObject deck)//todo deck chooser system
    {
        this.deck = deck;
        deckChosen = true;
    }

    public void Reset()
    {
        totalHands = 0;
    }
}
