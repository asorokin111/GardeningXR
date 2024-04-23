using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LianePlacing : MonoBehaviour
{
	[SerializeField]                private InputActionReference placeLianeAnchorButton;
	[SerializeField]                private ARAnchorManager      anchorManager;
	[SerializeField]                private XRRayInteractor      interactor;
	[SerializeField]                private Material             lianeMaterial;
	[GetComponent] [SerializeField] private MyMesh               myMesh;

	private List<Vector3> _anchors = new();

	private void Start()
	{
		placeLianeAnchorButton.action.performed += (ctx) => PlaceLianeAnchor();
	}
	
	private async void PlaceLianeAnchor()
	{
		interactor.TryGetCurrent3DRaycastHit(out RaycastHit hit);

		Pose hitPos = new Pose(hit.point, Quaternion.LookRotation(hit.normal));

		if (!hit.collider.gameObject.TryGetComponent(out ARPlane plane)) return;
		var  result  = await anchorManager.TryAddAnchorAsync(hitPos);
		bool success = result.TryGetResult(out ARAnchor anchor);

		if (!success) return;

		_anchors.Add(anchor.pose.position);
		if (_anchors.Count == 2)
		{
			GenerateLiane(_anchors[0], _anchors[1]);
			_anchors.Clear();
		}
	}

	private void GenerateLiane(Vector3 pos1, Vector3 pos2)
	{
		myMesh.GenerateLianeBetweenPoints(pos1, pos2);
	}

}
