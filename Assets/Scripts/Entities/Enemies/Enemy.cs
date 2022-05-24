using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Combat/EnemySO")]
public class Enemy : Entity
{
    [field: SerializeField] public int enemySlot = 0;
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
}
