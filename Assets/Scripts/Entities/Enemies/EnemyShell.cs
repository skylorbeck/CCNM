using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyShell : Shell
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform spotLight;
    [SerializeField] private Transform shadow;

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
                spotLight.localPosition = new Vector3(0, -0.05f, 0);
                shadow.localPosition = new Vector3(0, -0.33f, 0);
            }
        }
        base.InsertBrain(brain);
        health = maxHealth = (int)(health * GameManager.Instance.runSettings.GetHealthMod());
    }

    public override Task<int> OnAttack(Shell target, int baseDamage)
    {
        baseDamage = (int)(baseDamage * GameManager.Instance.runSettings.GetAttackMod());
        GameManager.Instance.uiStateObject.Ping(title+" attacked for "+baseDamage+" damage!");
        return base.OnAttack(target, baseDamage);
    }

    public override void Shield(int amount)
    {
        amount = (int)(amount * GameManager.Instance.runSettings.GetDefenseMod());
        GameManager.Instance.uiStateObject.Ping( title+" gained " + amount + " shield!");
        base.Shield(amount);
    }

    public override void Heal(Shell source, int baseHeal, StatusEffect.Element element)
    {
        GameManager.Instance.uiStateObject.Ping(title+" healed for " + baseHeal + "!");
        base.Heal(source, baseHeal, element);
    }
    
    public override async Task Attack(Shell target, Symbol symbol)
    {
        Light();
        await Task.Delay(250);
        await base.Attack(target, symbol);
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
        Dim();
        GameManager.Instance.uiStateObject.Ping(title + " died!");
        base.Kill();
    }
}
