using System;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EnemyShell : Shell
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform spotLight;
    [SerializeField] private Transform shadow;

    #region Event Broadcasts
    public override void OnDied()
    {
    }

    public override void OnShieldRegen()
    {
    }

    public override void OnHealed(int amtHealed)
    {
    }

    public override void OnShieldBreak(int shieldDamTaken)
    {
    }

    public override void OnDamaged(Shell attacker, int damageTaken)
    {
    }

    public override void OnAttacked(Shell target,Symbol symbol)
    {
    }
    #endregion
    public EnemyBrain enemyBrain
    {
        get { return (EnemyBrain)brain; }
        private set => InsertBrain(value);
    }

    public override void InsertBrain(Brain brain)
    {
        if (brain is EnemyBrain enemyBrain)
        {
            if (enemyBrain.isBoss)
            {
                transform.localPosition = new Vector3(0, 2.5f, 0);
                spotLight.localPosition = new Vector3(0, 0.85f, 0);
                shadow.localPosition = new Vector3(0, 0.85f, 0);
            }
        }
        base.InsertBrain(brain);
        SetCurrentHealth(brain.GetHealthMax());
    }

    public override int OnAttack(Shell target, int baseDamage)
    {
        int damage = base.OnAttack(target, baseDamage);
        GameManager.Instance.uiStateObject.Ping(title+" attacked for "+baseDamage+" damage!");
        return damage;
    }

    public override void Shield(int amount)
    {
        GameManager.Instance.uiStateObject.Ping( title+" gained " + amount + " shield!");
        base.Shield(amount);
    }

    public override void Heal(int baseHeal)
    {
        GameManager.Instance.uiStateObject.Ping(title+" healed for " + baseHeal + "!");
        base.Heal( baseHeal);
    }
    
    public override async void Attack(Shell target, Symbol symbol)
    {
        Light();
        await Task.Delay(250);
        transform.DOPunchPosition(Vector3.down, 0.5f, 1, 1);
        base.Attack(target, symbol);
    }

    public void Dim()
    {
        animator.SetTrigger("dim");
    }

    public void Light()
    {
        animator.SetTrigger("light");
    }

    public override void Kill()
    {
        GameManager.Instance.uiStateObject.Ping(title + " died!");
        KillSilently();
    }
    
    public void KillSilently()
    {
        Dim();
        base.Kill();
        if (enemyBrain.isBlank)
        {
            return;
        }
        SoundManager.Instance.PlayDeathSound();
        if (enemyBrain.isBoss)
        {
            GameManager.Instance.runPlayer.trackableStats.bossesKilled++;
        }
        else
        {
            GameManager.Instance.runPlayer.trackableStats.minionsKilled++;
        }
    }
}
