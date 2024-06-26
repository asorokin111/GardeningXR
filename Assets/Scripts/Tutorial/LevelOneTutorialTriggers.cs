using Gardening;

public class LevelOneTutorialTriggers : TutorialTriggers
{
    private enum TutorialStates
    {
        InitialState = 0,
        PotFilled = 1,
        SeedPlanted = 2,
        PlantGrown = 3,
    }

    private void OnEnable()
    {
        GroundFilling.OnPotFilled += PotFilledEventHandler;
        FlowerPot.OnSeedPlanted += SeedPlantedEventHandler;
        Plant.OnPlantGrown += PlantGrownEventHandler;
    }

    private void OnDisable()
    {
        Plant.OnPlantGrown -= PlantGrownEventHandler;
        FlowerPot.OnSeedPlanted -= SeedPlantedEventHandler;
        GroundFilling.OnPotFilled -= PotFilledEventHandler;
    }

    private void PotFilledEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.PotFilled - 1);
    }

    private void SeedPlantedEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.SeedPlanted - 1);
    }

    private void PlantGrownEventHandler()
    {
        CheckStateAndPrintNext((int)TutorialStates.PlantGrown - 1);
        GameManager.Instance.ChangeGameState(GameState.Win);
    }
}
