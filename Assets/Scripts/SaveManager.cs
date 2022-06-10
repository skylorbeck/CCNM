using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveManager
{
    //playerdata
    public EquipmentDataContainer[] equippedCards;
    public bool[] equippedSlots;
    public EquipmentList[] equipmentDataContainers;
    
    public void Save()
    {
        equippedCards = GameManager.Instance.battlefield.player.equippedCards;
        equippedSlots = GameManager.Instance.battlefield.player.equippedSlots;
        equipmentDataContainers = GameManager.Instance.battlefield.player.equipmentDataContainers;
        
        FileStream dataStream = new FileStream(Application.persistentDataPath + "/save.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(dataStream, this);
        dataStream.Close();
    }
}
//todo create serializable versions of scriptableobjects that you can request from the non-serializable ones