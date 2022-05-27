using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyAbilityWheel : AbilityWheel
{
    public float moveSpeed = 15f;
    public float moveSpeedMin = 10f;
    public float moveSpeedMax = 20f;
    public async Task Move(Vector3 targetPos)
    {
        Vector3 localPosition = transform.localPosition;
        moveSpeed = Random.Range(moveSpeedMin, moveSpeedMax);
        do
        {
            localPosition = Vector3.MoveTowards(localPosition, targetPos, Time.deltaTime * moveSpeed);
            transform.localPosition = localPosition;
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        } while (Vector3.Distance(transform.localPosition,targetPos)> 0f);
    }
}