using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GiveStatusAtEndOfTurn", menuName = "Combat/StatusEffect/GiveStatusAtEndOfTurn")]
public class GiveStatusAtEndOfTurn : StatusEffect
{
    public StatusEffect statusEffect;
    public int duration;
    public override void OnRemove(Shell target)
    {
        target.AddStatusEffect(statusEffect,target,duration);
        base.OnRemove(target);
    }
}
