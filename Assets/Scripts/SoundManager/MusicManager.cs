using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    [SerializeField] private AudioSource track;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void UpdateVolume()
    {
        track.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }
    
    public void UpdateVolume(float volume)
    {
        track.volume = volume;
    }

    public async void PlayTrack(AudioClip clip)
    {
        if (track.clip != null)
        {
            if (track.clip.Equals(clip))
            {
                return;
            }

            do
            {
                track.volume = Mathf.Lerp(track.volume, -0.5f, Time.deltaTime);
                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
            } while (track.isPlaying && track.volume > 0);
        }

        track.clip = clip;
        track.volume = 0;
        track.Play();
        do
        {
            track.volume = Mathf.Lerp(track.volume, PlayerPrefs.GetFloat("MusicVolume", 1f)+0.5f, Time.deltaTime);
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        } while (track.volume < PlayerPrefs.GetFloat("MusicVolume", 1f)*0.9f);
        track.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }


    public void OnDestroy()
    {
    }

    public void PlayTrack(string trackTitle)
    {
        PlayTrack(GameManager.Instance.musicRegistry.GetMusic(trackTitle));
    }
    public void PlayTrack(int trackIndex)
    {
        PlayTrack(GameManager.Instance.musicRegistry.GetMusic(trackIndex));
    }
}
