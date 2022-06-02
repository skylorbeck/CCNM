using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Combat/EnemySO")]
public class EnemyBrain : Brain
{
    [field: SerializeField] public int credits { get; private set; }
    public bool isBoss;
    public async Task Think()
    {
        
        //todo enemy turn logic
        await Task.Delay(1000);
    }
}
