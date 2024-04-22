using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTutorial : MonoBehaviour
{
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

    private void Start()
    {
        if (_isInitialized) return;

        for (int i = 0; i < _tutorialPhrasesList.Count; i++)
        {
            _tutorialPhrases.Enqueue(_tutorialPhrasesList[i]);
        }

        _isInitialized = true;
        _waitNextPhrase = new WaitForSeconds(_timeUntilNextPhrase);
        StartCoroutine(PlayTutorial());
    }

    IEnumerator PlayTutorial()
    {
        int phrasesCount = _tutorialPhrases.Count;
        for (int i = 0; i < phrasesCount; i++)
        {
            TutorialPhrase nextPhrase = _tutorialPhrases.Dequeue();
            _text.text = "";
            WaitForSeconds waitUntilNextChar = new WaitForSeconds(nextPhrase.phraseTypingDuration / nextPhrase.text.Length);

            foreach (char c in nextPhrase.text)
            {
                yield return waitUntilNextChar;
                _text.text += c;
            }

            yield return _waitNextPhrase;
        }
    }
}
