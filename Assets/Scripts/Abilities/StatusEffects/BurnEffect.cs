using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Combat/StatusEffect/BurnEffect")]
public class BurnEffect : StatusEffect
{
    [field: SerializeField] public int damageRatio { get; private set; } = 1;

    public override void Tick(Shell target, Shell source, int duration, int power)
    {
        target.Damage(source,damageRatio* (power),element,false);
        TextPopController.Instance.PopNegative("Burned", target.transform.position,target.isPlayer);
        base.Tick(target, source, duration, power);
    }
}
