using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CracksAppearing : MonoBehaviour
{
	[SerializeField] private ARPlaneManager planeManager;
	[SerializeField] private GameObject     test;

	[SerializeField] private InputActionReference raycastButton;
	[SerializeField] private XRRayInteractor      _interactor;
	private void Start()
	{
		planeManager.trackablesChanged.AddListener(Search);
	}

	private void Search(ARTrackablesChangedEventArgs<ARPlane> args)
	{
		var ceiling = args.added.First(plane =>plane.classifications ==( PlaneClassifications.Ceiling | PlaneClassifications.DoorFrame));
		Instantiate(test, ceiling.center, Quaternion.identity);
		planeManager.trackablesChanged.RemoveListener(Search);
	}
}
