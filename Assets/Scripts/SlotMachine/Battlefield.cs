using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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
        player.ClearPlayerObject();
        player.Clone(GameManager.Instance.metaPlayer);
        player.AddConsumables(0,1);
        player.AddConsumables(1,1);
        player.AddConsumables(2,1);
    }
    
    public void SetLevel(int level)
    {
        enemyLevel = level;
    }


    public void InsertSave(SavableBattlefield savableBattlefield)
    {
        deckChosen = savableBattlefield.deckChosen;
        totalHands = savableBattlefield.totalHands;
        if (savableBattlefield.deckIndex != -1)
        {
            deck = GameManager.Instance.deckRegistry.GetDeck(savableBattlefield.deckIndex);
        }
        randomState = savableBattlefield.randomState;
        player.InsertSaveFile(savableBattlefield.player);
    }

    public void ClearBattlefield()
    {
        totalHands = 0;
        randomState = null;
        deckChosen = false;
    }
}
[Serializable]
public class SavableBattlefield
{
    public bool deckChosen;
    public int deckIndex;
    public int totalHands;
    public Random.State randomState;
    public SavablePlayerBrain player;
    
    public SavableBattlefield(Battlefield battlefield)
    {
        deckChosen = battlefield.deckChosen;
        if (deckChosen)
        {
            deckIndex = GameManager.Instance.deckRegistry.GetDeckIndex(battlefield.deck.name);
        }
        else deckIndex = -1;
        totalHands = battlefield.totalHands;
        if (battlefield.randomState != null)
        {
            randomState = battlefield.randomState.Value;
        }
        else
        {
            Random.InitState(DateTime.Now.Millisecond);
            randomState = Random.state;
        }
        player = new SavablePlayerBrain(battlefield.player);
    }
}
