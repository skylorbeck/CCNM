using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Combat/StatusEffect/BurnEffect")]
public class BurnEffect : StatusEffect
{
    [field: SerializeField] public int damage { get; private set; } = 1;
    

    public override void Tick(Shell target)
    {
        //todo replace GetAttackMod with scaling damage from enemy
        target.Damage(null,(int)(damage* (target.isPlayer?GameManager.Instance.runSettings.GetAttackMod():GameManager.Instance.runPlayer.GetStatusDamage())),element);
        TextPopController.Instance.PopNegative("Burned", target.transform.position,target.isPlayer);
        base.Tick(target);
    }
}
