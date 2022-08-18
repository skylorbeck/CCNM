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
    [SerializeField] private AudioClip queuedTrack = null;
    public bool isPlaying => track.isPlaying;
    public float volume => track.volume;
    public float time => track.time;
    public float duration => track.clip.length;
    public float pitch => track.pitch;
    public float panStereo => track.panStereo;
    public bool loop => track.loop;
    public bool mute => track.mute;
    
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

    public void Update()
    {
        if (track.isPlaying && track.time >= track.clip.length)
        {
            track.Stop();
        }

        if (!track.isPlaying)
        {
            track.clip = queuedTrack;
            queuedTrack = null;
            track.Play();
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

    public async void PlayTrack(AudioClip clip,bool startOver = false)
    {
        if (track.clip != null)
        {
            if (track.clip.Equals(clip))
            {
                if (startOver)
                {
                    track.time = 0;
                    track.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
                }
                else
                {
                    //song already playing
                    return;
                }
            }
            else
            {
                do
                {
                    track.volume = Mathf.Lerp(track.volume, -0.5f, Time.deltaTime);
                    await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
                } while (track.isPlaying && track.volume > 0);
            }
        }

        track.clip = clip;
        track.volume = 0;
        track.Play();
        do
        {
            track.volume = Mathf.Lerp(track.volume, PlayerPrefs.GetFloat("MusicVolume", 1f)+0.5f, Time.deltaTime);//todo fix this. this is why the music starts for a frame 
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        } while (track.volume < PlayerPrefs.GetFloat("MusicVolume", 1f)*0.9f);
        track.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }
    public async void PlayTrackSimple(AudioClip clip,bool loop = true)
    {
        track.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        track.clip = clip;
        track.loop = loop;
        track.Play();
       
    }

    public void StopTrackSimple()
    {
        track.Stop();
    }
    
    public async void StopTrack(){
        do
        {
            track.volume = Mathf.Lerp(track.volume, -0.5f, Time.deltaTime);
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
           
        } while (track.isPlaying && track.volume > 0 );
        
        track.Stop();
    }
   
    
    public void PauseTrack()
    {
        track.Pause();
    }
    public void UnPauseTrack()
    {
        track.UnPause();
    }
     
    public void QueueTrack(AudioClip clip)
    {
        queuedTrack = clip;
    }
   

    public void OnDestroy()
    {
    }

    public void PlayTrack(string trackTitle, bool startOver = false)
    {
        PlayTrack(GameManager.Instance.musicRegistry.GetMusic(trackTitle), startOver);
    }
    public void PlayTrack(int trackIndex, bool startOver = false)
    {
        PlayTrack(GameManager.Instance.musicRegistry.GetMusic(trackIndex), startOver);
    }
    
    public void PlayTrackSimple(string trackTitle, bool loop = true)
    {
        PlayTrackSimple(GameManager.Instance.musicRegistry.GetMusic(trackTitle), loop);
    }

    public void PlayTrackSimple(int trackIndex, bool loop = true)
    {
        PlayTrackSimple(GameManager.Instance.musicRegistry.GetMusic(trackIndex),loop);
    }
}
