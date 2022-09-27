using System;using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability/ConsumerAbility")]
public class ConsumerAbility : AbilityObject
{
    [field: Header("Consumer")]
    [field: SerializeField] public int amountToConsume { get; private set; } = 0;
    [field: SerializeField] public bool strict { get; private set; } = false;
    [field: SerializeField] public bool stacksFromUser { get; private set; } = false;
    [field: SerializeField] public float cTargetDamageMultiplier { get; private set; } = 1;

    [field: SerializeField] public float cMinTargetDamageMultiplier { get; private set; } = 1;

    [field: SerializeField] public bool cDamageTarget { get; private set; } = false;
    [field: SerializeField] public bool cMultiplyByStacks { get; private set; } = false;

    [field: SerializeField] public bool cDamageUser { get; private set; } = false;
    [field: SerializeField] public float cUserDamageMultiplier { get; private set; } = 1;
    [field: SerializeField] public float cMinUserDamageMultiplier { get; private set; } = 1;
    [field: SerializeField] public bool cHealTarget { get; private set; } = false;
    [field: SerializeField] public bool cHealUser { get; private set; } = false;
    [field: SerializeField] public bool cShieldTarget { get; private set; } = false;
    [field: SerializeField] public float cTargetShieldMultiplier { get; private set; } = 1;
    [field: SerializeField] public bool cShieldUser { get; private set; } = false;
    [field: SerializeField] public float cUserShieldMultiplier { get; private set; } = 1;

    [field: SerializeField] public StatusEffect type { get; private set; }

    public override void Execute(Shell user, Shell target)
    {
        Type t = type.GetType();
        int stacks =stacksFromUser?user.statusDisplayer.GetStatusDuration(t):target.statusDisplayer.GetStatusDuration(t);

        if (cDamageTarget)
        {
            if (stacks > (strict?amountToConsume-1:0))
            {
                target.Damage(user,(int)(user.brain.GetStatusDamage() * (cMultiplyByStacks?stacks:1)*cTargetDamageMultiplier),false);
                if (amountToConsume==0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStatus(t);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStatus(t);
                    }
                } else if (amountToConsume>0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                }
            }
            else
            {
                target.Damage(user,(int)(user.brain.GetDamage()*cMinTargetDamageMultiplier),false);
            }
        }

        if (cDamageUser)
        {
            if (stacks > (strict?amountToConsume-1:0))
            {
                user.Damage(user, (int)(user.brain.GetStatusDamage() * (cMultiplyByStacks?stacks:1)*cUserDamageMultiplier), false);
                if (amountToConsume==0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStatus(t);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStatus(t);
                    }
                } else if (amountToConsume>0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                }
            } else
            {
                user.Damage(user,(int)(user.brain.GetDamage()*cMinUserDamageMultiplier),false);
            }
        }

        if (cHealTarget)
        {
            if (stacks > (strict?amountToConsume-1:0))
            {
                target.Heal((int)(user.brain.GetStatusDamage() * stacks*targetHealMultiplier));
                if (amountToConsume==0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStatus(t);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStatus(t);
                    }
                } else if (amountToConsume>0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                }
            }
        }

        if (cHealUser)
        {
            if (stacks > (strict?amountToConsume-1:0))
            {
                user.Heal((int)(user.brain.GetStatusDamage() * stacks*userHealMultiplier));
                if (amountToConsume==0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStatus(t);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStatus(t);
                    }
                } else if (amountToConsume>0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                }
            }
        }

        if (cShieldTarget)
        {
            if (stacks > (strict?amountToConsume-1:0))
            {
                target.Shield((int)(target.brain.GetShieldRate() * amountToConsume*cTargetShieldMultiplier));
                if (amountToConsume==0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStatus(t);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStatus(t);
                    }
                } else if (amountToConsume>0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                }
            }
        }
        
        if (cShieldUser)
        {
            if (stacks > (strict?amountToConsume-1:0))
            {
                user.Shield((int)(user.brain.GetShieldRate() * amountToConsume*cUserShieldMultiplier));
                if (amountToConsume==0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStatus(t);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStatus(t);
                    }
                } else if (amountToConsume>0)
                {
                    if (stacksFromUser)
                    {
                        user.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                    else
                    {
                        target.statusDisplayer.RemoveStacks(t,amountToConsume);
                    }
                }
            }
        }

        base.Execute(user, target);
    }
}