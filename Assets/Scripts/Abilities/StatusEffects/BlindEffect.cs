using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BlindEffect", menuName = "Combat/StatusEffect/BlindEffect")]

public class BlindEffect : StatusEffect
{
    public override int OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        baseDamage = 0;
        return base.OnAttack(target, attacker, baseDamage);
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
