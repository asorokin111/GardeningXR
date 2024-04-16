using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ARRaycastPlace : MonoBehaviour
{
	public LayerMask            grassLayer;
	public XRRayInteractor      interactor;
	public ARAnchorManager      anchorManager;
	public GameObject           prefab;
	public InputActionReference uhu;
	
	public Mesh     mesh;
	public Material material;

	private bool       _shouldPaint;
	private Collider[] grassColls = new Collider[10];

	private void OnEnable()
	{
		uhu.action.performed += ReenableDraw;
		uhu.action.canceled += ReenableDraw;
	}
	
	private void OnDisable()
	{
		uhu.action.performed -= ReenableDraw;
		uhu.action.canceled  -= ReenableDraw;
	}
	
	private void ReenableDraw(InputAction.CallbackContext ctx) => _shouldPaint = !_shouldPaint;

	private void Update()
	{
		if (_shouldPaint) SpawnAnchor(null);
	}

	public async void SpawnAnchor(BaseInteractionEventArgs args)
	{
		interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
		
		Pose hitPos = new Pose(hit.point, Quaternion.LookRotation(hit.normal));

		if (!hit.collider.gameObject.TryGetComponent(out ARPlane plane)) return;
		if (plane.classifications != PlaneClassifications.DoorFrame) return;
		
		var  result = await anchorManager.TryAddAnchorAsync(hitPos);
		bool success = result.TryGetResult(out ARAnchor anchor);

		if (!success) return;
		if (Physics.OverlapSphereNonAlloc(anchor.pose.position, 0.05f, grassColls, grassLayer) > 0) return;
		//material.SetPass(0);
		//Graphics.DrawMeshNow(mesh, anchor.pose.position, anchor.pose.rotation);
		Instantiate(prefab, anchor.pose.position, anchor.pose.rotation);
	}
}
