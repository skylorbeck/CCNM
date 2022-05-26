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
    public async Task Attack(Shell target)
    {
        await enemyBrain.Think();
        Symbol symbol = Instantiate(Resources.Load<Symbol>("Prefabs/Slotmachine/SymbolPrefab"), transform);
        symbol.SetAbility(brain.GetRandomAbility());
        await symbol.Consume(target,this);
        Destroy(symbol);
    }
}
