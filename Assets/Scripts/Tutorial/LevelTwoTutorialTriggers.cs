using Gardening;
using System.Collections;
using UnityEngine;

public class LevelTwoTutorialTriggers : MonoBehaviour
{
    private enum TutorialStates
    {
        InitialState = 0,
        FoamGrabbed = 1,
        FlowerPlanted = 2,
    }

    private int _currentState = (int)TutorialStates.InitialState;

    private void OnEnable()
    {
        Sponge.OnSpongeGrabbed += FoamGrabbedEventHandler;
        SpongeInsert.OnFlowerPlanted += FlowerPlantedEventHandler;
    }

    private void OnDisable()
    {
        SpongeInsert.OnFlowerPlanted -= FlowerPlantedEventHandler;
        Sponge.OnSpongeGrabbed -= FoamGrabbedEventHandler;
    }

    private void FoamGrabbedEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.FoamGrabbed - 1);
    }

    private void FlowerPlantedEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.FlowerPlanted - 1);
        GameManager.Instance.ChangeGameState(GameState.Win);
    }

    private void CheckStateAndPrintNext(int desiredState)
    {
        if (_currentState == desiredState)
        {
            ++_currentState;
            StartCoroutine(TriggerPhrase());
        }
    }

    private IEnumerator TriggerPhrase()
    {
        while (!SceneTutorial.instance.nextIsTrigger)
        {
            yield return null;
        }
        NextPhrase();
    }

    private void NextPhrase()
    {
        StartCoroutine(SceneTutorial.instance.PrintNextPhrase());
    }
}
