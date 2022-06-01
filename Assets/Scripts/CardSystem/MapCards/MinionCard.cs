using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MinionCard", menuName = "Cards/MapCard/MinionCard")]
public class MinionCard : MapCard
{
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }
    
}