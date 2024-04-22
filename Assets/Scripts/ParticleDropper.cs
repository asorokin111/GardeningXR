using UnityEngine;

public abstract class ParticleDropper : MonoBehaviour
{
    [SerializeField]
    protected ParticleSystem _particleSystem;
    protected abstract bool CheckTilt();

    protected virtual void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Update()
    {
        var emission = _particleSystem.emission;
        emission.enabled = CheckTilt();
    }
}
