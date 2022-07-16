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
    
    [SerializeField] private AudioSource currentTrack;
    [SerializeField] private AudioClip defaultTrack;
    [SerializeField] private AudioClip defaultTrack2;
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
        currentTrack.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
    }
    
    public void UpdateVolume(float volume)
    {
        currentTrack.volume = volume;
    }
    
    public void PlayDefaultTrack()
    {
        PlayTrack(defaultTrack);
    }
    
    public void PlayDefaultTrack2()
    {
        PlayTrack(defaultTrack2);
    }

    public async void PlayTrack(AudioClip clip)
    {
        if (currentTrack.clip.Equals(clip))
        {
            return;
        }
        do
        {
            currentTrack.volume = Mathf.Lerp(currentTrack.volume, -0.5f, Time.deltaTime);
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        } while (currentTrack.isPlaying && currentTrack.volume > 0);

        currentTrack.clip = clip;
        currentTrack.volume =PlayerPrefs.GetFloat("MusicVolume", 1f); //*volumeParam
        currentTrack.Play();
    }


    public void OnDestroy()
    {
    }
}
