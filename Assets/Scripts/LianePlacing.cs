using System.Collections.Generic;
using System.Linq;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LianePlacing : MonoBehaviour
{
	[SerializeField] private int lianesCount;
	[GetComponent] [SerializeField] private ARPlaneManager 	 arPlaneManager;
	[GetComponent] [SerializeField] private LianeMeshGenerator   lianeMeshGenerator;
	
	private void Start()
	{
		arPlaneManager.trackablesChanged.AddListener(PlaceLianes);
	}

	private void PlaceLianes(ARTrackablesChangedEventArgs<ARPlane> args)
	{
		var groundPlane = args.added.First(plane => plane.classifications == PlaneClassifications.DoorFrame);
		if (groundPlane == default) return;

		for (int i = 0; i < lianesCount; i++)
		{
			var groundPos = groundPlane.center + new Vector3(
				Random.Range(-groundPlane.extents.x, groundPlane.extents.x),
				0,
				Random.Range(-groundPlane.extents.y, groundPlane.extents.y)
				);

			var ceilingPos = groundPos + new Vector3(
				Random.Range(-0.4f, 0.4f),
				3,
				Random.Range(-0.4f, 0.4f)
				);
			
			lianeMeshGenerator.GenerateLianeBetweenPoints(groundPos, ceilingPos);
		}
		
		arPlaneManager.trackablesChanged.RemoveListener(PlaceLianes);
	}
}
