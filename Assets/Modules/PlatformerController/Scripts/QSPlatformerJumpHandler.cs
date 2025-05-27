// Character Controller 2D (c) 2025 Yojhan Steven García Peña
using UnityEngine;


namespace QuickStart.Platformer
{
    [AddComponentMenu("QuickStart platformer/Core/Jump Handler")]

    public class QSPlatformerJumpHandler : MonoBehaviour
    {
        [Header("Jump height")]
        public float jumpForce = 10;
        public float timeToAdjustJumpHeight = 0.5f;
        public float adjustedJumpDownwardsForce = 20f;

        [Header("Gamefeel upgrades")]
        public float timeToCoyoteJump = 0.2f;
        public float timeToCacheJump = 0.2f;

        [Header("Double jump")]
        public bool canDoubleJump = true;

        /* Private attributes */

        //Know if the jump button has been pressed
        private bool jumpButtonBuffered = false;

        //Jump button just pressed this frame. Useful so you dont
        //double jump just by holding the jump button
        private bool jumpButtonJustPressed = false;

        //Player has used double jump
        private bool doubleJumpUsed = false;

        //Time the jump button was press. Useful to check for a cached jump
        private float lastJumpButtonTime = 0;

        //Last time the player jumped
        private float lastJumpTime = 0;

        //Is the jump button being pressed this frame
        private bool isJumpButtonBeingPressed = false;

        //This is to control the jump height of the jump
        //This will be true when the player jumps from the ground until it lands again
        private bool isGroundJumpBeingPerformed = false;


        /// <summary>
        /// TODO: summary jump input
        /// </summary>
        /// <param name="jumpPressedThisFrame"></param>
        /// <param name="jumpReleasedThisFrame"></param>
        public void UpdateJumpInput(bool isJumpButtonPressed, bool jumpPressedThisFrame)
        {
            if (!isActiveAndEnabled) return;


            if (jumpPressedThisFrame)
            {
                jumpButtonJustPressed = true;
            }

            if (isJumpButtonPressed)
            {
                jumpButtonBuffered = true;
                lastJumpButtonTime = Time.time;
            }

            isJumpButtonBeingPressed = isJumpButtonPressed;
        }

        /// <summary>
        /// TODO: summary should jump
        /// </summary>
        /// <returns>Will the jump be performed this frame</returns>
        public bool ShouldJump(bool grounded, float timeSinceAirborn)
        {
            if (!isActiveAndEnabled) return false;
            //Check there hasnt been another jump a few frames before
            bool isTooClosePreviousJump = Time.time - lastJumpTime < timeToCoyoteJump;

            bool shouldJump = false;


            //Does the jump originate from the ground?
            bool isGroundJump = true;

            /*
                We can jump in four different situations:
                    
                    Ground jumps:
                    - Default behaviour: We are in the ground and jump
                    - Cached jump: We pressed the jump button just before the character hit the ground
                    - Coyote time: We pressed the jump button just after becoming airborn (technically in a coyote time you are in the air
                      but we will consider it a ground jump)
                    

                    Air jump:
                    - Double jump: We are in the air and jump, and we still had the double jump avaliable
             */


            //Only try to jump after a few moments of the previous jump
            if (!isTooClosePreviousJump)
            {
                /*
                 * Just one ground jump at a time! 
                 * This way we dont allow a coyote time jump after a normal jump for example
                */

                if (!isGroundJumpBeingPerformed)
                {
                    //Default behaviour
                    if (grounded && jumpButtonBuffered)
                    {
                        shouldJump = true;
                    }

                    //Cached jump
                    else if (grounded && Time.time - lastJumpButtonTime < timeToCacheJump)
                    {
                        shouldJump = true;
                    }

                    //Coyote time
                    else if (!grounded && jumpButtonBuffered && timeSinceAirborn < timeToCoyoteJump)
                    {
                        shouldJump = true;
                    }
                }

                //Double jump
                if (!shouldJump && canDoubleJump && jumpButtonJustPressed && !doubleJumpUsed)
                {
                    doubleJumpUsed = true;
                    shouldJump = true;
                    isGroundJump = false;
                }

                //Reset jump status
                if (grounded)
                {
                    doubleJumpUsed = false;
                    isGroundJumpBeingPerformed = false;

                }
                jumpButtonBuffered = false;
                jumpButtonJustPressed = false;
            }


            //jump performed from the ground
            if (shouldJump)
            {
                isGroundJumpBeingPerformed = isGroundJump;
            }

            return shouldJump;
        }

        /// <summary>
        /// TODO: summary jump logic
        /// </summary>
        /// <param name="rigidBody"></param>
        public void PerformJump(Rigidbody2D rigidBody)
        {
            if (!isActiveAndEnabled) return;

            //Nullify vertical velocity of the jump
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0);

            rigidBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

            lastJumpTime = Time.time;
            jumpButtonBuffered = false;

            //TODO: custom force to double jump
        }


        /// <summary>
        /// TODO: adjust jump height description
        /// </summary>
        /// <param name="rigidBody"></param>
        public void AdjustJumpHeight(Rigidbody2D rigidBody)
        {
            if (!isActiveAndEnabled) return;

            if (timeToAdjustJumpHeight <= 0) return;


            bool isInTimeRange = Time.time - lastJumpTime < timeToAdjustJumpHeight;

            bool isJumpButtonHeld = (isInTimeRange && isJumpButtonBeingPressed);

            if (!isGroundJumpBeingPerformed || !isJumpButtonHeld)
            {
                rigidBody.AddForce(Vector2.down * adjustedJumpDownwardsForce);
            }
        }
    }
}