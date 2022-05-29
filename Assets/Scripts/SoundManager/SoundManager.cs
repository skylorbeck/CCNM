using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource prefab;
    private ObjectPool<AudioSource> audioPool;

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
        } while (audioSource.isPlaying);
        audioPool.Release(audioSource);
    }
}
