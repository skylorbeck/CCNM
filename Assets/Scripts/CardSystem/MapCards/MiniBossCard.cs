using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MiniBossCard", menuName = "Cards/MapCard/MiniBossCard")]
public class MiniBossCard : MapCard
{
    [field:SerializeField] public EnemyBrain bossBrain{ get;private set; }
    [field:SerializeField] public EnemyBrain[] enemies{ get;private set; }
}