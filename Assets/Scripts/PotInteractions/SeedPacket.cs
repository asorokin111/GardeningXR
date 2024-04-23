using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine;

namespace Gardening
{
    public class SeedPacket : ParticleDropper
    {
        public delegate void SeedPacketPickUpAction();
        public static SeedPacketPickUpAction OnSeedPacketFirstPickedUp;

        public Plant associatedPlant;

        private XRGrabInteractable _interactable;

        /// <summary>
        /// Checks if the seed packet's rotation is suitable for dropping seeds
        /// </summary>
        protected override bool CheckTilt()
        {
            var angles = transform.rotation.eulerAngles;
            int dropAngle = 100;
            // Prevents the seed packet from acting weirdly with large rotation values
            int modulo = 360 - dropAngle;
            return Mathf.Abs(angles.x) % modulo >= dropAngle || Mathf.Abs(angles.z) % modulo >= dropAngle;
        }

        protected override void Awake()
        {
            base.Awake();
            _interactable = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            _interactable.firstSelectEntered.AddListener((SelectEnterEventArgs _) => OnSeedPacketFirstPickedUp?.Invoke());
        }

        private void OnDisable()
        {
            _interactable.firstSelectEntered.RemoveAllListeners();
        }
    }
}
