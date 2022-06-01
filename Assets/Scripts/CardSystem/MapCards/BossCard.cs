using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BossCard", menuName = "Cards/MapCard/BossCard")]
public class BossCard : MapCard
{
    [field:SerializeField] public EnemyBrain bossBrain{ get;private set; }
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }

}