using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [Header("Sliders")]
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _SFXSlider;

    public const string MIXER_MUSIC_VOLUME = "MusicVolume";
    public const string MIXER_SFX_VOLUME = "SFXVolume";

    private void Awake()
    {
        LoadVolume();
        _musicSlider.onValueChanged.AddListener(SetMusicValue);
        _SFXSlider.onValueChanged.AddListener(SetSFXValue);
    }
    private void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_VOLUME_KEY, 1f);
        _musicSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_VOLUME_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_VOLUME_KEY, _musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_VOLUME_KEY, _SFXSlider.value);
    }
    private void LoadVolume() //Volume Saved in HS_VolumeSettings
    {
        float musicVolume = PlayerPrefs.GetFloat(MIXER_MUSIC_VOLUME, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(MIXER_SFX_VOLUME, 1f);

        audioMixer.SetFloat(MIXER_MUSIC_VOLUME, musicVolume);
        audioMixer.SetFloat(MIXER_SFX_VOLUME, sfxVolume);
    }
    private void SetMusicValue(float value)
    {
        audioMixer.SetFloat(MIXER_MUSIC_VOLUME, Mathf.Log10(value) * 20);
    }
    private void SetSFXValue(float value)
    {
        audioMixer.SetFloat(MIXER_SFX_VOLUME, Mathf.Log10(value) * 20);
    }
}
