// QuickStart Platformer (c) 2025 Yojhan Steven Garc�a Pe�a

namespace QuickStart.Platformer
{
    public class QSPlatformerStateMachine
    {
        public enum State
        {
            Iddle, Running, Jump, Fall, Slide, SuperJump,


            Unknown
        }


        private State previousState;
        private State currentState;


        public State state
        {
            get
            {
                return currentState;
            }
            set
            {
                previousState = currentState;
            }

        }



    }

}