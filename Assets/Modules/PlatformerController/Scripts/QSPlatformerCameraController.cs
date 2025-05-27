// QuickStart Platformer (c) 2025 Yojhan Steven García Peña

using UnityEngine;

namespace QuickStart.Platformer
{
    [AddComponentMenu("QuickStart platformer/Extras/Camera controller")]


    public class QSPlatformerCameraController : MonoBehaviour
    {

        [System.Serializable]
        public class SmoothTransition
        {
            [Range(0, 1.5f)]
            public float smoothTime;

            private Vector2 velocity;
            private Vector2 value;


            public Vector2 Smooth(Vector2 newValue)
            {
                return value = Vector2.SmoothDamp(value, newValue, ref velocity, smoothTime);
            }

            public void Set(Vector2 newValue)
            {
                value = newValue;
            }

            public SmoothTransition(float smoothTime)
            {
                this.smoothTime = smoothTime;
            }
        }


        [Header("Camera bounds")]
        public bool useCameraBounds = false;
        public Vector2 minPosition = new Vector2(-10, -10);
        public Vector2 maxPosition = new Vector2(10, 10);

        [Header("Camera follow")]
        public bool smoothFollow = true;
        public SmoothTransition horizontalFollow = new(.2f);
        public SmoothTransition verticalFollow = new(.2f);

        [Header("Enhanced camera")]
        public bool useSmartOffsets = false;

        [Header("Grounded displacement")]
        public SmoothTransition groundedDisplacement = new(.6f);
        public float groundedOffset = 2;

        [Header("Input based Lookahead")]
        public SmoothTransition inputDisplacement = new(.4f);
        public float horizontalInputOffset = 4f;
        public float verticalInputOffset = 2;
        public bool disableVerticalLookahead = false;

        [Header("Falling lookahead")]
        public SmoothTransition verticalVelocityDisplacement = new(.4f);
        public float verticalVelocityDisplacementRatio = 0.5f;

        private Transform parentTransform;
        private Transform cameraTransform;

        public void InitialiseCameraController()
        {
            cameraTransform = Camera.main.transform;

            parentTransform = new GameObject("Camera parent").transform;
            parentTransform.position = cameraTransform.position;

            cameraTransform.SetParent(parentTransform);

            horizontalFollow.Set(parentTransform.position);
            verticalFollow.Set(parentTransform.position);
        }

        public bool ShouldCameraUpdate(Vector2 position)
        {
            return true;
        }

        public void UpdateCameraPosition(Vector2 position, Vector2 input, Vector2 velocity, bool isGrounded)
        {
            if (!isActiveAndEnabled) return;

            if (!smoothFollow)
            {
                if (useCameraBounds)
                {
                    position = AdjustToCameraBounds(position);
                }

                parentTransform.position = AdjustCameraZpos(position);
                return;
            }

            Vector2 horizontal = horizontalFollow.Smooth(position);
            Vector2 vertical = verticalFollow.Smooth(position);

            Vector2 targetPosition = new Vector2(horizontal.x, vertical.y);
            Vector2 targetOffset = Vector2.zero;

            if (useSmartOffsets)
            {
                float verticalMultiplier = disableVerticalLookahead ? 0 : verticalInputOffset;

                targetOffset += inputDisplacement.Smooth(input * new Vector2(horizontalInputOffset, verticalMultiplier));

                targetOffset += groundedDisplacement.Smooth(Vector2.up * (isGrounded ? groundedOffset : 0));

                targetOffset += verticalVelocityDisplacement.Smooth(Vector2.up * (velocity.y * verticalVelocityDisplacementRatio));
            }


            Vector2 finalPosition = targetPosition + targetOffset;

            if (useCameraBounds)
            {
                finalPosition = AdjustToCameraBounds(finalPosition);
            }

            parentTransform.position = AdjustCameraZpos(finalPosition);
        }


        private Vector2 AdjustToCameraBounds(Vector2 position)
        {
            float hCameraHeight = Camera.main.orthographicSize;
            float hCameraWidth = hCameraHeight * Camera.main.aspect;

            Vector2 minPossibleValue = minPosition + new Vector2(hCameraWidth, hCameraHeight);
            Vector2 maxPossibleValue = maxPosition - new Vector2(hCameraWidth, hCameraHeight);

            position.x = Mathf.Clamp(position.x, minPossibleValue.x, maxPossibleValue.x);
            position.y = Mathf.Clamp(position.y, minPossibleValue.y, maxPossibleValue.y);

            return position;
        }


        private Vector3 AdjustCameraZpos(Vector2 position)
        {
            return new Vector3(position.x, position.y, -10);
        }


        private void Reset()
        {
            minPosition = Camera.main.transform.position - new Vector3(10, 10);
            maxPosition = Camera.main.transform.position + new Vector3(10, 10);
        }

        private void OnDrawGizmosSelected()
        {
            if (!useCameraBounds) return;

            Gizmos.color = Color.red;

            Vector2 topLeft = new Vector2(minPosition.x, maxPosition.y);
            Vector2 bottomRight = new Vector2(maxPosition.x, minPosition.y);

            Gizmos.DrawLine(minPosition, bottomRight);
            Gizmos.DrawLine(bottomRight, maxPosition);
            Gizmos.DrawLine(maxPosition, topLeft);
            Gizmos.DrawLine(topLeft, minPosition);
        }

        //TODO: camera deathzone
        //TODO: smart reset
    }
}
