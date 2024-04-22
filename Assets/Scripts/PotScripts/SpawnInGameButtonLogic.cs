using UnityEngine;

public class SpawnInGameButtonLogic : MonoBehaviour, IIngameButtonLogic
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform _placeToSpawn;

    public void TriggerAction()
    {
        Instantiate(prefab, _placeToSpawn.position, prefab.transform.rotation);
    }
}
