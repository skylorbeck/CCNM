using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Combat/EnemySO")]
public class EnemyBrain : Brain
{
    public bool isBoss;
    public async Task Think()
    {
        
        //todo enemy turn logic
        await Task.Delay(1000);
    }
}
