using System.Linq;
using UnityEngine;

public class EnemyShell : Shell
{ 
    public EnemyBrain enemyBrain
    {
        get { return (EnemyBrain)brain;}
        private set => InsertBrain(value);
    }
    
}
