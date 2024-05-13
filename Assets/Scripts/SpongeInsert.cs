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
        public delegate void FlowerPlantedAction();
        public static FlowerPlantedAction OnFlowerPlanted;
        [SerializeField]
        private XRGrabInteractable _interactable;
        [SerializeField]
        private Vector3 _startingAngles = Vector3.zero;
        private bool _isAnchored = false;
        private Rigidbody _rb;
        private Quaternion _startingRotation;
        private FlowerState _state;
        private LayerMask _startingLayer;
        // Needed to return rigidbody to correct values after releasing grab
        private bool _prevKinematicState;
        private const float _interactionCooldown = 1f;

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
            _startingRotation = Quaternion.Euler(_startingAngles);
            _interactable = GetComponent<XRGrabInteractable>();
            _interactable.selectEntered.AddListener(Unanchor);
            _interactable.selectExited.AddListener(RestoreCorrectKinematicState);
            _state = FlowerState.All;
            _startingLayer = gameObject.layer;
            _prevKinematicState = _rb.isKinematic;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Sponge") || _isAnchored) return;
            AnchorTo(other);
        }

        private void AnchorTo(Collision other)
        {
            if ((_state & FlowerState.Anchorable) == FlowerState.None) return;
            
            ToggleAnchoredLayer();
            StartCoroutine(TemporaryFlipFlowerState(FlowerState.Unanchorable, _interactionCooldown));
            StartCoroutine(TemporarilyDisableInteractable(_interactionCooldown));
           
            transform.SetParent(other.transform, true);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, other.GetContact(0).normal);
            transform.rotation *= _startingRotation;

            _prevKinematicState = true;
            _rb.isKinematic = _prevKinematicState;
            _isAnchored = true;
            OnFlowerPlanted?.Invoke();
        }

        private void Unanchor(SelectEnterEventArgs _)
        {
            if (!_isAnchored || (_state & FlowerState.Unanchorable) == FlowerState.None) return;
            StartCoroutine(TemporaryFlipFlowerState(FlowerState.Anchorable, _interactionCooldown)); // Anchor immunity
            ToggleAnchoredLayer();
            transform.SetParent(null, true);

            _prevKinematicState = false;
            _rb.isKinematic = _prevKinematicState;
            _isAnchored = false;
        }

        /// <summary>
        /// Return this rigidbody to a state set by anchor/unanchor rather than whatever it was before being grabbed.
        /// Unless it's already the same, in which case it's flipped
        /// </summary>
        private void RestoreCorrectKinematicState(SelectExitEventArgs _)
        {
            if (_rb.isKinematic != _prevKinematicState)
            {
                _rb.isKinematic = _prevKinematicState;
            }
            else
            {
                // I feel like something should've gone wrong with this, but it didn't?
                _rb.isKinematic = false;
            }
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

        private IEnumerator TemporarilyDisableInteractable(float seconds)
        {
            _interactable.enabled = false;
            yield return new WaitForSeconds(seconds);
            _interactable.enabled = true;
        }

        private void ToggleAnchoredLayer()
        {
            gameObject.layer = gameObject.layer == _startingLayer ? LayerMask.NameToLayer("AnchoredPlant") : _startingLayer;
        }
    }
}
