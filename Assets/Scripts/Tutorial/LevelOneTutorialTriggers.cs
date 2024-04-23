using Gardening;
using UnityEngine;

public class LevelOneTutorialTriggers : MonoBehaviour
{
    private enum TutorialStates
    {
        InitialState = 0,
        SoilBagPickedUp = 1,
        PotFilled = 2,
        SeedBagPickedUp = 3,
        SeedPlanted = 4,
        WateringCanPickedUp = 5,
        PlantGrown = 6,
    }

    private int _currentState = (int)TutorialStates.InitialState;

    private void OnEnable()
    {
        if (SceneTutorial.instance.playOnStart)
        {
            Debug.LogWarning("PlayOnStart enabled for tutorial in a scene with tutorial triggers. Disabling triggers");
            return;
        }
        WateringCan.OnWateringCanFirstPickedUp += WateringCanPickedUpEventHandler;
        DirtPacket.OnDirtPacketFirstPickedUp += SoilBagPickedUpEventHandler;
        GroundFilling.OnPotFilled += PotFilledEventHandler;
        FlowerPot.OnSeedPlanted += SeedPlantedEventHandler;
        SeedPacket.OnSeedPacketFirstPickedUp += SeedBagPickedUpEventHandler;
        Plant.OnPlantGrown += PlantGrownEventHandler;
    }

    private void OnDisable()
    {
        if (SceneTutorial.instance.playOnStart) return;
        Plant.OnPlantGrown -= PlantGrownEventHandler;
        SeedPacket.OnSeedPacketFirstPickedUp -= SeedBagPickedUpEventHandler;
        FlowerPot.OnSeedPlanted -= SeedPlantedEventHandler;
        GroundFilling.OnPotFilled -= PotFilledEventHandler;
        DirtPacket.OnDirtPacketFirstPickedUp -= SoilBagPickedUpEventHandler;
        WateringCan.OnWateringCanFirstPickedUp -= WateringCanPickedUpEventHandler;
    }

    private void SoilBagPickedUpEventHandler()
    {
        //NextPhrase();
        CheckStateAndPrintNext((int)TutorialStates.SoilBagPickedUp - 1);
    }

    private void PotFilledEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.PotFilled - 1);
    }

    private void SeedBagPickedUpEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.SeedBagPickedUp - 1);
    }

    private void SeedPlantedEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.SeedPlanted - 1);
    }

    private void WateringCanPickedUpEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.WateringCanPickedUp - 1);
    }

    private void PlantGrownEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.PlantGrown - 1);
    }

    private void CheckStateAndPrintNext(int desiredState)
    {
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
