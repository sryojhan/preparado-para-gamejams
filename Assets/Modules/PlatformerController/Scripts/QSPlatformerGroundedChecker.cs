using UnityEngine;
using UnityEngine.Events;

namespace QuickStart.Platformer
{
    [AddComponentMenu("QuickStart platformer/Core/Grounded check")]
    public class QSPlatformerGroundedChecker : MonoBehaviour
    {
        [Tooltip("Size of circunference used to calculate if the character is grounded")]
        public float feetSize = 0.2f;
        [Tooltip("Collection of layers the player can use to stand on")]
        public LayerMask groundLayer = 1;

        [Header("Callbacks")]
        [Tooltip("Called when the characer goes from airborn to a grounded status")]
        public UnityAction OnGrounded;
        [Tooltip("Called when the characer goes from grounded to an airborn status")]
        public UnityAction OnAirborn;

        /* status properties with public getters  */
        public bool IsGrounded { get; private set; }

        public bool JustGrounded { get; private set; }
        public bool JustAirborn { get; private set; }

        private float groundedTime;
        private float airbornTime;


        public void CheckGrounded(BoxCollider2D boxCollider)
        {
            if (!isActiveAndEnabled) return;

            //Reset previous status
            JustGrounded = JustAirborn = false;


            //Do a overlap circle at the bottom of the player collider
            Collider2D[] collisions = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, boxCollider.bounds.min.y), feetSize, groundLayer);

            bool newGroundedValue = false;

            /* 
                We do this loop so the player so the player can
                be in the same layer as the rest of the scene objects
            */
            foreach (Collider2D collision in collisions)
            {
                if (collision && collision != boxCollider)
                {
                    newGroundedValue = true;
                    break;
                }
            }


            if (newGroundedValue != IsGrounded)
            {
                //The player has changed its status since the previous frame

                if (newGroundedValue)
                {
                    //The player has just entered the ground
                    OnGrounded?.Invoke();

                    JustGrounded = true;
                    groundedTime = Time.time;
                }
                else
                {
                    //The player has just become airborn
                    OnAirborn?.Invoke();

                    JustAirborn = true;
                    airbornTime = Time.time;
                }

                IsGrounded = newGroundedValue;
            }

        }


        /// <summary>
        /// TODO: summary time since grounded
        /// </summary>
        /// <returns></returns>
        public float TimeSinceGrounded()
        {
            return IsGrounded ? Time.time - groundedTime  : float.PositiveInfinity;
        }

        /// <summary>
        /// TODO: summary time since grounded
        /// </summary>
        /// <returns></returns>
        public float TimeSinceAirborn()
        {
            return !IsGrounded ? Time.time - airbornTime : float.PositiveInfinity;
        }


    }


}