using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RunSettings", menuName = "Global/RunSettings")]
public class RunSettings : ScriptableObject//todo make this save
{
    public float health;
    public float shield;
    public float armor;
    public float attack;
    public float dodge;
    public float critChance;
    public float critDamage;
    
    public float multiplier;//difficulty multiplier
    
    [Header("Modifiers")]
    public double healthMod;
    public double shieldMod;
    public double armorMod;
    public double attackMod;
    public double dodgeMod;
    public double critChanceMod;
    public double critDamageMod;
    
    public double finalMultiplier;

    public float GetHealthMod()
    {
        return (float)(healthMod * Math.Pow(10,multiplier));
    }
    public float GetShieldMod()
    {
        return (float)(shieldMod * Math.Pow(10,multiplier));
    }
    public float GetAttackMod()
    {
        return (float)(attackMod * Math.Pow(10,multiplier));
    }
    public float GetArmorMod()
    {
        return (float)(armorMod * Math.Pow(10,multiplier));
    }
    
    public float GetDodgeMod()
    {
        return (float)(dodgeMod * Math.Pow(10,multiplier));
    }
    public float GetCritChanceMod()
    {
        return (float)(critChanceMod * Math.Pow(10,multiplier));
    }
    public float GetCritDamageMod()
    {
        return (float)(critDamageMod * Math.Pow(10,multiplier));
    }

    public void Reset()
    {
        armorMod = 1;
        attackMod = 1;
        healthMod = 1;
        shieldMod = 1;
        dodgeMod = 1;
        critChanceMod = 1;
        critDamageMod = 1;
        health = 1;
        shield = 1;
        armor = 1;
        attack = 1;
        dodge = 1;
        critChance = 1;
        critDamage = 1;
        multiplier = 0;
    }
}