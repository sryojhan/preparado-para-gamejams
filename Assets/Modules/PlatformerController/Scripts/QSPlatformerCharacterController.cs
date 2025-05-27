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
        public Rigidbody2D RigidBodyComponent { get; private set; }
        public BoxCollider2D BoxColliderComponent { get; private set; }
        public SpriteRenderer SpriteRendererComponent { get; private set; }
        public Animator AnimatorComponent { get; private set; }

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

        private void Start()
        {
            InitialiseCameraController();
        }

        private void Update()
        {
            UpdatePlayerInput();
            
            //TODO: ManageSlideLogic();

            UpdateCurrentCharacterState();
            UpdateAnimation();
        }


        private void FixedUpdate()
        {
            CheckGrounded();

            ManageMovement();

            TryJump();
            AdjustJumpHeight();
        }

        private void LateUpdate()
        {
            CameraController.UpdateCameraPosition(transform.position, InputManager.GetLeftStick(),RigidBodyComponent.linearVelocity, GroundedChecker.IsGrounded);
        }

        /* Awake */
        private void InitialiseStateMachine()
        {
            stateMachine = new QSPlatformerStateMachine();
        }

        private void FetchComponentReferences()
        {
            BoxColliderComponent = GetComponentInChildren<BoxCollider2D>();
            RigidBodyComponent = GetComponentInChildren<Rigidbody2D>();
            SpriteRendererComponent = GetComponentInChildren<SpriteRenderer>();
            AnimatorComponent = GetComponentInChildren<Animator>();


            MovementHandler = GetComponentInChildren<QSPlatformerMovementHandler>();
            JumpHandler = GetComponentInChildren<QSPlatformerJumpHandler>();
            SlideHandler = GetComponentInChildren<QSPlatformerSlideHandler>();
            GroundedChecker = GetComponentInChildren<QSPlatformerGroundedChecker>();
            InputManager = GetComponentInChildren<QSPlatformerInputManager>();
            AnimationController = GetComponentInChildren<QSPlatformerAnimationController>();
            CameraController = GetComponentInChildren<QSPlatformerCameraController>();
        }


        /* Start */

        private void InitialiseCameraController()
        {
            CameraController.InitialiseCameraController();
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
            //TODO: slide
        }

        private void UpdateCurrentCharacterState()
        {
            float horizontalSpeedMagnitude = Mathf.Abs(RigidBodyComponent.linearVelocityX);

            if(GroundedChecker.IsGrounded && stateMachine.state != QSPlatformerStateMachine.State.Jump)
            {

                if(horizontalSpeedMagnitude > 0.05)
                {
                    stateMachine.state = QSPlatformerStateMachine.State.Running;
                }
                else
                {
                    stateMachine.state = QSPlatformerStateMachine.State.Iddle;
                }
            }

            else
            {
                if(RigidBodyComponent.linearVelocityY <= 0)
                {
                    stateMachine.state = QSPlatformerStateMachine.State.Fall;
                }
            }
        }


        private void UpdateAnimation()
        {
            AnimationController.SetFlip(SpriteRendererComponent, RigidBodyComponent.linearVelocityX);
            AnimationController.UpdateAnimation(AnimatorComponent, stateMachine);
        }



        /* FixedUpdate */
        private void CheckGrounded()
        {
            GroundedChecker.CheckGrounded(BoxColliderComponent);
        }

        private void ManageMovement()
        {
            MovementHandler.ApplyHorizontalMovement(RigidBodyComponent);
            MovementHandler.ApplyVerticalMovement(RigidBodyComponent);
        }

        private void TryJump()
        {
            if (JumpHandler.ShouldJump(
                GroundedChecker.IsGrounded,
                GroundedChecker.TimeSinceAirborn())
                )
            {
                JumpHandler.PerformJump(RigidBodyComponent);


                stateMachine.state = QSPlatformerStateMachine.State.Jump;
            }
        }


        private void AdjustJumpHeight()
        {
            JumpHandler.AdjustJumpHeight(RigidBodyComponent);
        }





    }
}