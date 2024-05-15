using Gardening;

public class LevelTwoTutorialTriggers : TutorialTriggers
{
    private enum TutorialStates
    {
        InitialState = 0,
        FoamGrabbed = 1,
        FlowerPlanted = 2,
    }

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
}
