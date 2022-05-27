using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfusionEffect", menuName = "Combat/StatusEffect/ConfusionEffect")]
public class ConfusionEffect : StatusEffect
{
    public override async Task<int> OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        if (Random.value < 0.25f)
        {
            await attacker.Damage(attacker, baseDamage ,Element.None);
            await Task.Delay(100);
            TextPopController.Instance.PopNegative("Confused", attacker.transform.position);
        }
        
        return await base.OnAttack(target, attacker, baseDamage);
    }
}
