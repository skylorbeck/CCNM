using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private AudioSource effectSource;
    
    [SerializeField] private AudioClip uiClick;
    [SerializeField] private AudioClip uiAccept;
    [SerializeField] private AudioClip uiDeny;
    [SerializeField] private AudioClip uiBack;
    [SerializeField] private AudioClip wheelClick;
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
    
    public void PlayUiClick(float volumeScale = 1f)
    {
        PlaySound(uiClick,volumeScale);
    }
    
    public void PlayUiAccept(float volumeScale = 1f)
    {
        PlaySound(uiAccept,volumeScale);
    }
    
    public void PlayUiDeny(float volumeScale = 1f)
    {
        PlaySound(uiDeny,volumeScale);
    }
    
    public void PlayUiBack(float volumeScale = 1f)
    {
        PlaySound(uiBack,volumeScale);
    }
    
    public void PlayWheelClick(float volumeScale = 1f)
    {
        PlaySound(wheelClick,volumeScale);
    }

    public void PlayEffect(string clip, float volumeScale = 1f)
    {
        PlayEffect(Resources.Load<AudioClip>("Audio/effect/" + clip),volumeScale);
    }
    public void PlayEffect(AudioClip clip, float volumeScale = 1f)
    {
        PlaySound(clip, volumeScale);
    }
    
    public void PlaySound(AudioClip clip, float volumeScale = 1f)
    {
        effectSource.volume = Random.Range(0.8f, 1f)* PlayerPrefs.GetFloat("EffectVolume", 1f) * volumeScale;
        effectSource.pitch = Random.Range(0.9f, 1.1f);

        effectSource.PlayOneShot(clip);
    }

    public void OnDestroy()
    {
        
    }
}
