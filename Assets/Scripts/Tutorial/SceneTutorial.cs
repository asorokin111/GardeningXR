using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTutorial : MonoBehaviour
{
    public static SceneTutorial instance;

    public bool isCurrentlyPrintingPhrase = false;
    public bool nextIsTrigger = false;
    [Header("UI")]
    [SerializeField]
    private TMP_Text _text;

    [Header("Tutorial contents")]
    [SerializeField]
    private List<TutorialPhrase> _tutorialPhrasesList = new();
    private Queue<TutorialPhrase> _tutorialPhrases = new();

    [Header("Tutorial parameters")]
    [SerializeField]
    private float _timeUntilNextPhrase;
    [SerializeField]
    [Tooltip("How many seconds to wait for each character typed")]
    private float _waitPerCharacter;

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
        for (int i = 0; i < _tutorialPhrasesList.Count; i++)
        {
            _tutorialPhrases.Enqueue(_tutorialPhrasesList[i]);
        }
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
            if (_tutorialPhrases.Peek().isFillerText)
            {
                nextIsTrigger = false;
                StartCoroutine(PrintNextPhrase());
            }
            else
            {
                nextIsTrigger = true;
            }
            yield return new WaitUntil(() => isCurrentlyPrintingPhrase == false);
            yield return new WaitForSeconds(_timeUntilNextPhrase);
        }
    }

    public IEnumerator PrintNextPhrase()
    {
        yield return new WaitUntil(() => !isCurrentlyPrintingPhrase);
        isCurrentlyPrintingPhrase = true;
        var nextPhrase = _tutorialPhrases.Dequeue();
        _text.text = "";
        var waitUntilNextChar = new WaitForSeconds(_waitPerCharacter);
        foreach (char c in nextPhrase.text)
        {
            yield return waitUntilNextChar;
            _text.text += c;
        }
        isCurrentlyPrintingPhrase = false;
    }
}
