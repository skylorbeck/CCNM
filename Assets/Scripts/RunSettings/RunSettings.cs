using UnityEngine;

[CreateAssetMenu(fileName = "RunSettings", menuName = "Global/RunSettings")]
public class RunSettings : ScriptableObject
{
    public float health;
    public float defense;
    public float attack;
    public float xp;
    public float credits;
    public float multiplier;
    [Header("Modifiers")]
    public double healthMod;
    public double defenseMod;
    public double attackMod;
    public double xpMod;
    public double creditsMod;
    public double finalMultiplier;    

}