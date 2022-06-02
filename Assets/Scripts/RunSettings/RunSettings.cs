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
        return (float)(healthMod + Math.Pow(multiplier,3));
    }
    public float GetDefenseMod()
    {
        return (float)(defenseMod + Math.Pow(multiplier,3));
    }
    public float GetAttackMod()
    {
        return (float)(attackMod + Math.Pow(multiplier,3));
    }
}