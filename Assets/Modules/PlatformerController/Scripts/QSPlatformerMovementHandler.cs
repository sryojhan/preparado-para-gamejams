// Character Controller 2D (c) 2025 Yojhan Steven García Peña
using UnityEngine;

namespace QuickStart.Platformer
{
    [AddComponentMenu("QuickStart platformer/Core/Movement")]

    [RequireComponent(typeof(Rigidbody2D))]
    public class QSPlatformerMovementHandler : MonoBehaviour
    {
        /* public attributes */

        [Header("Horizontal movement")]

        [Tooltip("The maximum speed the character can reach by running")]
        public float maxHorizontalSpeed = 10;

        [Tooltip("Multiplier that controls how strong the acceleration force is applied. Higher values = faster acceleration.")]
        public float accelerationRate = 10;

        [Tooltip("Multiplier that controls how strong the deceleration force is applied. Higher values = faster deceleration.")]
        public float decelerationRate = 10;

        [Range(0.5f, 2.5f)]
        [Tooltip("Exponent that shapes the acceleration curve. 1 = linear, 2 = sharper response, <1 = slower and smoother.")]
        public float accelerationPower = 1;


        [Header("Vertical movement")]
        [Tooltip("TODO: fallSpeed description")]
        public float fallSpeed = 10f;

        [Tooltip("TODO: fallspeed header")]
        public float fastFallSpeed = 70f;

        /* Private atributes */

        private Vector2 input;
      
        /// <summary>
        /// Stores the input for its later use in the late update.
        /// This method may seem redundant, as it simply forwards input, 
        /// but it's designed for consistency and to allow future extensions or overrides.
        /// </summary>
        public void UpdatePlayerInput(Vector2 leftStickInput)
        {
            if (!isActiveAndEnabled)
            {
                input = Vector2.zero;
                return;
            }

            input = leftStickInput;
        }



        /// <summary>
        /// TODO: horizontal input description
        /// </summary>
        public void ApplyHorizontalMovement(Rigidbody2D rigidBody)
        {
            if (!isActiveAndEnabled) return;

            //Desired speed based on the player input
            float targetVelocity = input.x * maxHorizontalSpeed;

            //Calculate the difference between current speed and desired speed
            float velocityDiff = targetVelocity - rigidBody.linearVelocityX;

            //Calculate if we need to accelerate or decelerate to achieve target velocity
            float rate = (Mathf.Abs(targetVelocity) - Mathf.Abs(rigidBody.linearVelocityX)) > 0.01f
                ? accelerationRate //acceleration rate
                : decelerationRate;  //deceleration rate

            /*
             * Calculate the final force: (diff * rate) ^ accelerationPower
             * This has the following effect
             * 
             */
            float force = Mathf.Pow(Mathf.Abs(velocityDiff) * rate, accelerationPower) * Mathf.Sign(velocityDiff);


            rigidBody.AddForce(force * Vector2.right, ForceMode2D.Force);
        }

        /// <summary>
        /// TODO: vertical movement summary
        /// </summary>
        /// <param name="rigidBody"></param>
        public void ApplyVerticalMovement(Rigidbody2D rigidBody)
        {
            if (!isActiveAndEnabled) return;

            float downwardsSpeed = 0;


            //Add aditional speed when going downwards (make the character less floaty)
            if(rigidBody.linearVelocityY < 0)
            {
                downwardsSpeed += fallSpeed;
            }

            //We only use the negative vertical input
            float inputVelocity = Mathf.Min(input.y, 0);
            if(inputVelocity < 0)
            {
                downwardsSpeed += fastFallSpeed;
            }

            rigidBody.AddForce(Vector2.down * downwardsSpeed, ForceMode2D.Force);
        }
    }
}