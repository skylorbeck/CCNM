using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battlefield", menuName = "Combat/Battlefield")]
public class Battlefield : ScriptableObject
{
    public EnemyBrain[] enemies;
    public PlayerBrain player;
    public bool fightOver = false;
}
