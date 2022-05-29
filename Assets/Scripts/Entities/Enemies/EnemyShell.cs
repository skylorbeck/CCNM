using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyShell : Shell
{
    [SerializeField]private Animator animator;
    public EnemyBrain enemyBrain
    {
        get { return (EnemyBrain)brain;}
        private set => InsertBrain(value);
    }
    public override async Task Attack(Shell target,Symbol symbol)
    {
        Light();
        await Task.Delay(250);
        await base.Attack(target,symbol);
    }

    public void Dim()
    {
        animator.SetTrigger("dim");
    }
    
    public void Light()
    {
        animator.SetTrigger("light");
    }}
