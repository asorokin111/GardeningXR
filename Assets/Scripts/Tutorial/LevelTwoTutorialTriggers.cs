using Gardening;
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
        Debug.Log("Sponge event handler called");
        CheckStateAndPrintNext((int)TutorialStates.FoamGrabbed - 1);
    }

    private void FlowerPlantedEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.FlowerPlanted - 1);
        GameManager.Instance.ChangeGameState(GameState.Win);
    }

    private void CheckStateAndPrintNext(int desiredState)
    {
        Debug.Log($"Current state: {_currentState}, Needed state: {desiredState}");
        if (_currentState == desiredState)
        {
            ++_currentState;
            NextPhrase();
        }
    }

    private void NextPhrase()
    {
        StartCoroutine(SceneTutorial.instance.PrintNextPhrase());
    }
}
