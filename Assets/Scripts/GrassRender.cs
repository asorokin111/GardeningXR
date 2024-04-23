using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

class GrassRender : MonoBehaviour
{
	struct DrawData
	{
		public Vector3    Pos;
		public Quaternion Rot;
		public Vector3    Scale;
	}

	public Mesh     mesh;
	public Material material;
  
	List<DrawData> instances;

	private ComputeBuffer             _drawDataBuffer;
	ComputeBuffer                     _argsBuffer;
	uint[]                            _args = new uint[5];
	MaterialPropertyBlock             _mpb;
	private static readonly  int      Data = Shader.PropertyToID("_DrawData");

	private Bounds bounds = new Bounds(Vector3.zero, Vector3.one * 100f);
	
	void Awake()
	{
		instances = new List<DrawData>();

		_argsBuffer = new ComputeBuffer(5, sizeof(uint), ComputeBufferType.IndirectArguments);
		// Meshes with sub-meshes needs more structure, this assumes a single sub-mesh
		_args[0] = mesh.GetIndexCount(0);

		_mpb = new MaterialPropertyBlock();
	}
  
	void OnDestroy()
	{
		_argsBuffer?.Release();
		_drawDataBuffer?.Release();
	}
  
	void LateUpdate()
	{
		Graphics.DrawMeshInstancedIndirect(
			mesh, 0, material, 
			bounds,
			_argsBuffer, 0,
			_mpb
		);
	}

	void PushDrawData()
	{
		if (_drawDataBuffer == null || _drawDataBuffer.count < instances.Count)
		{
			_drawDataBuffer?.Release();
			_drawDataBuffer = new ComputeBuffer(instances.Count, Marshal.SizeOf<DrawData>());
		}
		_drawDataBuffer.SetData(instances);
	}

	public void PlaceObject(Vector3 position, Quaternion rotation)
	{
		instances.Add(new DrawData()
		{
			Pos   = position,
			Rot   = Quaternion.identity,
			Scale = Vector3.one
		});
		
		PushDrawData();
		_mpb.SetBuffer(Data, _drawDataBuffer);
    
		_args[1] = (uint)instances.Count;
		_argsBuffer.SetData(_args);
	}
}