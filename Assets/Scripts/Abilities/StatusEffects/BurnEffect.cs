using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Combat/StatusEffect/BurnEffect")]
public class BurnEffect : StatusEffect
{
    [field: SerializeField] public int damage { get; private set; } = 1;
    

    public override async Task Tick(Shell target)
    {
        await target.Damage(null,(int)(damage* (target.isPlayer?GameManager.Instance.runSettings.GetAttackMod():GameManager.Instance.battlefield.player.GetStatusDamage())),element);
        TextPopController.Instance.PopNegative("Burned", target.transform.position,target.isPlayer);
        await base.Tick(target);
    }
}
