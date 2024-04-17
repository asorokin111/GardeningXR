using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gardening
{
    public class Plant : MonoBehaviour
    {
        public bool IsCurrentlyGrowing { get; private set; }

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private float _minGrowValue;
        [SerializeField] private float _maxGrowValue;

        [SerializeField] private int _growthTime;
        [SerializeField] private float _refreshRate;

        private List<Material> _growMaterials = new List<Material>();

        private bool _isGrown;
        private int index = 0;


        /// <summary>
        /// Sets flower to the default values
        /// </summary>
        public void PlantThePlant()
        {
            _growMaterials = GetFlowerGrowMaterials(_meshRenderer);
            foreach (Material mat in _growMaterials)
            {
                mat.SetFloat("Grow_", _minGrowValue);
            }

            _isGrown = false;
        }
        public void StartPlantGrowth()
        {
            if (_isGrown)
                return;
            foreach (Material material in _growMaterials)
            {
                StartCoroutine(GrowMaterial(material));
            }
        }

        public void StopPlantGrowth()
        {
            IsCurrentlyGrowing = false;
            StopAllCoroutines();
        }

        #region Materials Processor
        IEnumerator GrowMaterial(Material material)
        {
            IsCurrentlyGrowing = true;

            index++;
            //Debug.Log(index);    // Index to track how often coroutine is being called

            float currentValue = material.GetFloat("Grow_");
            while (!_isGrown)
            {
                currentValue += 1 / (_growthTime / _refreshRate);
                if (currentValue >= _maxGrowValue)
                {
                    _isGrown = true;
                    material.SetFloat("Grow_", _maxGrowValue);
                    yield break;
                }

                if (currentValue > _maxGrowValue)
                    currentValue = _maxGrowValue;

                material.SetFloat("Grow_", currentValue);

                yield return new WaitForSeconds(_refreshRate);
            }
        }
        private List<Material> GetFlowerGrowMaterials(MeshRenderer flower)
        {
            var materials = new List<Material>();
            foreach (var mat in flower.materials)
            {
                if (!mat.HasFloat("Grow_")) continue;
                materials.Add(mat);
            }
            return materials;
        }
        #endregion
    }
}