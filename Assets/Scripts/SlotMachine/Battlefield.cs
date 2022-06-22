using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Battlefield", menuName = "Combat/Battlefield")]
public class Battlefield : ScriptableObject
{
    [field:SerializeField] public DeckObject deck{ get;private set; }
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }
    [field:SerializeField] public PlayerBrain player{ get;private set; }
    public bool deckChosen = false;
    [field: SerializeField] public int totalHands { get; private set; } = 0;
    [field: SerializeField] public int enemyLevel { get; private set; } = 0;
    public Random.State? randomState = null;

    
    public bool runOver => totalHands > deck.BossAt;

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
        totalHands = 1;
        player.Clone(GameManager.Instance.metaPlayer);
    }
    
    public void SetLevel(int level)
    {
        enemyLevel = level;
    }
}
