using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveSystem;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Battlefield", menuName = "Combat/Battlefield")]
public class Battlefield : ScriptableObject
{
    [field:SerializeField] public DeckObject deck{ get;private set; }
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }
    [field:SerializeField] public PlayerBrain player{ get;private set; }
    [field:SerializeField] public bool deckChosen{ get;private set; }= false;
    [field:SerializeField] public bool runStarted{ get;private set; }= false;
    [field: SerializeField] public int totalHands { get; private set; } = 0;
    [field: SerializeField] public int enemyLevel { get; private set; } = 0;
    public Random.State? randomState = null;

    

    public bool runOver => totalHands > deck.bossAt;

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
    public void InsertDeck(DeckObject deck)
    {
        this.deck = deck;
        deckChosen = true;
    }

    public void Reset()
    {
        ClearBattlefield();
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
        runStarted = savableBattlefield.runStarted;
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
        totalHands = 1;
        randomState = null;
        deckChosen = false;
        runStarted = false;
    }
    public void StartRun()
    {
        runStarted = true;
    }
}
[Serializable]
public class SavableBattlefield
{
    public bool deckChosen;
    public bool runStarted;
    public int deckIndex;
    public int totalHands;
    public Random.State randomState;
    public SavablePlayerBrain player;
    
    public SavableBattlefield(Battlefield battlefield)
    {
        runStarted = battlefield.runStarted;
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

