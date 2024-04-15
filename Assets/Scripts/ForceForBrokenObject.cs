using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForceForBrokenObject : MonoBehaviour
{
    private List<Rigidbody> _objectParts;

    private void Awake()
    {
        _objectParts = GetComponentsInChildren<Rigidbody>().ToList();
    }

    public void AddForceToAllParts(Vector3 velocity)
    {
        foreach (var part in _objectParts)
        {
            part.velocity = velocity;
        }
    }
}
