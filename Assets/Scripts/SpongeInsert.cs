using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
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
        private Quaternion _startingRotation;
        private FlowerState _state;
        private LayerMask _startingLayer;

        [Flags]
        private enum FlowerState
        {
            None = 0,
            Anchorable = 1,
            Unanchorable = 2,
            All = 3,
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _startingRotation = transform.rotation;
            _interactable = GetComponent<XRGrabInteractable>();
            _interactable.selectEntered.AddListener(Unanchor);
            _state = FlowerState.All;
            _startingLayer = gameObject.layer;
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
            if ((_state & FlowerState.Anchorable) == FlowerState.None) return;

            ToggleAnchoredLayer();
            StartCoroutine(TemporaryFlipFlowerState(FlowerState.Unanchorable, 2));
            _rb.isKinematic = true;

            transform.SetParent(other.transform, true);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, other.GetContact(0).normal);
            transform.rotation *= _startingRotation;

            _isAnchored = true;
        }

        private void Unanchor(SelectEnterEventArgs args)
        {
            if (!_isAnchored || (_state & FlowerState.Unanchorable) == FlowerState.None) return;

            StartCoroutine(TemporaryFlipFlowerState(FlowerState.Anchorable, 2)); // Anchor immunity
            _isAnchored = false;
            ToggleAnchoredLayer();
            transform.SetParent(null, true);
            _rb.isKinematic = false;
        }

        /// <summary>
        /// Flip a given flower state flag and flip it again after <paramref name="seconds"/> seconds pass
        /// </summary>
        private IEnumerator TemporaryFlipFlowerState(FlowerState flag, float seconds)
        {
            _state ^= flag;
            yield return new WaitForSeconds(seconds);
            _state ^= flag;
        }

        private void ToggleAnchoredLayer()
        {
            if (gameObject.layer == _startingLayer)
            {
                gameObject.layer = LayerMask.NameToLayer("AnchoredPlant");
            }
            else
            {
                gameObject.layer = _startingLayer;
            }
        }
    }

}
