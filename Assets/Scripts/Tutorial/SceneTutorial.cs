using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTutorial : MonoBehaviour
{
    public static SceneTutorial instance;

    public bool playOnStart;

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
    }

    private void Start()
    {
        if (_isInitialized) return;

        for (int i = 0; i < _tutorialPhrasesList.Count; i++)
        {
            _tutorialPhrases.Enqueue(_tutorialPhrasesList[i]);
        }

        _isInitialized = true;
        if (playOnStart)
        {
            _waitNextPhrase = new WaitForSeconds(_timeUntilNextPhrase);
            StartCoroutine(PlayTutorial());
        }
    }

    private IEnumerator PlayTutorial()
    {
        int phrasesCount = _tutorialPhrases.Count;
        for (int i = 0; i < phrasesCount; i++)
        {
            StartCoroutine(PrintNextPhrase());

            yield return _waitNextPhrase;
        }
    }

    public IEnumerator PrintNextPhrase()
    {
        var nextPhrase = _tutorialPhrases.Dequeue();
        _text.text = "";
        var waitUntilNextChar = new WaitForSeconds(nextPhrase.phraseTypingDuration / nextPhrase.text.Length);
        foreach (char c in nextPhrase.text)
        {
            yield return waitUntilNextChar;
            _text.text += c;
        }
    }
}
