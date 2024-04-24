using System;
using System.Collections;
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
	[SerializeField] private LayerMask            grassLayer;
	[SerializeField] private XRRayInteractor      interactor;
	[SerializeField] private ARAnchorManager      anchorManager;
	[SerializeField] private GameObject           grassPrefab;
	[SerializeField] private InputActionReference drawGrassButton;

	private bool       _shouldPaint;
	private Collider[] _grassColls = new Collider[10];

	private float _deltaTime;

	private void OnEnable()
	{
		drawGrassButton.action.performed += ReenableDraw;
		drawGrassButton.action.canceled  += ReenableDraw;
	}

	private void OnDisable()
	{
		drawGrassButton.action.performed -= ReenableDraw;
		drawGrassButton.action.canceled  -= ReenableDraw;
	}

	private void ReenableDraw(InputAction.CallbackContext ctx) => _shouldPaint = !_shouldPaint;

	private void Update()
	{
		if (_shouldPaint) SpawnAnchor();
	}

	private async void SpawnAnchor()
	{
		interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

		Pose hitPos = new Pose(hit.point, Quaternion.LookRotation(hit.normal));

		if (!hit.collider.gameObject.TryGetComponent(out ARPlane plane)) return;
		if (plane.classifications != PlaneClassifications.DoorFrame) return;

		var  result  = await anchorManager.TryAddAnchorAsync(hitPos);
		bool success = result.TryGetResult(out ARAnchor anchor);

		if (!success) return;
		if (Physics.OverlapSphereNonAlloc(anchor.pose.position, 0.05f, _grassColls, grassLayer) > 0) return;
		Instantiate(grassPrefab, anchor.pose.position, anchor.pose.rotation);
	}
}