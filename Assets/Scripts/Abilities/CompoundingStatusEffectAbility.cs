using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability/CompoundingStatusEffectAbility")]
public class CompoundingStatusEffectAbility : AbilityObject
{
    [field: SerializeField] public StatusEffect ifhave { get; private set; }
    public int duration;
    public bool invert;

    public override void Execute(Shell user, Shell target)
    {
        if (invert?!target.statusDisplayer.HasStatus(ifhave):target.statusDisplayer.HasStatus(ifhave))
        {
            target.statusDisplayer.AddStatus(targetStatus, target, user, duration);
        }

        base.Execute(user, target);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
