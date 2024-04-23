using UnityEngine;

namespace Gardening
{
    public class FlowerPot : MonoBehaviour
    {
        public Plant plant { get; private set; }

        public delegate void SeedPlantedAction();
        public static SeedPlantedAction OnSeedPlanted;

        [SerializeField] private Transform _plantRootsPosition;
        private bool _isSeedPlanted = false;

        private GroundFilling _groundFillerScript;
        private float _timeFillInactive = 0.0f;
        private float _timePlantGrowthInactive = 0.0f;

        private void Start()
        {
            _groundFillerScript = GetComponent<GroundFilling>();
        }

        private void Update()
        {
            if (_groundFillerScript.isCurrentlyFilling)
            {
                _timeFillInactive += Time.deltaTime;
                if (_timeFillInactive > 0.2f)
                {
                    _groundFillerScript.StopFillingAndCoroutine();
                }
            }

            if (plant == null || !plant.IsCurrentlyGrowing) return;
            _timePlantGrowthInactive += Time.deltaTime;
            if (_timePlantGrowthInactive > 0.2f)
            {
                plant.StopPlantGrowth();
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Dirt"))
            {
                _timeFillInactive = 0f;
                if (!_groundFillerScript.isCurrentlyFilling)
                    _groundFillerScript.StartFillingAndCoroutine();
            }

            if (!_groundFillerScript.isFilled)
                return;

            if (other.CompareTag("Seed") && !_isSeedPlanted)
            {
                if (other.transform.root.TryGetComponent<SeedPacket>(out var seedPacket))
                {
                    Quaternion sproutRotation = Quaternion.identity;
                    sproutRotation.eulerAngles = new Vector3(-90f, 0f, 0f);
                    plant = Instantiate(seedPacket.associatedPlant, _plantRootsPosition.position, sproutRotation);
                    plant.PlantThePlant();
                    plant.transform.parent = transform;
                    _isSeedPlanted = true;
                    OnSeedPlanted?.Invoke();
                }
            }

            if (!_isSeedPlanted)
                return;

            if (other.CompareTag("Water") && !plant.IsCurrentlyGrowing)
            {
                _timePlantGrowthInactive = 0f;
                plant.StartPlantGrowth();
            }
        }
    }
}


