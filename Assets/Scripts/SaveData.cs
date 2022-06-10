using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Global/SaveData")]
[Serializable]
public class SaveData : ScriptableObject
{ [field:SerializeField] public int credits { get;private set; } = 0;

    public void AddCredits(int amt)
    {
        amt = (int)(amt * GameManager.Instance.runSettings.creditsMod);//todo replace with inventory multipliers
        credits += amt;
    }
}
