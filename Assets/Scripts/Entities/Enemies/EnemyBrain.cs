using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Combat/EnemySO")]
public class EnemyBrain : Brain
{
    [field: SerializeField] public int credits { get; private set; }
    public bool isBoss;

    #region computedStatGetters
    public override float GetDamage()
    {
        return GetStrength()* GameManager.Instance.runSettings.GetAttackMod();
    }

    public override float GetShieldMax()
    {
        return GetGrit() * GameManager.Instance.runSettings.GetShieldMod();
    }

    public override float GetShieldRate()
    {
        return GetResolve() * GameManager.Instance.runSettings.GetShieldMod();
    }

    public override float GetCritChance()
    {
        return GetSpeed() * GameManager.Instance.runSettings.GetCritChanceMod();
    }

    public override float GetCritDamage()
    {
        return GetSkill() * GameManager.Instance.runSettings.GetCritDamageMod();
    }

    public override float GetDodgeChance()
    {
        return GetDexterity() * GameManager.Instance.runSettings.GetDodgeMod();
    }
    public override float GetMaxHealth()
    {
        return GetVitality() * GameManager.Instance.runSettings.GetHealthMod();
    }
    #endregion
    public async Task Think()
    {
        
        //todo enemy turn logic
        await Task.Delay(1000);
    }
}
