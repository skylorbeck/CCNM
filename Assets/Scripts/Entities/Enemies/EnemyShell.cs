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

    public override void Shield(int amount)
    {
        amount = (int)(amount * GameManager.Instance.runSettings.GetDefenseMod());
        base.Shield(amount);
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
        base.Kill();
    }
}
