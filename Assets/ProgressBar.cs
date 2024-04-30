using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _fillingSpeed;
    [SerializeField] private Gradient _gradient;

    private Coroutine _currentCoroutine;

    /// <summary>
    /// Expects value between 0 and 1 
    /// </summary>
    /// <param name="value"></param>
    public void SetProgress(float value)
    {
        if (value < 0 || value > 1)
        {
            Debug.Log($"Given value is: {value}. It is not possible.");
            value = Mathf.Clamp01(value);
        }
        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        _currentCoroutine = StartCoroutine(AnimateFilling(value));
    }

    IEnumerator AnimateFilling(float targetValue)
    {

        float time = 0;
        float initialFillValue = _fillImage.fillAmount;
        while(time < 1)
        {
            _fillImage.fillAmount = Mathf.Lerp(initialFillValue, targetValue, time);
            time += Time.deltaTime * _fillingSpeed;
            _fillImage.color = _gradient.Evaluate( 1- _fillImage.fillAmount);
            yield return null;
        }

        _fillImage.fillAmount = targetValue;
    }
}
