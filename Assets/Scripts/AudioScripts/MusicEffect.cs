using UnityEngine;

public class MusicEffect : MonoBehaviour, IMusicEffect
{
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayEffect()
    {
        if(!_source.isPlaying)
            _source.Play();
    }


    public void StopEffect()
    {
        if (_source.isPlaying)
            _source.Stop();
    }
}
