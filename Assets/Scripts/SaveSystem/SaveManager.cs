using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SaveSystem;
using UnityEngine;

[Serializable]
public class SaveManager
{
    int version = 1;
    
    public void SaveMeta()
    {
        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream dataStream = new FileStream(Application.persistentDataPath + "/meta.dat", FileMode.Create);
        bf.Serialize(dataStream, new SavablePlayerBrain(GameManager.Instance.metaPlayer));
        
        dataStream.Close();
    }

    public void SaveRun()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream dataStream = new FileStream(Application.persistentDataPath + "/battlefield.dat", FileMode.Create);
        bf.Serialize(dataStream, new SavableBattlefield(GameManager.Instance.battlefield));
        
        dataStream.Close();
    }

    public void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/meta.dat"))
        {
            FileStream dataStream = new FileStream(Application.persistentDataPath + "/meta.dat", FileMode.Open);
            GameManager.Instance.metaPlayer.InsertSaveFile((SavablePlayerBrain)bf.Deserialize(dataStream));
            dataStream.Close();
        }
        else
        {
            GameManager.Instance.metaPlayer.ClearPlayerObject();
            SaveMeta();
        }

        if (File.Exists(Application.persistentDataPath + "/battlefield.dat"))
        {
            FileStream dataStream = new FileStream(Application.persistentDataPath + "/battlefield.dat", FileMode.Open);
            GameManager.Instance.battlefield.InsertSave((SavableBattlefield)bf.Deserialize(dataStream));
            dataStream.Close();
        }
        else
        {
            GameManager.Instance.battlefield.ClearBattlefield();
            SaveRun();
        }
    }

    public void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + "/meta.dat");
        File.Delete(Application.persistentDataPath + "/battlefield.dat");
        GameManager.Instance.battlefield.Reset();
        GameManager.Instance.metaPlayer.ClearPlayerObject();
        SaveMeta();
        SaveRun();
    }
}