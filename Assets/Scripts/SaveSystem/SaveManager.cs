using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveManager
{
    private SavablePlayerBrain saveFile;

    public void Save()
    {
        saveFile = new SavablePlayerBrain(GameManager.Instance.metaPlayer);
        FileStream dataStream = new FileStream(Application.persistentDataPath + "/save.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(dataStream, saveFile);
        dataStream.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            FileStream dataStream = new FileStream(Application.persistentDataPath + "/save.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            saveFile = (SavablePlayerBrain)bf.Deserialize(dataStream);
            GameManager.Instance.metaPlayer.InsertSaveFile(saveFile);
            dataStream.Close();
        }
        else
        {
            GameManager.Instance.metaPlayer.ClearPlayerObject();
            Save();
        }
    }
}