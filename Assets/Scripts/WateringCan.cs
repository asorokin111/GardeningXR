using UnityEngine;

public class WateringCan : ParticleDropper
{
    protected override bool CheckTilt()
    {
        var angles = UnityEditor.TransformUtils.GetInspectorRotation(transform);
        int dropAngle = 50;
        int modulo = 360 - dropAngle;
        return Mathf.Abs(angles.x) % modulo < dropAngle;
    }
}
