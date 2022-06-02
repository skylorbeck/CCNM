using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RunSettings", menuName = "Global/RunSettings")]
public class RunSettings : ScriptableObject
{
    public float health;
    public float defense;
    public float attack;
    public float xp;
    public float credits;
    public float multiplier;//difficulty multiplier
    [Header("Modifiers")]
    public double healthMod;
    public double defenseMod;
    public double attackMod;
    public double xpMod;
    public double creditsMod;
    public double finalMultiplier;

    public float GetHealthMod()
    {
        Debug.Log(healthMod);
        Debug.Log(Math.Pow(10,multiplier));
        Debug.Log((float)(healthMod * Math.Pow(10,multiplier)));
        return (float)(healthMod * Math.Pow(10,multiplier));
    }
    public float GetDefenseMod()
    {
        return (float)(defenseMod * Math.Pow(10,multiplier));
    }
    public float GetAttackMod()
    {
        return (float)(attackMod * Math.Pow(10,multiplier));
    }
}