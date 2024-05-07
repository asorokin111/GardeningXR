using Es.InkPainter;
using UnityEngine;

public class ParticlePainter : MonoBehaviour
{
    [SerializeField] private Brush brush;

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Water"))
            return;

        var paintObject = GetComponent<InkCanvas>();

        if(paintObject != null)
        {
            paintObject.Paint(brush, other.transform.position);
        }
    }
}
