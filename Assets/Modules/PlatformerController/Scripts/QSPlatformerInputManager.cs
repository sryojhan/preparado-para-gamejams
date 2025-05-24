using UnityEngine;

namespace QuickStart.Platformer
{
    [AddComponentMenu("QuickStart platformer/Core/Input manager")]
    public class QSPlatformerInputManager : MonoBehaviour
    {
        /*
            You dont have to use this class type,
            you can use your own input map if you
            alreadt have one
        */
        QSPlatformerInputMap inputMap;

        /*Manage the input map*/
        private void Awake()
        {
            inputMap = new QSPlatformerInputMap();
        }

        private void OnEnable()
        {
            inputMap.Enable();
        }

        private void OnDisable()
        {
            inputMap.Disable();
        }


        /* Expose stick input */
        public float GetHorizontalAxis()
        {
            return inputMap.Player.Move.ReadValue<Vector2>().x;
        }

        public float GetVerticalAxis()
        {
            return inputMap.Player.Move.ReadValue<Vector2>().y;
        }


        public Vector2 GetLeftStick()
        {
            return inputMap.Player.Move.ReadValue<Vector2>();
        }

        public Vector2 GetRightStick()
        {
            return inputMap.Player.Move.ReadValue<Vector2>();
        }


        /* Expose jump input */
        public bool GetJump()
        {
            return inputMap.Player.Jump.ReadValue<float>() > 0.5f;
        }

        public bool WasJumpPressedThisFrame()
        {
            return inputMap.Player.Jump.WasPressedThisFrame();
        }
        public bool WasJumpReleasedThisFrame()
        {
            return inputMap.Player.Jump.WasReleasedThisFrame();
        }
    }
}