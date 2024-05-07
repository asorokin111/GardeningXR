using Nrjwolf.Tools.AttachAttributes;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
	[GetComponent][SerializeField] private LianeMeshGenerator leaneMeshGenerator;
	[SerializeField]               private Transform          p1, p2;

	private void Start()
	{
		leaneMeshGenerator.GenerateLianeBetweenPoints(p1.position, p2.position);
	}
}
