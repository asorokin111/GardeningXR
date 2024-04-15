using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneTutorial : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TMP_Text _text;
    [Header ("Tutorial contents")]
    [SerializeField] 
    private List<TutorialPhrase> _tutorialPhrasesList = new List<TutorialPhrase>();
    private Queue<TutorialPhrase> _tutorialPhrases = new Queue<TutorialPhrase>();

    private bool _isInitialized = false;

    public void StartTutorial()
    {
        if (_isInitialized)
            return;

        for (int i = _tutorialPhrasesList.Count; i >= 0; i--)
        {
            _tutorialPhrases.Enqueue(_tutorialPhrasesList[i]);
        }
        _isInitialized = true;
        StartCoroutine(PlayNextPhrase());

    }
    IEnumerator PlayNextPhrase()
    {
        TutorialPhrase nextPhrase = _tutorialPhrases.Dequeue();
        _text.text = nextPhrase.text;
        yield return new WaitForSeconds(nextPhrase.phraseDuraction);

    }

}
