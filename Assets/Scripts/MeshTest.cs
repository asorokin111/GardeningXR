using System;
using System.Collections;
using System.Collections.Generic;
using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
	[GetComponent][SerializeField] private MyMesh myMesh;
	[GetComponent][SerializeField] private MeshFilter myMeshFilter;
	[SerializeField] private Transform p1, p2;

	private void Start()
	{
		myMesh.GenerateLianeBetweenPoints(p1.position, p2.position);
	}
}
