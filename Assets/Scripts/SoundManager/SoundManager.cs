using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource prefab;
    private ObjectPool<AudioSource> audioPool;
    CancellationTokenSource cts;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
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
        audioSource.Play();
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
