using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource _effect;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayEffectOnce(AudioClip clip)
    {
        _effect.PlayOneShot(clip);
    }

    public void PlayEffectRandomOnce(AudioClip[] clips)
    {
        _effect.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
