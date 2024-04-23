using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nrjwolf.Tools.AttachAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrassPlacingRetest : MonoBehaviour
{
    [SerializeField]                private XRRayInteractor      interactor;
    [SerializeField]                private ARAnchorManager      anchorManager;
    [SerializeField]                private InputActionReference drawGrassButton;
    [GetComponent] [SerializeField] private GrassRender          grassRender;
    [SerializeField]                private ARPlaneManager       planeManager;

    private float       _deltaTime;
    private IEnumerator _coroutine;

    private bool _canDraw;
    
    private void OnEnable()
    {
        drawGrassButton.action.performed += (ctx) => Reenable();
        _coroutine                       =  AnchorCoroutine();
        planeManager.trackablesChanged.AddListener(PlaceRandomGrass);

    }

    private void Reenable()
    {
        SpawnAnchor();
        _canDraw = !_canDraw;
        
        if (_canDraw)
        {
            StartCoroutine(_coroutine);
        }
        else
        {
            StopCoroutine(_coroutine);
        }
    }
    

    private IEnumerator AnchorCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnAnchor();   
        }
    }

    private async void SpawnAnchor()
    {
        interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

        Pose hitPos = new Pose(hit.point, Quaternion.LookRotation(hit.normal));

        if (!hit.collider.gameObject.TryGetComponent(out ARPlane plane)) return;
        if (plane.classifications != PlaneClassifications.DoorFrame) return; ;

        var  result  = await anchorManager.TryAddAnchorAsync(hitPos);
        bool success = result.TryGetResult(out ARAnchor anchor);

        if (!success) return;
        grassRender.PlaceObject(anchor.pose.position, anchor.pose.rotation);
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
