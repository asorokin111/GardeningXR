using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTutorial : MonoBehaviour
{
    public static SceneTutorial instance;

    [Header("UI")]
    [SerializeField]
    private TMP_Text _text;

    [Header("Tutorial contents")]
    [SerializeField]
    private List<TutorialPhrase> _tutorialPhrasesList = new List<TutorialPhrase>();
    private Queue<TutorialPhrase> _tutorialPhrases = new Queue<TutorialPhrase>();

    [Header("Tutorial parameters")]
    [SerializeField]
    private float _timeUntilNextPhrase;
    private WaitForSeconds _waitNextPhrase;

    private bool _isInitialized = false;
    private bool _isCurrentlyPrintingPhrase = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        _text = GameObject.Find("Tutorial Text").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if (_isInitialized) return;

        for (int i = 0; i < _tutorialPhrasesList.Count; i++)
        {
            _tutorialPhrases.Enqueue(_tutorialPhrasesList[i]);
        }

        _isInitialized = true;
        _waitNextPhrase = new WaitForSeconds(_timeUntilNextPhrase);
        PlayTutorial();
    }

    private void PlayTutorial()
    {
        StartCoroutine(PrintFillerPhrases());
    }

    private IEnumerator PrintFillerPhrases()
    {
        while (true)
        {
            if (!_isCurrentlyPrintingPhrase && _tutorialPhrases.Peek().isFillerText)
            {
                StartCoroutine(PrintNextPhrase());
            }
            yield return _waitNextPhrase;
        }
    }

    public IEnumerator PrintNextPhrase()
    {
        if (_isCurrentlyPrintingPhrase)
        {
            yield return null;
        }
        _isCurrentlyPrintingPhrase = true;
        var nextPhrase = _tutorialPhrases.Dequeue();
        _text.text = "";
        var waitUntilNextChar = new WaitForSeconds(nextPhrase.phraseTypingDuration / nextPhrase.text.Length);
        foreach (char c in nextPhrase.text)
        {
            yield return waitUntilNextChar;
            _text.text += c;
        }
        _isCurrentlyPrintingPhrase = false;
    }
}
