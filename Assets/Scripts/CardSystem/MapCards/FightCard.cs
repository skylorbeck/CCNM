using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightCard : MapCard
{
    
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }
    public override Sprite Icon()
    {
        List<EnemyBrain> nonBlankEnemies = new List<EnemyBrain>();
        foreach (EnemyBrain enemyBrain in enemies)
        {
            if (enemyBrain.isBlank)
            {
                continue;
            }
            nonBlankEnemies.Add(enemyBrain);
        }
        EnemyBrain enemy = nonBlankEnemies[Random.Range(0, nonBlankEnemies.Count)];
        
        return enemy.icon[Random.Range(0, enemies.Length)];
    }
}