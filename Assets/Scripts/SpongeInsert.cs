using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace Gardening
{
    /// <summary>
    /// Class that handles inserting flowers into floral sponges
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class SpongeInsert : MonoBehaviour
    {
        [SerializeField]
        private XRGrabInteractable _interactable;
        private bool _isAnchored = false;
        private Rigidbody _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _interactable = GetComponent<XRGrabInteractable>();
            //_interactable.selectEntered.AddListener(Unanchor);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Sponge") || _isAnchored) return;
            AnchorTo(other);
        }

        //private Vector3 GetAverageOfContactPoints(ContactPoint[] points)
        //{
        //    Vector3 sum = Vector3.zero;
        //    Array.ForEach(points, delegate (ContactPoint c) { sum += c.point; });
        //    return sum / points.Length;
        //}

        private void AnchorTo(Collision other)
        {
            _interactable.enabled = false;
            _rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            //Vector3 averageOfPoints = GetAverageOfContactPoints(other.contacts);
            //transform.SetPositionAndRotation(averageOfPoints, Quaternion.FromToRotation(transform.up, -other.GetContact(0).normal));
            transform.SetParent(other.transform, true);
            transform.rotation = Quaternion.FromToRotation(transform.up, -other.GetContact(0).normal);

            _isAnchored = true;
        }

        //        private void Unanchor(SelectEnterEventArgs args)
        //        {
        //#pragma warning disable 0252
        //            // Comparing references here, need to check if this actually works
        //            if (args.interactableObject != _interactable) { Debug.Log("Interactable object is not equal to interactable"); return; };
        //#pragma warning restore
        //            _isAnchored = false;
        //            transform.SetParent(null, true);
        //            GetComponent<Collider>().enabled = true;
        //            _rb.isKinematic = false;
        //        }
    }

}
