using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyAbilityWheel : AbilityWheel
{
    public float moveSpeed = 5f;
    public async Task Move(Vector3 targetPos)
    {
        Vector3 localPosition = transform.localPosition;
        do
        {
            localPosition = Vector3.MoveTowards(localPosition, targetPos, Time.deltaTime * moveSpeed);
            transform.localPosition = localPosition;
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        } while (Vector3.Distance(transform.localPosition,targetPos)> 0.1f);
    }

}