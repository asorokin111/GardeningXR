using Nrjwolf.Tools.AttachAttributes;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrassPlacingRetest : MonoBehaviour
{
    [SerializeField]                private XRRayInteractor      interactor;
    [SerializeField]                private ARAnchorManager      anchorManager;
    [GetComponent] [SerializeField] private GrassRender          grassRender;
    [SerializeField]                private ARPlaneManager       planeManager;

    private void OnEnable()
    {
        planeManager.trackablesChanged.AddListener(PlaceRandomGrass);
    }

    private void PlaceRandomGrass(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        var plane = args.added.First(plane => plane.classifications == PlaneClassifications.DoorFrame);
        if (plane == default) return;
        
        var center  = plane.center;
        var extends = plane.extents;

        const float step = 0.1f;
        
        float extendsHalfX = extends.x / 2;
        float extendsHalfZ = extends.y / 2;
        for (float x = -extendsHalfX; x <= extendsHalfX; x += step)
        {
            for (float z = -extendsHalfZ; z <= extendsHalfZ; z += step)
            {
                var newPos = new Vector3(center.x + x, 0, center.z + z);
                if (Mathf.PerlinNoise(newPos.x, newPos.z) <= 0.3f)
                {;
                    grassRender.PlaceObject(newPos, Quaternion.identity);
                }
            }
        }
        
        planeManager.trackablesChanged.RemoveListener(PlaceRandomGrass);
    }
}
