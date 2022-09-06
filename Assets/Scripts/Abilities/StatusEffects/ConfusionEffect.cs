using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfusionEffect", menuName = "Combat/StatusEffect/ConfusionEffect")]
public class ConfusionEffect : StatusEffect
{
    public override int OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        if (Random.value < 0.25f)
        {
            attacker.Damage(attacker, baseDamage ,Element.None);
            TextPopController.Instance.PopNegative("Confused", attacker.transform.position,attacker.isPlayer);
        }
        
        return base.OnAttack(target, attacker, baseDamage);
    }
}
