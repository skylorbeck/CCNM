using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Combat/EnemySO")]
public class EnemyBrain : Brain
{
    public async Task EnemyTurn()
    {
        Debug.Log(title+ " Turn");
        await Task.Delay(1000);
    }
}
