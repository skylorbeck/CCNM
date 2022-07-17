using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicRegistry", menuName = "Data/MusicRegistry")]
public class MusicRegistry : ScriptableObject, ISerializationCallbackReceiver
{
    public AudioClip[] musicTracks;
    public List<string> keys = new List<string>();
    public List<int> values = new List<int>();
    public Dictionary<string, int> MusicDictionary = new Dictionary<string, int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        for (var index = 0; index < musicTracks.Length; index++)
        {
            values.Add(index);
            keys.Add(musicTracks[index].name);
        }
    }

    public void OnAfterDeserialize()
    {
        MusicDictionary = new Dictionary<string, int>();

        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
            MusicDictionary.Add(keys[i], values[i]);

    }

    public AudioClip GetMusic(string MusicTitle)
    {
        if (MusicDictionary.ContainsKey(MusicTitle))
        {
            return musicTracks[MusicDictionary[MusicTitle]];
        }
        else
        {
            Debug.LogError("Music not found in registry: " + MusicTitle);
            return null;
        }
    }

    public AudioClip GetMusic(int MusicIndex)
    {
        if (MusicIndex < musicTracks.Length)
        {
            return musicTracks[MusicIndex];
        }
        else
        {
            Debug.LogError("Music not found in registry: " + MusicIndex);
            return null;
        }
    }

    public AudioClip GetRandomMusic()
    {
        return musicTracks[UnityEngine.Random.Range(0, musicTracks.Length)];
    }

    public int GetMusicIndex(string MusicTitle)
    {
        if (MusicDictionary.ContainsKey(MusicTitle))
        {
            return MusicDictionary[MusicTitle];
        }
        else
        {
            Debug.LogError("Music not found in registry: " + MusicTitle);
            return -1;
        }
    }
}