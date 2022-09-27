using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Wet", menuName = "Combat/StatusEffect/WetEffect")]
public class WetEffect : StatusEffect
{
    public override void OnApply(Shell target, Shell attacker, int duration, int power)
    {
        if (target.statusDisplayer.HasStatus(typeof(BurnEffect)))
        {
            target.statusDisplayer.RemoveStatus(typeof(BurnEffect));
        }
        base.OnApply(target, attacker, duration, power);
    }
}