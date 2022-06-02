using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Global/SaveData")]
public class SaveData : ScriptableObject
{
    [field:SerializeField] public RunSettings runSettings { get;private set; }
    [field:SerializeField] public int credits { get;private set; } = 0;

    public void AddCredits(int amt)
    {
        amt = (int)(amt * runSettings.creditsMod);//todo replace with inventory multipliers
        credits += amt;
    }
}
