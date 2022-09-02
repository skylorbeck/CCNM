using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Combat/EnemySO")]
public class EnemyBrain : Brain
{
    public bool isBoss;
    public bool isBlank;

    #region computedStatGetters
    public override int GetDamage()
    {
        return (int)(base.GetDamage() * GameManager.Instance.runPlayer.level * GameManager.Instance.runSettings.GetAttackMod());
    }

    public override int GetShieldMax()
    {
        return (int)(base.GetShieldMax() * GameManager.Instance.runPlayer.level * GameManager.Instance.runSettings.GetShieldMod());
    }

    public override int GetShieldRate()
    {
        return (int)(base.GetShieldRate() * GameManager.Instance.runPlayer.level * GameManager.Instance.runSettings.GetShieldMod());
    }

    public override float GetCritChance()
    {
        return (int)(base.GetCritChance() * GameManager.Instance.runSettings.GetCritChanceMod());
    }

    public override float GetCritDamage()
    {
        return (int)(base.GetCritDamage() * GameManager.Instance.runSettings.GetCritDamageMod());
    }

    public override float GetDodgeChance()
    {
        return (int)(base.GetDodgeChance() * GameManager.Instance.runSettings.GetDodgeMod());
    }
    public override int GetHealthMax()
    {
        return (int)(base.GetHealthMax() * GameManager.Instance.runPlayer.level * GameManager.Instance.runSettings.GetHealthMod());//todo multiply by player level
    }
    #endregion
    public async Task Think()
    {
        
        //todo enemy turn logic
        await Task.Delay(1000);
    }
}
