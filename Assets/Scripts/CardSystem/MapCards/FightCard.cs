using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FightCard : MapCard
{
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }
    
}