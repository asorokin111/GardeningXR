using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private bool _fadeOnStart = true;
    [SerializeField] private float _duration = 2f;
    [SerializeField] private Color _fadeColor;

    private Renderer _renderer;

    public float Duration { get; }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_fadeOnStart)
            FadeIn();
    }

    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while(timer <= _duration)
        {
            Color newColor = _fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / _duration);

            _renderer.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = _fadeColor;
        newColor2.a = alphaOut;

        _renderer.material.SetColor("_BaseColor", newColor2);
    }
}
