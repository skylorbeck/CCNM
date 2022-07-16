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
    [SerializeField] private AudioSource prefab;
    private ObjectPool<AudioSource> audioPool;
    CancellationTokenSource cts;
    
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

        cts = new CancellationTokenSource();
        audioPool = new ObjectPool<AudioSource>(
            () =>
            {
                AudioSource audioSource = Instantiate(prefab, transform);
                return audioSource;
            },
            audioSource =>
            {
                audioSource.gameObject.SetActive(true);
            },
            audioSource =>
            {
                audioSource.gameObject.SetActive(false);
            },
            audioSource => {
                Destroy(audioSource);
            },
            true, 10, 20
        );
    }
    
    public void PlayUiClick()
    {
        PlaySound(uiClick);
    }
    
    public void PlayUiAccept()
    {
        PlaySound(uiAccept);
    }
    
    public void PlayUiDeny()
    {
        PlaySound(uiDeny);
    }
    
    public void PlayUiBack()
    {
        PlaySound(uiBack);
    }
    
    public void PlayWheelClick()
    {
        PlaySound(wheelClick);
    }

    public void PlayEffect(string clip)
    {
        PlaySound(Resources.Load<AudioClip>("Audio/effect/" + clip));
    }
    public void PlayEffect(AudioClip clip)
    {
        PlaySound(clip);
    }
    
    public async void PlaySound(AudioClip clip)
    {
        AudioSource audioSource = audioPool.Get();
        audioSource.clip = clip;
        audioSource.volume = 1;
        audioSource.Play();
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        do
        {
            await Task.Delay(100);
        } while (!cts.Token.IsCancellationRequested && audioSource.isPlaying);
        audioPool.Release(audioSource);
    }

    public void OnDestroy()
    {
        cts.Cancel();
    }
}
