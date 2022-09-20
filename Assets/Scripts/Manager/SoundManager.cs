using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource _effect;
    [SerializeField] AudioSource _music;
    [SerializeField] AudioClip[] _clips;
    [SerializeField] AudioClip[] _endClips;
    [SerializeField] float _runVol;
    [SerializeField] float _pauseVol;
    int randomNum;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        GameManager.OnStateChange += GameManagerOnStateChanged;

    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.CountDown:
                randomNum = 0;//Random.Range(0, _clips.Length);
                _music.loop = true;
                _music.volume = _runVol;
                _music.clip = _clips[randomNum];
                _music.Play();
                break;
            case GameManager.GameState.Run:
                _music.volume = _runVol;
                break;
            case GameManager.GameState.Pause:
                _music.volume = _pauseVol;
                break;
            case GameManager.GameState.End:
                _music.loop = false;
                _music.clip = _endClips[randomNum];
                _music.Play();
                break;
            default:
                throw new System.Exception("Something wrong, Patrick?");

        }
        //throw new NotImplementedException();
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
