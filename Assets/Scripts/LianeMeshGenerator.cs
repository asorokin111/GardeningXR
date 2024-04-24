using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LianeMeshGenerator : MonoBehaviour
{
	[SerializeField] private float      step, radius;
	[SerializeField] private int        radiusPoints;
	[SerializeField] private Material   lianeMaterial, leafMaterial;
	[SerializeField] private GameObject leafPrefab;

	private List<Vector3> GetPoints(Vector3 pos1, Vector3 pos3)
	{
		
		Vector3 pos2 = (pos1 + pos3) / 2;
		pos2.y = Mathf.Min(pos1.y, pos3.y);
		
		List<Vector3> points = new List<Vector3>();
		for (var t = 0f; t < 1; t += step)
		{
			var x = GetPoint(pos1.x, pos2.x, pos3.x, t);
			var y = GetPoint(pos1.y, pos2.y, pos3.y, t);
			var z = GetPoint(pos1.z, pos2.z, pos3.z, t);
			
			Vector3 point = new Vector3(x, y, z);
			points.Add(point);
		}
		points.Insert(0, pos1);
		points.Add(pos3);

		return points;
	}
	public void GenerateLianeBetweenPoints(Vector3 pos1, Vector3 pos3)
	{
		var           points    = GetPoints(pos1, pos3);
		List<Vector3> vertices  = new List<Vector3>();
		List<Vector3> normals   = new List<Vector3>();
		var           angleStep = 360f / radiusPoints;
		
		GameObject resultLeaves = new GameObject();
		for (var i = 0; i < points.Count - 1; i++)
		{
			var direction      = (points[i + 1] - points[i]).normalized;
			var crossWith      = direction == Vector3.up ? Vector3.right : Vector3.up;
			var rotationVector = Vector3.Cross(crossWith, direction).normalized * radius;
			var center         = (points[i] + points[i + 1])                    * 0.5f;
			
			for (var angle = 0f; angle < 360; angle += angleStep)
			{
				var rotatedVector = Quaternion.AngleAxis(angle, direction) * rotationVector;
				
				vertices.Add(center + rotatedVector);
				normals.Add(rotatedVector.normalized);
				
				if (Random.Range(0f, 1f) >= 0.1f) continue;
				
				var leaf = Instantiate(leafPrefab, center + rotatedVector, Quaternion.identity);
				leaf.transform.up = rotatedVector.normalized;
				leaf.transform.SetParent(resultLeaves.transform);
			}			
		}

		var  triangles = GetTriangles(points.Count - 2);
		var  uvs       = GetUVs(vertices.Count);
		Mesh mesh      = new Mesh();
		mesh.vertices   = vertices.ToArray();
		mesh.triangles  = triangles.ToArray();
		mesh.normals    = normals.ToArray();
		mesh.uv   = uvs.ToArray();

		GameObject resultLiane = new GameObject();
		var render = resultLiane.AddComponent<MeshRenderer>();
		var filter = resultLiane.AddComponent<MeshFilter>();
		
		render.material = lianeMaterial;
		filter.mesh       = mesh;
		
		CombineLeaves(resultLeaves.transform);
	}

	private void CombineLeaves(Transform leafParent)
	{
		MeshFilter[]      meshFilters = leafParent.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine     = new CombineInstance[meshFilters.Length];

		int i = 0;
		while (i < meshFilters.Length)
		{
			combine[i].mesh      = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			
			meshFilters[i].gameObject.SetActive(false);

			i++;
		}

		foreach (Transform obj in leafParent)
		{
			Destroy(obj.gameObject);
		}

		leafParent.AddComponent<MeshFilter>();
		var renderer = leafParent.AddComponent<MeshRenderer>();
		renderer.sharedMaterial = leafMaterial;
		Mesh mesh = new Mesh();
		mesh.CombineMeshes(combine);
		leafParent.GetComponent<MeshFilter>().sharedMesh = mesh;
		leafParent.gameObject.SetActive(true);
	}

	private List<int> GetTriangles(int ringsAmount)
	{
		var triangles = new List<int>();

		for (int i = 0; i < ringsAmount; i++)
		{
			var ringStartIndex     = i * radiusPoints;
			var nextRingStartIndex = ringStartIndex + radiusPoints;
			for (int delta = 0; delta < radiusPoints - 1; delta++)
			{
				var a = ringStartIndex     + delta;
				var b = nextRingStartIndex + delta;
				var c = nextRingStartIndex + delta + 1;
				var d = ringStartIndex     + delta + 1;
			
				// first triangle
				triangles.Add(c);
				triangles.Add(b);
				triangles.Add(a);
			
				// second triangle
				triangles.Add(d);
				triangles.Add(c);
				triangles.Add(a);
			}
			
			triangles.Add(nextRingStartIndex);
			triangles.Add(nextRingStartIndex + radiusPoints - 1);
			triangles.Add(nextRingStartIndex                - 1);

			triangles.Add(ringStartIndex);
			triangles.Add(nextRingStartIndex);
			triangles.Add(nextRingStartIndex - 1);
		}

		return triangles;
	}

	private List<Vector2> GetUVs(int verticesAmount)
	{
		List<Vector2> uvs         = new List<Vector2>();
		var           ringsAmount = verticesAmount / radiusPoints;
		for (int i = 0; i < verticesAmount; i++)
		{
			float x = (i % radiusPoints) / (float) radiusPoints;
			float y = (i / radiusPoints) / (float) ringsAmount;
			uvs.Add(new Vector2(x, y));
		}

		return uvs;
	}
	
	private float GetPoint(float x0, float x1, float x2, float t)
	{
		return Mathf.Pow(1 - t, 2) * x0 + 2 * (1 - t) * t * x1 + Mathf.Pow(t, 2) * x2;
	}
}
