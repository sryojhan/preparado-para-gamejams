// QuickStart Platformer (c) 2025 Yojhan Steven García Peña

using UnityEngine;

namespace QuickStart.Platformer
{
    [AddComponentMenu("QuickStart platformer/Character controller")]

    [RequireComponent(typeof(QSPlatformerGroundedChecker))]
    [RequireComponent(typeof(QSPlatformerMovementHandler))]

    public class QSPlatformerCharacterController : MonoBehaviour
    {

        /* Private attributes */
        private QSPlatformerStateMachine stateMachine;

        /* Components references*/
        public Rigidbody2D RigidBody { get; private set; }
        public BoxCollider2D BoxCollider { get; private set; }

        /* Platformer references */
        public QSPlatformerMovementHandler MovementHandler { get; private set; }
        public QSPlatformerJumpHandler JumpHandler { get; private set; }
        public QSPlatformerSlideHandler SlideHandler { get; private set; }
        public QSPlatformerGroundedChecker GroundedChecker { get; private set; }
        public QSPlatformerInputManager InputManager { get; private set; }
        public QSPlatformerAnimationController AnimationController { get; private set; }
        public QSPlatformerCameraController CameraController { get; private set; }


        private void Awake()
        {
            InitialiseStateMachine();
            FetchComponentReferences();
        }

        private void Update()
        {
            UpdatePlayerInput();

            ManageSlideLogic();
            UpdateCurrentCharacterState();
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            CheckGrounded();

            ApplyStickMovement();


            TryJump();
            AdjustJumpHeight();
        }


        /* Awake */
        private void InitialiseStateMachine()
        {
            stateMachine = new QSPlatformerStateMachine();
        }

        private void FetchComponentReferences()
        {
            BoxCollider = GetComponentInChildren<BoxCollider2D>();
            RigidBody = GetComponentInChildren<Rigidbody2D>();

            MovementHandler = GetComponentInChildren<QSPlatformerMovementHandler>();
            JumpHandler = GetComponentInChildren<QSPlatformerJumpHandler>();
            SlideHandler = GetComponentInChildren<QSPlatformerSlideHandler>();
            GroundedChecker = GetComponentInChildren<QSPlatformerGroundedChecker>();
            InputManager = GetComponentInChildren<QSPlatformerInputManager>();
            AnimationController = GetComponentInChildren<QSPlatformerAnimationController>();
            CameraController = GetComponentInChildren<QSPlatformerCameraController>();
        }




        /* Update */
        private void UpdatePlayerInput()
        {
            MovementHandler.UpdatePlayerInput(InputManager.GetLeftStick());
            JumpHandler.UpdateJumpInput(
                InputManager.GetJump(),
                InputManager.WasJumpPressedThisFrame()
            );
        }



        private void ManageSlideLogic()
        {

        }

        private void UpdateCurrentCharacterState()
        {

        }
        private void UpdateAnimation()
        {

        }





        /* FixedUpdate */
        private void CheckGrounded()
        {
            GroundedChecker.CheckGrounded(BoxCollider);
        }

        private void ApplyStickMovement()
        {
            MovementHandler.ApplyHorizontalMovement(RigidBody);
            MovementHandler.ApplyVerticalMovement(RigidBody);
        }

        private void TryJump()
        {
            if (JumpHandler.ShouldJump(
                GroundedChecker.IsGrounded,
                GroundedChecker.TimeSinceAirborn())
                )
            {
                JumpHandler.PerformJump(RigidBody);
            }
        }


        private void AdjustJumpHeight()
        {
            JumpHandler.AdjustJumpHeight(RigidBody);
        }





    }
}