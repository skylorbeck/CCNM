using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerObject", menuName = "Combat/PlayerSO")]
public class PlayerObject : Entity
{
    public new void Update()
    {
        base.Update();
    }

    public new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    public new void Damage(int damage)
    {
        base.Damage(damage);
    }
    
    public new void Heal(int heal)
    {
        base.Heal(heal);
    }
    
  
    public new void Shield(int amount)
    {
        base.Shield(amount);
    }
    
    public new void AddStatusEffect(StatusEffect statusEffect)
    {
        base.AddStatusEffect(statusEffect);
    }
    
    public new void RemoveStatusEffect(StatusEffect statusEffect)
    {
        base.RemoveStatusEffect(statusEffect);
    }
}
