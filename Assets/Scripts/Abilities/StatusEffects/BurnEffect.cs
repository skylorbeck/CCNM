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
        await target.Damage(null,damage,element);
        TextPopController.Instance.PopNegative("Burned", target.transform.position);
        await base.Tick(target);
    }
}
