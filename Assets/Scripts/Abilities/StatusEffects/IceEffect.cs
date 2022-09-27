using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceEffect", menuName = "Combat/StatusEffect/IceEffect")]
public class IceEffect : StatusEffect
{
    public StatusEffect FreezeEffect;

    public override void OnApply(Shell target, Shell attacker,int duration,int power)
    {
        if (target.statusDisplayer.HasStatus(GetType()) && target.statusDisplayer.GetStatusDuration(GetType())>=maxStacks)
        {
            target.statusDisplayer.RemoveStatus(GetType());
            target.statusDisplayer.AddStatus(FreezeEffect,target,attacker,1);
        }
        else
        {
            base.OnApply(target, attacker, duration, power);
        }
    }
}