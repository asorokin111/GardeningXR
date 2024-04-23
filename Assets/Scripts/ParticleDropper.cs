using UnityEngine;

public abstract class ParticleDropper : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem _particleSystem;
    protected IMusicEffect _musicEffect;

    protected abstract bool CheckTilt();

    protected virtual void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _musicEffect = GetComponent<IMusicEffect>();
    }

    protected virtual void Update()
    {
        var emission = _particleSystem.emission;
        emission.enabled = CheckTilt();

        if (_musicEffect != null)
            PlayMusicEffectWhenEnabled();
    }

    protected virtual void PlayMusicEffectWhenEnabled()
    {
        if (_particleSystem.emission.enabled)
        {
            _musicEffect.PlayEffect();
        }
        else
        {
            _musicEffect.StopEffect();
        }
    }
}
