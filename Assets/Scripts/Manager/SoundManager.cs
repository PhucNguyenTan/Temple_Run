using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource _effect;
    [SerializeField] AudioSource _music;
    [SerializeField] AudioClip[] _clips;
    [SerializeField] AudioClip[] _endClips;
    [SerializeField] float _gameRunVolMul;
    [SerializeField] float _pauseVolMul;

    [SerializeField] Slider _sliderMusic;
    [SerializeField] Slider _sliderEffect;

    float _currentMusicMultiplier;
    int randomNum;
    float _currentMusicSlider = 1f;
    float _currentEffectSlider = 1f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _currentEffectSlider = GetMusicSaved("Effect_volume");
        _currentMusicSlider = GetMusicSaved("Music_volume");
    }

    float GetMusicSaved(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return 1f;
        }
        return PlayerPrefs.GetFloat(key);
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
                _effect.volume = _currentEffectSlider;
                _music.volume = _currentMusicSlider * _gameRunVolMul;
                _currentMusicMultiplier = _gameRunVolMul;
                _music.clip = _clips[randomNum];
                _music.Play();
                break;
            case GameManager.GameState.Run:
                _music.volume = _currentMusicSlider * _gameRunVolMul;
                _currentMusicMultiplier = _gameRunVolMul;
                break;
            case GameManager.GameState.Pause:
                _music.volume = _currentMusicSlider * _pauseVolMul;
                _currentMusicMultiplier = _pauseVolMul;
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

    public void ChangeSoundEffectVolume(float volume)
    {
        _effect.volume = volume;
        _currentEffectSlider = volume;
        PlayerPrefs.SetFloat("Effect_volume", volume);
    }

    public void ChangeMusicVolume(float volume)
    {
        _music.volume = volume * _currentMusicMultiplier;
        _currentMusicSlider = volume;
        PlayerPrefs.SetFloat("Music_volume", volume);
    }

    public void UpdateOptionUI()
    {
        _sliderEffect.value = _currentEffectSlider;
        _sliderMusic.value = _currentMusicSlider;
    }
}
