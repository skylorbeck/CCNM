using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Global/SaveData")]
[Serializable]
public class SaveData : ScriptableObject
{
    [field: SerializeField] public int credits { get; private set; } = 0;
    [field: SerializeField] public int ego { get; private set; } = 0;
//todo put tracked stats here
    public void AddCredits(int amt)
    {
        amt = (int)(amt * GameManager.Instance.runSettings.creditsMod); //todo replace with inventory multiplier
        credits += amt;
    }

    public void AddEgo(int amt)
    {
        amt = (int)(amt * GameManager.Instance.runSettings.finalMultiplier); //todo replace with inventory multiplier
        ego += amt;
    }
}