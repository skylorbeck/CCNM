using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability/ScalingWithStatusAbility")]
public class ScalingWithStatusAbility : AbilityObject
{
    [field: SerializeField] public StatusEffect ifhave { get; private set; }
    [field: SerializeField] public bool scaleWithUser { get; private set; }
    [field: SerializeField] public float ifhavevalue { get; private set; } = 1;
  
    
    public override void Execute(Shell user, Shell target)
    {
        base.Execute(user, target);
        int stacks = user.statusDisplayer.GetStatusCount(ifhave.GetType());
        if (scaleWithUser)
        {
            if (user.statusDisplayer.HasStatus(ifhave))
            {
                target.Damage(user,(int)(user.brain.GetStatusDamage()*ifhavevalue*stacks),false);
            }
        } else if (target.statusDisplayer.HasStatus(ifhave))
        {
            target.Damage(user,(int)(user.brain.GetStatusDamage()*ifhavevalue*stacks),false);
        }
    }
}
