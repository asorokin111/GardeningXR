using System;
using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiralTrajectoryTest : MonoBehaviour
{
	[SerializeField] private float        StartRadius;
	[SerializeField] private LineRenderer lineRenderer;

	private List<Vector3> positions = new List<Vector3>();

	private void Start()
	{
		for (var t = 0f; t < 30; t += 0.1f)
		{
			positions.Add(GetPosition(t));
		}
		
		lineRenderer.positionCount = positions.Count;
		lineRenderer.SetPositions(positions.ToArray());
	}

	private Vector3 GetPosition(float t)
	{
		
		float radiusDelta = Mathf.PerlinNoise1D(t) * 2;
		return new Vector3(
			Mathf.Cos(t) * StartRadius + radiusDelta,
			Mathf.Sin(t) * StartRadius + radiusDelta,
			3*t
			);
	}
}
