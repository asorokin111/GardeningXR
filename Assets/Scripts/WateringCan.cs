public class WateringCan : ParticleDropper
{
    protected override bool CheckTilt()
    {
        return _particleSystem.transform.position.y < transform.position.y;
    }
}
