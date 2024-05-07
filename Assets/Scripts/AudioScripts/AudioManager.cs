using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get { return _instance; } }

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioData _audioData;

    private static AudioManager _instance;


    private Dictionary<string, AudioClip> _audios = new();

    public const string MUSIC_VOLUME_KEY = "musicVolume";
    public const string SFX_VOLUME_KEY = "sfxVolume";


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        foreach (var obj in _audioData.AudioObjects)
        {
            var processedName = obj.audioName.ToLower().Trim(); // clear (no whitespaces, no uppercase) version of AudioCLip name
            if (_audios.ContainsKey(processedName))
                continue;
            _audios.Add(processedName, obj.audioClip);
        }
    }

    /// <summary>
    /// NOT case or whitespaces sensitive param
    /// </summary>
    private AudioClip GetAudio(string audioName)
    {
        var processedName = audioName.ToLower().Trim(); // clear (no whitespaces, no uppercase) version of AudioClip name
        return _audios.GetValueOrDefault(processedName);
    }

    public enum SoundType
    {
        SFX,
        Music,
    }

    public void PlaySound(string audioName, SoundType soundType)
    {
        AudioSource source = soundType switch
        {
            SoundType.SFX => _sfxSource,
            SoundType.Music => _musicSource,
            _ => _sfxSource,
        };
        source.clip = GetAudio(audioName);
        if (source.clip != null)
            source.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void ResumeMusic()
    {
        _musicSource.UnPause();
    }
}