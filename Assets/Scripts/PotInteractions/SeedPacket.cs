using UnityEngine;

namespace Gardening
{
    public class SeedPacket : ParticleDropper
    {
        public Plant associatedPlant;

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
    }
}
