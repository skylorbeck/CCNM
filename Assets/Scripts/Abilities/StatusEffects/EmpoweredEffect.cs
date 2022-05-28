using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EmpoweredEffect", menuName = "Combat/StatusEffect/EmpoweredEffect")]
public class EmpoweredEffect : StatusEffect
{
    public override Task<int> OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        baseDamage *= 2;
        return base.OnAttack(target, attacker, baseDamage);
    }
}
