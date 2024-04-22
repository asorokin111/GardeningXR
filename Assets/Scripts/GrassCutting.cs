using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassCutting : MonoBehaviour
{
	[SerializeField] private Mesh[] cuttedMeshes;
	
	private MeshFilter _meshFilter;
	private int _cuttedMeshIndex;

	private void Start()
	{
		_meshFilter = GetComponent<MeshFilter>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Trimmer")) return;
		if (_cuttedMeshIndex == cuttedMeshes.Length) return;
		_meshFilter.mesh = cuttedMeshes[_cuttedMeshIndex];
		_cuttedMeshIndex++;
	}
}
