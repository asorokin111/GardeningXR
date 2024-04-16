using System;
using System.Collections;
using UnityEngine;

namespace Gardening
{
    public class GroundFilling : MonoBehaviour
    {
        public bool isFilled = false;
        public bool isCurrentlyFilling = false;

        [SerializeField] private float fillRate, stepSize;
        [SerializeField] private Transform groundTransform;
        [SerializeField] private Transform lowerBound, upperBound;
        [SerializeField] private float startRadius, endRadius;

        [Tooltip("Will adjust its scale to pot's scale if toggled ")]
        [SerializeField] private bool isPotShapedCircle;

        private Vector3 _startGroundTransform;
        private float _potHeight;
        private float _radiusDiff;

        private float _direction = 1;

        private IEnumerator _currentCoroutine;

        private void Start()
        {
            SetUp();
        }

        /// <summary>
        /// Set mode to "Fill" or "Take out"
        /// </summary>
        /// <param name="direction"></param>
        public void SetFillingMode(GroundFillingMode direction)
        {
            _direction = direction switch
            {
                GroundFillingMode.Fill => 1,
                GroundFillingMode.TakeOut => -1,
                _ => throw new NotSupportedException()
            };
            //switch(direction)
            //{
            //    case GroundFillingMode.Fill:
            //        _direction = 1f;
            //        break;
            //    case GroundFillingMode.TakeOut:
            //        _direction = -1f;
            //        break;
            //}
        }

        public void StartFillingAndCoroutine()
        {
            StartFilling();
            StartFillCoroutine();
        }

        public void StopFillingAndCoroutine()
        {
            StopFillCoroutine();
            StopFilling();
        }

        private void SetUp()
        {
            SetUpGroundTransform();
            SetUpPotHeight();
            SetUpRadius();
        }

        private void SetUpGroundTransform() => _startGroundTransform = groundTransform.localScale;
        private void SetUpPotHeight() => _potHeight = upperBound.localPosition.y - lowerBound.localPosition.y;
        private void SetUpRadius() => _radiusDiff = endRadius - startRadius;

        private void StartFillCoroutine()
        {
            _currentCoroutine = FillCoroutine();
            StartCoroutine(_currentCoroutine);
        }

        private void StopFillCoroutine()
        {
            StopCoroutine(_currentCoroutine);
        }

        private void StartFilling() => isCurrentlyFilling = true;
        private void StopFilling() => isCurrentlyFilling = false;

        private IEnumerator FillCoroutine()
        {
            isCurrentlyFilling = true;
            while (true)
            {
                if (HasFillLimitBeenReached())
                {
                    StopFillingAndCheckIsFilledUp();

                    yield break;
                }

                MoveDirt();

                CheckIsPotShapedCircleAndAdjustRadius();

                yield return new WaitForSeconds(fillRate);
            }
        }

        private void StopFillingAndCheckIsFilledUp()
        {
            StopFilling();

            if (IsReachedTopBoundary())
            {
                IsFilledUp(true);
            }
            else
            {
                IsFilledUp(false);
            }
        }

        private void CheckIsPotShapedCircleAndAdjustRadius()
        {
            if (isPotShapedCircle)
            {
                AdjustRadius();
            }
        }

        private bool HasFillLimitBeenReached()
        {
            bool hasTopLimitBeenReached = _direction > 0 && IsReachedTopBoundary();
            bool hadBottomLimitBeenReached = _direction < 0 && IsReachedBottomBoundary();

            return hasTopLimitBeenReached || hadBottomLimitBeenReached;
        }

        private bool IsReachedTopBoundary()
        {
            bool isReachedTopBoundary
                = groundTransform.localPosition.y > upperBound.localPosition.y;

            return isReachedTopBoundary;
        }

        private bool IsReachedBottomBoundary()
        {
            bool isReachedBottomBoundary
                = groundTransform.localPosition.y < lowerBound.localPosition.y;

            return isReachedBottomBoundary;
        }

        private void IsFilledUp(bool isFilled) => this.isFilled = isFilled;

        private void MoveDirt()
        {
            groundTransform.localPosition += Vector3.up * (stepSize * _direction);
        }

        private void AdjustRadius()
        {
            float currentHeight = CalculateCurrentHeight();
            float scale = CalculateScale(currentHeight);
            ApplyScale(scale);
        }

        private float CalculateCurrentHeight() => groundTransform.localPosition.y - lowerBound.localPosition.y;
        private float CalculateScale(float currentHeight) => startRadius + _radiusDiff * currentHeight / _potHeight;

        private void ApplyScale(float targetScale)
        {
            Vector3 newScale = _startGroundTransform * targetScale;
            groundTransform.localScale = newScale;
        }
    }
}
