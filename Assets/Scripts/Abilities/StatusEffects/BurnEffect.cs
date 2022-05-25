using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnEffect", menuName = "Combat/StatusEffect/BurnEffect")]
public class BurnEffect : StatusEffect
{
    [field: SerializeField] public int damage { get; private set; } = 1;

    public override void OnApply(Shell target)
    {
        //do something to the shell
    }

    public override void OnRemove(Shell target)
    {
        //do something to the shell
    }
    
    public override async Task Tick(Shell target)
    {
        target.Damage(damage);
        await base.Tick(target);
    }
}
