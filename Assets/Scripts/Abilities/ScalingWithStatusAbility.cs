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
        int stacks= 0;
        if (scaleWithUser)
        {
            stacks = user.statusDisplayer.GetStatusDuration(ifhave.GetType());
            if (user.statusDisplayer.HasStatus(ifhave))
            {
                target.Damage(user,Mathf.RoundToInt(user.brain.GetStatusDamage()*ifhavevalue*stacks),false);
            }
        } else if (target.statusDisplayer.HasStatus(ifhave))
        {
            stacks = target.statusDisplayer.GetStatusDuration(ifhave.GetType());
            target.Damage(user,Mathf.RoundToInt(user.brain.GetStatusDamage()*ifhavevalue*stacks),false);
        }
    }
}
