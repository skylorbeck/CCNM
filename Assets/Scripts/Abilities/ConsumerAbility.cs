﻿using System;using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability/ConsumerAbility")]
public class ConsumerAbility : AbilityObject
{
    [field: Header("Consumer")]
    [field: SerializeField] public bool cDamageTarget { get; private set; } = false;

    [field: SerializeField] public bool cDamageUser { get; private set; } = false;
    [field: SerializeField] public bool cHealTarget { get; private set; } = false;
    [field: SerializeField] public bool cHealUser { get; private set; } = false;

    [field: SerializeField] public StatusEffect type { get; private set; }

    public override void Execute(Shell user, Shell target)
    {
        Type t = type.GetType();
        if (cDamageTarget)
        {
            int stacks = target.statusDisplayer.GetStatusDuration(t);
            if (stacks > 0)
            {
                target.Damage(user,(int)(user.brain.GetDamage()*0.1f)+ (int)(user.brain.GetStatusDamage() * stacks*targetDamageMultiplier),false);
                target.statusDisplayer.RemoveStatus(t);
            }
        }

        if (cDamageUser)
        {
            int stacks = user.statusDisplayer.GetStatusDuration(t);
            if (stacks > 0)
            {
                user.Damage(user, (int)(user.brain.GetDamage()*0.1f)+ (int)(user.brain.GetStatusDamage() * stacks*userDamageMultiplier), false);
                user.statusDisplayer.RemoveStatus(t);

            }
        }

        if (cHealTarget)
        {
            int stacks = target.statusDisplayer.GetStatusDuration(t);
            if (stacks > 0)
            {
                target.Heal((int)(user.brain.GetStatusDamage() * stacks*targetHealMultiplier));
                target.statusDisplayer.RemoveStatus(t);
            }
        }

        if (cHealUser)
        {
            int stacks = user.statusDisplayer.GetStatusDuration(t);
            if (stacks > 0)
            {
                user.Heal((int)(user.brain.GetStatusDamage() * stacks*userHealMultiplier));
                user.statusDisplayer.RemoveStatus(t);
            }

        }

        base.Execute(user, target);
    }
}