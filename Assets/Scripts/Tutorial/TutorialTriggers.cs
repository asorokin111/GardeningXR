using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggers : MonoBehaviour
{
    protected int _currentState = 0;
    protected Queue<int> _triggerQueue = new();

    protected void Awake()
    {
        StartCoroutine(QueueWaiter());
    }

    protected void CheckStateAndPrintNext(int desiredState)
    {
        if (_currentState == desiredState)
        {
            ++_currentState;
            TriggerPhrase();
        }
    }

    protected void TriggerPhrase()
    {
        _triggerQueue.Enqueue(_currentState);
    }

    protected IEnumerator QueueWaiter()
    {
        while (true)
        {
            yield return new WaitUntil(() => SceneTutorial.instance.nextIsTrigger && _triggerQueue.Count != 0);
            _triggerQueue.Dequeue();
            NextPhrase();
        }
    }

    protected void NextPhrase()
    {
        StartCoroutine(SceneTutorial.instance.PrintNextPhrase());
    }
}
