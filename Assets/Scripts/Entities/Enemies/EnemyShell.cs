using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyShell : Shell
{ 
    
    public EnemyBrain enemyBrain
    {
        get { return (EnemyBrain)brain;}
        private set => InsertBrain(value);
    }
    public override async Task Attack(Shell target,Symbol symbol)
    {
        spriteRenderer.color = Color.white;
        await base.Attack(target,symbol);
    }

    public void Dim()
    {
        spriteRenderer.color = Color.gray;
    }
}
