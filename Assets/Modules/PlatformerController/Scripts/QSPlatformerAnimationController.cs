using UnityEngine;

namespace QuickStart.Platformer
{
    [AddComponentMenu("QuickStart platformer/Extras/Animation controller")]
    public class QSPlatformerAnimationController : MonoBehaviour
    {
        public string idleAnimation = "Idle";
        public string runAnimation = "Run";
        public string jumpAnimation = "Jump";
        public string fallAnimation = "Fall";

        public void SetFlip(SpriteRenderer spriteRenderer, float horizontalSpeed)
        {
            if(horizontalSpeed > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (horizontalSpeed < -0.01f)
            {
                spriteRenderer.flipX = true;
            }
        }

        public void UpdateAnimation(Animator animator, QSPlatformerStateMachine stateMachine)
        {
            //If the state is the same there is no need to change the animation
            if (!stateMachine.StateHasChanged()) return;

            switch (stateMachine.state)
            {
                case QSPlatformerStateMachine.State.Iddle:
                    animator.Play(idleAnimation);
                    break;
                case QSPlatformerStateMachine.State.Running:
                    animator.Play(runAnimation);
                    break;
                case QSPlatformerStateMachine.State.Jump:
                    animator.Play(jumpAnimation);
                    break;
                case QSPlatformerStateMachine.State.Fall:
                    animator.Play(fallAnimation);
                    break;


                case QSPlatformerStateMachine.State.Slide:
                    break;
                case QSPlatformerStateMachine.State.SuperJump:
                    break;
                case QSPlatformerStateMachine.State.Unknown:
                    break;
                default:
                    break;
            }

        }
    }
}